using System;

namespace LaGranLucha.API
{
    public class CartItem : IEquatable<CartItem>
    {
        #region PROPERTIES

        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public Variant Product { get; set; }
        public float UnitPrice { get { return Product.Price; } }
        public float TotalPrice { get { return UnitPrice * Quantity; } }

        #endregion

        #region BEHAVIORS

        public CartItem(Variant product)
        {
            Product = product;
            VariantId = product.Id;
        }

        public bool Equals(CartItem item)
        {
            return item.VariantId == VariantId;
        }

        #endregion
    }
}
