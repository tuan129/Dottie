//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dottie.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CartItem
    {
        public int cartItemId { get; set; }
        public Nullable<int> cartId { get; set; }
        public Nullable<int> productId { get; set; }
        public int quantity { get; set; }
    
        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual Product Product { get; set; }
    }
}
