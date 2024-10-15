using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dottie.Models;
using System.Security.Claims;

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

		public ActionResult ProductDetailWithID(int productId)
		{
			var product = database.Products.FirstOrDefault(p => p.productId == productId);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
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
				Session["UserEmail"] = user.email;
				return RedirectToAction("TrangChu");
			}

			ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
			return View();
		}

		/*EndDangNhap*/

		/*Info*/
		public ActionResult infoAccount()
		{
			var email = Session["UserEmail"]?.ToString();
			if (email != null)
			{
				var user = database.Users.FirstOrDefault(u => u.email == email);
				var shippingOrdersCount = database.Orders.Count(el => el.userId == user.userId && el.status == "Đang giao hàng");
				ViewBag.ShippingOrdersCount = shippingOrdersCount;
				return View(user);
			}
			else
			{
				RedirectToAction("error");
			}

			return RedirectToAction("TrangChu");
		}

		[HttpPost]
		public ActionResult logout()
		{
			Session.Clear();
			return RedirectToAction("TrangChu");
		}

		public ActionResult YeuThich()
		{
			var email = Session["UserEmail"]?.ToString();
			if (email != null)
			{
				var user = database.Users.FirstOrDefault(u => u.email == email);
				var favoriteProducts = database.FavoriteItems
					.Where(f => f.Favorite.userId == user.userId)
					.Select(f => f.Product) 
					.ToList();
				return View(favoriteProducts);
			}
			return RedirectToAction("TrangChu");
		}

		[HttpPost]
		public JsonResult AddToFavorites(int productId)
		{
			if (Session["UserEmail"] != null)
			{
				var email = Session["UserEmail"].ToString();
				var user = database.Users.FirstOrDefault(u => u.email == email);

				// Kiểm tra xem người dùng đã có danh sách yêu thích chưa
				var favorite = database.Favorites.FirstOrDefault(f => f.userId == user.userId);
				if (favorite == null)
				{
					favorite = new Favorite
					{
						userId = user.userId
					};
					database.Favorites.Add(favorite);
					database.SaveChanges();
				}

				var existingFavoriteItem = database.FavoriteItems.FirstOrDefault(f => f.favoriteId == favorite.favoriteId && f.productId == productId);
				if (existingFavoriteItem == null)
				{
					var favoriteItem = new FavoriteItem
					{
						favoriteId = favorite.favoriteId,
						productId = productId
					};
					database.FavoriteItems.Add(favoriteItem); 
					database.SaveChanges();
				}
				int favoriteCount = database.FavoriteItems.Count(f => f.favoriteId == favorite.favoriteId);
				Session["FavoriteCount"] = favoriteCount;

				return Json(new { favoriteCount });
			}
			return Json(new { favoriteCount = 0 });
		}

		public class CartViewModel
		{
			public List<CartItem> CartItems { get; set; }
			public decimal TotalAmount { get; set; }
		}

		public ActionResult Cart()
		{
			var email = Session["UserEmail"]?.ToString();
			var user = database.Users.FirstOrDefault(u => u.email == email);

			var cart = database.ShoppingCarts.FirstOrDefault(c => c.userId == user.userId);

			var cartItems = cart != null ? database.CartItems.Where(ci => ci.cartId == cart.cartId).ToList() : new List<CartItem>();

			decimal totalAmount = cartItems.Sum(ci => ci.quantity * database.Products.FirstOrDefault(p => p.productId == ci.productId)?.price ?? 0);

			var model = new CartViewModel
			{
				CartItems = cartItems,
				TotalAmount = totalAmount
			};

			return View(model);
		}

		[HttpPost]
		public ActionResult AddToCart(int productId, int quantity)
		{
			var email = Session["UserEmail"]?.ToString();
			if (email != null)
			{
				var user = database.Users.FirstOrDefault(u => u.email == email);
				var cart = database.ShoppingCarts.FirstOrDefault(c => c.userId == user.userId);

				if (cart == null)
				{
					cart = new ShoppingCart { userId = user.userId };
					database.ShoppingCarts.Add(cart); 
					database.SaveChanges();
				}
				
				var existingItem = database.CartItems.FirstOrDefault(ci => ci.cartId == cart.cartId && ci.productId == productId);

				if (existingItem != null)
				{
					existingItem.quantity += quantity;
					database.Entry(existingItem).State = System.Data.Entity.EntityState.Modified; 
				}
				else
				{
					var newCartItem = new CartItem
					{
						cartId = cart.cartId,
						productId = productId,
						quantity = quantity
					};
					database.CartItems.Add(newCartItem);
				}

				database.SaveChanges(); 

				return Json(new { success = true });
			}

			return Json(new { success = false, message = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng." });
		}
		[HttpPost]
		public ActionResult RemoveFromCart(int cartItemId)
		{
			if (Session["UserEmail"] != null)
			{
				var email = Session["UserEmail"].ToString();
				var user = database.Users.FirstOrDefault(u => u.email == email);
				var cartItem = database.CartItems.FirstOrDefault(ci => ci.cartItemId == cartItemId);
				if (cartItem != null)
				{
					database.CartItems.Remove(cartItem);
					database.SaveChanges();
				}
			}

			return RedirectToAction("Cart"); 
		}

		public ActionResult Thanhtoan(decimal totalAmount)
		{
			var model = new Dottie.Models.CartViewModel
			{
				TotalAmount = totalAmount 
			};

			return View(model);
		}

		[HttpPost]
		public ActionResult HoanTatDonHang(string cus_address, string cus_phone, decimal finalAmount)
		{
			string userEmail = Session["UserEmail"]?.ToString();
			if (!string.IsNullOrEmpty(userEmail))
			{
				var user = database.Users.FirstOrDefault(u => u.email == userEmail);
				if (user != null)
				{
					var cart = database.ShoppingCarts.FirstOrDefault(c => c.userId == user.userId);
					if (cart != null)
					{
						var cartItems = database.CartItems
												.Where(ci => ci.cartId == cart.cartId)
												.ToList();

						var order = new Order
						{
							userId = user.userId,
							shippingAddress = cus_address,
							phoneNumber = cus_phone,
							totalAmount = finalAmount,
							orderDate = DateTime.Now,
							status = "Đang giao hàng"
						};
						database.Orders.Add(order);
						database.SaveChanges();
						ClearCart();
						return RedirectToAction("Trangchu", "Home");
					}
				}
			}
			return RedirectToAction("Error", "Home");
		}

		private void ClearCart()
		{
			var email = Session["UserEmail"]?.ToString();
			if (email != null)
			{
				var user = database.Users.FirstOrDefault(u => u.email == email);
				var cart = database.ShoppingCarts.FirstOrDefault(c => c.userId == user.userId);
				if (cart != null)
				{
					var cartItems = database.CartItems.Where(ci => ci.cartId == cart.cartId).ToList();
					foreach (var item in cartItems)
					{
						database.CartItems.Remove(item);
					}
					database.SaveChanges();
				}
			}
		}
	}
}