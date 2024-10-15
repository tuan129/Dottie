using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dottie.Models
{
	public class CartViewModel
	{
		public List<CartItem> CartItems { get; set; } = new List<CartItem>(); 
		public decimal TotalAmount { get; set; }
	}
}