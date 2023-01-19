using MultiShop.Models.Base;

namespace MultiShop.Models
{
    public class ProductInformation:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
