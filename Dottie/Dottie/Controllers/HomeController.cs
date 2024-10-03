using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dottie.Models;

namespace Dottie.Controllers
{
	public class HomeController : Controller
	{
        private readonly dottieEntities _context; 

        public HomeController()
        {
            _context = new dottieEntities();
        }

        public ActionResult TrangChu()
		{
            return View();
        }

        public ActionResult register()
		{
			return View();
		}

        public ActionResult yeuthich ()
		{
            return View();
		}
	}
}