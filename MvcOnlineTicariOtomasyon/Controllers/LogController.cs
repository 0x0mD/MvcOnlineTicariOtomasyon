using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class LogController : Controller
    {
        [HttpPost]
        public ActionResult SendMessageToConsole(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);

            // JSON olarak mesajı döndürme
            return Json(new { success = true });
        }
    }
}