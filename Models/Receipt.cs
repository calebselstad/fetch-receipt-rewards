using fetch_receipt_rewards.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace fetch_receipt_rewards.Models
{
    public class Receipt
    {
        [Required]
        [JsonProperty("retailer")]
        public string Retailer { get; set; }

        [Required]
        [JsonProperty("purchaseDate")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [JsonProperty("purchaseTime")]
        [System.Text.Json.Serialization.JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly PurchaseTime { get; set; }

        [Required]
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [Required]
        [JsonProperty("total")]
        public string Total { get; set; }
    }
}
