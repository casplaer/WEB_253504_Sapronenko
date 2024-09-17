using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253504_Sapronenko.Models;

namespace WEB_253504_Sapronenko.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.LabTitle = "Лабораторная работа 2";

            var selectList = new SelectList(
                new List<ListDemo>
                {
                    new ListDemo {Id = 0, Name = "Выбор 1"},
                    new ListDemo {Id = 1, Name = "Выбор 2"},
                    new ListDemo {Id = 2, Name = "Выбор 3"},
                },
                "Id",
                "Name"
            );

            return View(selectList);
        }

    }
}
