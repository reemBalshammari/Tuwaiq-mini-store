using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_store.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Image { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Categories? Category { get; set; }

        public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    }
}