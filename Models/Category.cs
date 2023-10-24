using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MarketAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
}
