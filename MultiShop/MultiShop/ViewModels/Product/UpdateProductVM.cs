using MultiShop.Models;
using MultiShop.Models.Base;

namespace MultiShop.ViewModels
{
    public class UpdateProductVM:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public IFormFile? CoverImage { get; set; }
        public ICollection<IFormFile>? OtherImages { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }
        public int CategoryId { get; set; }
        public int? DiscountId { get; set; }
        public int InformationId { get; set; }
        public List<int> ColorIds { get; set; }
        public List<int> SizeIds { get; set; }
        public List<int> ImageIds { get; set; }
    }
}
