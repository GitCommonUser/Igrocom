using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Igrocom.Models;
using Microsoft.AspNetCore.Authentication;
using Igrocom.Data;
using Microsoft.EntityFrameworkCore;


namespace Igrocom.Controllers;

public class HomeController : Controller
{
    private readonly IgrocomContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger,IgrocomContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var topGames = _context.Game.FromSqlRaw("SELECT * FROM return_popular_games()").ToList();

        ViewData["PopularGames"] = new List<Game>(topGames);

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> Logout()
    {

        await HttpContext.SignOutAsync("MyCookie");
        return RedirectToAction("Index");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
