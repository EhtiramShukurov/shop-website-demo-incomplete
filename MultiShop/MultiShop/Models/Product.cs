using MultiShop.Models.Base;

namespace MultiShop.Models
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public int CategoryId { get; set; }
        public int? DiscountId { get; set; }
        public int InformationId { get; set; }
        public Discount? Discount { get; set; }
        public Category? Category { get; set; }
        public ProductInformation? Information { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }

		public ICollection<ProductColor>? ProductColors { get; set; }
        public ICollection<ProductSize>? ProductSizes { get; set; }
    }
}
