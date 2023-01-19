using MultiShop.Models.Base;

namespace MultiShop.Models
{
    public class Discount:BaseEntity
    {
        public double Price { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
