using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Igrocom.Data;
using Igrocom.Models;
using Microsoft.AspNetCore.Authorization;

namespace Igrocom.Controllers
{
    [Authorize]
    public class ContentController : Controller
    {
        private readonly IgrocomContext _context;

        public ContentController(IgrocomContext context)
        {
            _context = context;
        }

        // GET: Content
        public async Task<IActionResult> Index(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                return View(await _context.Content.Where(g => g.Title.ToLower().Contains(searchString.ToLower())).ToListAsync());
            }

            return View(await _context.Content.ToListAsync());
        }

        // GET: Content/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            var file = from f in _context.MediaFiles where f.ContentId == id select f;
            ViewData["Files"] = new List<MediaFiles>(file);

            ViewData["IsFavorite"] = false;
            if (User.Identity.IsAuthenticated)
            {
                var favContent = from c in _context.UserContent where c.ContentId == id && c.UserId ==  GetCurrentUser() select c;

                if (favContent.ToList().Count > 0)
                {
                    ViewData["IsFavorite"] = true;
                }
            }


            return View(content);
        }

        // GET: Content/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Content content, IFormFile imageData, List<IFormFile> mediaFilesData)
        {

            if (imageData != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageData.CopyTo(memoryStream);
                    content.Image = memoryStream.ToArray();
                    content.ImageMime = imageData.ContentType;
                }
            }

            if (mediaFilesData != null)
            {
                content.MediaFiles = new List<MediaFiles>();

                var removeFiles = from f in _context.MediaFiles where f.ContentId == content.Id select f;
                if (removeFiles != null)
                {
                    _context.MediaFiles.RemoveRange(removeFiles);
                }

                for (int i = 0; i < mediaFilesData.Count; i++)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await mediaFilesData[i].CopyToAsync(memoryStream);
                        content.MediaFiles.Add(new MediaFiles { File = memoryStream.ToArray(), FileMime = mediaFilesData[i].ContentType, ContentId = content.Id });
                    }
                }
            }

            content.ReleaseDate = DateOnly.FromDateTime(DateTime.Now);
            ModelState.Remove("UserContent");


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(content);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(content.Id))
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

            return View(content);
        }

        // GET: Content/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            return View(content);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, Content content, IFormFile imageData, List<IFormFile> mediaFilesData)
        {
            if (id != content.Id)
            {
                return NotFound();
            }

            var existingContent = _context.Content.Include(g => g.MediaFiles).FirstOrDefault(g => g.Id == id);

            if (existingContent == null)
            {
                return NotFound();
            }

            existingContent.Title = content.Title;
            existingContent.Preface = content.Preface;
            existingContent.Text = content.Text;

            if (imageData != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageData.CopyTo(memoryStream);
                    existingContent.Image = memoryStream.ToArray();
                    existingContent.ImageMime = imageData.ContentType;
                }
            }

            if (mediaFilesData.Count > 0)
            {
                foreach (var existingFile in existingContent.MediaFiles)
                {
                    _context.MediaFiles.Remove(existingFile);
                }

                for (int i = 0; i < mediaFilesData.Count; i++)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await mediaFilesData[i].CopyToAsync(memoryStream);
                        existingContent.MediaFiles.Add(new MediaFiles { File = memoryStream.ToArray(), FileMime = mediaFilesData[i].ContentType, ContentId = id });
                    }
                }
            }



            ModelState.Remove("imageData");
            ModelState.Remove("UserContent");


            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(content.Id))
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

            return View(content);


        }

        // GET: Content/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // POST: Content/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var content = await _context.Content.FindAsync(id);
            Console.WriteLine("HERE");
            if (content != null)
            {
                Console.WriteLine("HERE2");
                var usrContent = _context.UserContent.Where(ug => ug.ContentId == id);

                _context.Content.Remove(content);
                try
                {
                    _context.Content.Remove(content);
                    await _context.SaveChangesAsync();
                    _context.UserContent.RemoveRange(usrContent.ToList());
                    await _context.SaveChangesAsync();
                }
                catch
                {

                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ContentExists(int id)
        {
            return _context.Content.Any(e => e.Id == id);
        }


        [HttpGet]
        public IActionResult GetImage(int id)
        {
            var content = _context.Content.Find(id);
            if (content?.Image != null && content?.ImageMime != null)
            {
                return File(content?.Image, content?.ImageMime);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavorites(int id)
        {
            var userId = GetCurrentUser();

            // Проверка, если игра уже в избранном
            if (!_context.UserContent.Any(ug => ug.UserId == userId && ug.ContentId == id))
            {
                var userContent = new UserContent
                {
                    UserId = userId,
                    ContentId = id
                };

                _context.UserContent.Add(userContent);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromFavorites(int id)
        {
            var userId = GetCurrentUser();

            // Проверка, если статья в избранном
            if (_context.UserContent.Any(ug => ug.UserId == userId && ug.ContentId == id))
            {
                var usrContent = _context.UserContent.FirstOrDefault(ug => ug.UserId == userId && ug.ContentId == id);
                _context.UserContent.Remove(usrContent);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
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
