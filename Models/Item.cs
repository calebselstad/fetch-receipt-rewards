using System.ComponentModel.DataAnnotations;

namespace fetch_receipt_rewards.Models
{
    public class Item
    {
        [Required]
        [RegularExpression(@"^[\w\s\-]+$")]
        public string ShortDescription { get; set; }

        [Required]
        [RegularExpression(@"^\d+\.\d{2}$")]
        public string Price { get; set; }
    }
}
