using System.ComponentModel.DataAnnotations;

namespace mini_store.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;
    }
}