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
    
    public partial class FavoriteItem
    {
        public int favoriteItemId { get; set; }
        public Nullable<int> favoriteId { get; set; }
        public Nullable<int> productId { get; set; }
    
        public virtual Favorite Favorite { get; set; }
        public virtual Product Product { get; set; }
    }
}
