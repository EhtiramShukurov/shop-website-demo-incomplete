using MultiShop.Models.Base;

namespace MultiShop.Models
{
    public class Slider:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public string ImageUrl { get; set; }
    }
}
