
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Igrocom.Data;
using Igrocom.Models;
using Microsoft.AspNetCore.Authorization;


namespace Igrocom.Controllers
{
    public class GameController : Controller
    {
        private readonly IgrocomContext _context;

        public GameController(IgrocomContext context)
        {
            _context = context;
        }

        // GET: Content
        public async Task<IActionResult> Index(string searchString)
        {
            if(!String.IsNullOrEmpty(searchString))
            {
                return View(await _context.Game.Where(g => g.Title.ToLower().Contains(searchString.ToLower())).ToListAsync());
            }

            return View(await _context.Game.ToListAsync());
        }

        public async Task<IActionResult> SetRating(string ratingValue, string gameId)
        {
            int gId = int.Parse(gameId);
            if (!String.IsNullOrEmpty(ratingValue))
            {
                try
                {
                    int rating = int.Parse(ratingValue);
                    if(rating > 0 && rating <= 100)
                    {

                        // Если оценка уже была
                        if (_context.Rating.Any(r => r.GameId == gId && r.UserId == GetCurrentUser()))
                        {
                            var existingRating = _context.Rating.Where(r => r.GameId == gId && r.UserId == GetCurrentUser()).FirstOrDefault();
                            existingRating.Value = rating;

                            _context.Rating.Update(existingRating);
                        }
                        else
                        {
                            Rating newRating = new Rating();
                            newRating.GameId = gId;
                            newRating.UserId = GetCurrentUser();
                            newRating.Value = rating;

                            _context.Rating.Add(newRating);
                        }

                        await _context.SaveChangesAsync();
                    }
                }
                catch
                {
                    ModelState.AddModelError("Укажите число !", ratingValue);
                }
            }

            return RedirectToAction("Details", new { id = gId });

        }

        // GET: Content/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            var file = from f in _context.GameFiles where f.GameId==id select f;
            ViewData["Files"] = new List<GameFiles>(file);


            ViewData["IsFavorite"] = false;
            ViewData["UserRating"] = -1;
            if (User.Identity.IsAuthenticated)
            {
                var favGame = from g in _context.UserGame where g.GameId==id && g.UserId == GetCurrentUser() select g;
                if(favGame.ToList().Count > 0)
                {
                    ViewData["IsFavorite"] = true;
                }

                if(_context.Rating.Any(r => r.GameId == id && r.UserId == GetCurrentUser()))
                {
                    ViewData["UserRating"] = _context.Rating.Where(r => r.GameId == id && r.UserId == GetCurrentUser()).Select(r => r.Value).FirstOrDefault();
                }
                

                var games = await _context.UserGame.Where(ug => ug.UserId == id).Select(ug => ug.Game).ToListAsync();
                ViewData["Games"] = new List<Game>(games);

            }



            return View(game);
        }

        #region CREATE

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Game game, IFormFile imageData, List<IFormFile> gameFilesData)
        {

            if(imageData != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageData.CopyTo(memoryStream);
                    game.Image = memoryStream.ToArray();
                    game.ImageMime = imageData.ContentType;
                }
            }

            if (gameFilesData != null)
            {
                game.GameFiles = new List<GameFiles>();

                var removeFiles = from f in _context.GameFiles where f.GameId == game.Id select f;
                if (removeFiles != null)
                {
                    _context.GameFiles.RemoveRange(removeFiles);
                }

                for (int i = 0; i < gameFilesData.Count; i++)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await gameFilesData[i].CopyToAsync(memoryStream);
                        game.GameFiles.Add(new GameFiles {File = memoryStream.ToArray(), FileMime = gameFilesData[i].ContentType, FileName = gameFilesData[i].FileName, GameId = game.Id});
                    }
                }
            }

            ModelState.Remove("UserGame");
            ModelState.Remove("Ratings");

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            // Выводим ошибки в лог или передаем их в представление
            foreach (var error in errors)
            {
                Console.WriteLine(error); // Или используйте логирование
            }

            game.Rating = 1;


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(game);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(game);
        }

        #endregion

        #region EDIT

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, Game game,IFormFile imageData, List<IFormFile> gameFilesData)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            var existingGame = _context.Game.Include(g => g.GameFiles).FirstOrDefault(g => g.Id == id);

            if(existingGame == null)
            {
                return NotFound();
            }

            existingGame.Title = game.Title;
            existingGame.Description = game.Description;
            existingGame.Genre = game.Genre;
            existingGame.Peculiarities = game.Peculiarities;
            existingGame.ReleaseDate = game.ReleaseDate;
            existingGame.Review = game.Review;

            if(imageData != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageData.CopyTo(memoryStream);
                    existingGame.Image = memoryStream.ToArray();
                    existingGame.ImageMime = imageData.ContentType;
                }
            }

            if(gameFilesData.Count > 0)
            {
                foreach(var existingFile in existingGame.GameFiles)
                {
                    _context.GameFiles.Remove(existingFile);
                }

                for (int i = 0; i < gameFilesData.Count; i++)
                {
                    using(var memoryStream = new MemoryStream()){
                        await gameFilesData[i].CopyToAsync(memoryStream);
                        existingGame.GameFiles.Add(new GameFiles{File = memoryStream.ToArray(), FileMime = gameFilesData[i].ContentType, FileName = gameFilesData[i].FileName, GameId = id});
                    }
                }
            }

            ModelState.Remove("imageData");
            ModelState.Remove("UserGame");
            ModelState.Remove("Ratings");

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            // Выводим ошибки в лог или передаем их в представление
            foreach (var error in errors)
            {
                Console.WriteLine(error); // Или используйте логирование
            }




            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(game);


        }

        #endregion



        [HttpGet]
        public IActionResult GetImage(int id){

            var files = _context.GameFiles.Where(f => f.GameId == id).ToList();

            if(files.Count > 0)
            {
                return File(files[0].File, files[0].FileMime);
            }

            return null;
        }

        [HttpGet]
        public IActionResult GetMainImage(int id)
        {
            var game = _context.Game.Find(id);
            if(game?.Image != null && game?.ImageMime != null){
                return File(game?.Image, game?.ImageMime);
            }
            else{
                return null;
            }
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.FindAsync(id);
            if (game != null)
            {
                var usrGame = _context.UserGame.Where(ug => ug.GameId == id);
                _context.UserGame.RemoveRange(usrGame.ToList());

                _context.Game.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavorites(int id)
        {
            var userId = GetCurrentUser();

            // Проверка, если игра уже в избранном
            if (!_context.UserGame.Any(ug => ug.UserId == userId && ug.GameId == id))
            {
                var userGame = new UserGame
                {
                    UserId = userId,
                    GameId = id
                };

                _context.UserGame.Add(userGame);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromFavorites(int id)
        {
            var userId = GetCurrentUser();

            // Проверка, если игра уже в избранном
            if (_context.UserGame.Any(ug => ug.UserId == userId && ug.GameId == id))
            {
                var usrGame = _context.UserGame.FirstOrDefault(ug => ug.UserId == userId && ug.GameId == id);
                _context.UserGame.Remove(usrGame);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }




        private bool ContentExists(int id)
        {
            return _context.Game.Any(e => e.Id == id);
        }

        private int GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            }

            return -1;
        }
        
    }
}





