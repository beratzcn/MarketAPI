using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string categoryName { get; set; }
        public Category Category { get; set; }
    }
}
