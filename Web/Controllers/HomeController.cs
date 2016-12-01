using Grabber.Server.TrainTicket;
using MessageBomber.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TracySecretTool.EF;
using TracySecretTool.Server;
using Web.AuthorizeAttr;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string aa)
        {
            int a = 1, b = 0;
            int c = a / b;
            return View();
        }
    }
}
