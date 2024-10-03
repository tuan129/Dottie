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
		private readonly dottieEntities3 database; 

		public HomeController()
		{
			database = new dottieEntities3();
		}

		public ActionResult TrangChu()
		{
			var products = database.Products.ToList();
			return View(products);
		}

		public ActionResult producDetails()
		{
			return View();
		}

/*Đăng ký*/
		public ActionResult register()
		{
			return View();
		}

		[HttpPost]
		public ActionResult register(Dottie.Models.User user)
		{
			if (ModelState.IsValid)
			{
				try
				{
					database.Users.Add(user);
					database.SaveChanges();
					return RedirectToAction("TrangChu");
				}
				catch
				{
					return Content("Error while registering.");
				}
			}

			return View(user);
		}
		/*End đăng ký*/

		/*Đăng nhập*/

		[HttpPost]
		public ActionResult login(string email, string password)
		{
			var user = database.Users.FirstOrDefault(u => u.email == email && u.password == password);
			if (user != null)
			{
				// Đăng nhập thành công, có thể lưu thông tin vào session hoặc cookie nếu cần
				Session["UserEmail"] = user.email; // Ví dụ lưu email vào session
				return RedirectToAction("TrangChu");
			}

			ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
			return View();
		}

		public ActionResult yeuthich ()
		{
			return View();
		}
	}
}