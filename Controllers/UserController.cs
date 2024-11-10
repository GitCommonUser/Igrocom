
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Igrocom.Data;
using Igrocom.Models;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;


namespace Igrocom.Controllers
{
    public class UserController : Controller
    {
        private readonly IgrocomContext _context;

        public UserController(IgrocomContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                return View(await _context.User.Where(g => g.Login.ToLower().Contains(searchString.ToLower())).ToListAsync());
            }

            return View(await _context.User.ToListAsync());
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            user.Role = Models.User.Roles.common;

            if(_context.User.Any(e => e.Login == user.Login))
            {
                ModelState.AddModelError("Login","Этот логин уже занят !");
            }

            ModelState.Remove("UserContent");
            ModelState.Remove("UserGame");


            if (ModelState.IsValid)
            {
                user.Password = Hash(user.Password);

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }

            return View();
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            string hashedPassword = Hash(password);
            var user = _context.User.FirstOrDefault(u => u.Login == login && u.Password == hashedPassword);

            if(user != null)
            {

                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.Name, login),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, "MyCookie");
                await HttpContext.SignInAsync("MyCookie", new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index","Home");
            }
            ModelState.AddModelError("Login","Не верный логин или пароль !");
            return View();


        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var games = await _context.UserGame.Where(ug => ug.UserId == id).Select(ug => ug.Game).ToListAsync();
            ViewData["Games"] = new List<Game>(games);

            var contents = await _context.UserContent.Where(ug => ug.UserId == id).Select(ug => ug.Content).ToListAsync();
            ViewData["Contents"] = new List<Content>(contents);


            return View(user);
        }

        #region CREATE


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (_context.User.Any(e => e.Login == user.Login))
            {
                ModelState.AddModelError("Login", "Этот логин уже занят !");
            }

            if (ModelState.IsValid)
            {
                user.Password = Hash(user.Password);

                _context.Add(user);
                await _context.SaveChangesAsync();

            }
            return View(user);
        }

        #endregion

        #region EDIT

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User usr)
        {
            if(id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(usr.Login))
            {
                user.Login = usr.Login;
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        #endregion



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                var usrGame = _context.UserGame.Where(ug => ug.UserId == id);
                _context.UserGame.RemoveRange(usrGame.ToList());

                var usrContent = _context.UserContent.Where(ug => ug.UserId == id);
                _context.UserContent.RemoveRange(usrContent.ToList());

                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ContentExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        private string Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputData = Encoding.UTF8.GetBytes(input);
                byte[] hashData = md5.ComputeHash(inputData);

                StringBuilder builder = new StringBuilder();
                for( int i = 0; i < hashData.Length; i++)
                {
                    builder.Append(hashData[i].ToString("X2"));
                }

                return builder.ToString();
            }
        }


    }
}
