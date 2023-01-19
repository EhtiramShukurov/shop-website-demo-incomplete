using MultiShop.Models.Base;

namespace MultiShop.Models
{
    public class Offer:BaseEntity
    {
        public string PrimaryTitle { get; set; }
        public string SecondaryTitle { get; set;}
        public string ImageUrl { get; set; }
    }
}
