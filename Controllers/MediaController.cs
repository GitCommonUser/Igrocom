using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace Igrocom.Controllers;

public class MediaController : Controller
{
    // 
    // GET: /HelloWorld/
    public IActionResult Index()
    {
        return View();
    }
    // 
    // GET: /HelloWorld/Welcome/ 
    //Отвечает за отображение отдельного контента, т.е открывает отдельную страницу с выбранным контентом (статья, интервью, обзор тд тп)
    public IActionResult Content(string id, int numTimes = 1)
    {
        ViewData["Message"] = "Контент номер - " + id; // id самого медиа контента, также string поменять на int
        ViewData["NumTimes"] = numTimes; //Удалить в последствии
        return View();
    }
}