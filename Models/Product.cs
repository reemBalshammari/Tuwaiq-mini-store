using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_store.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المنتج مطلوب ولا يمكن تركه فارغ")]
        [StringLength(100, ErrorMessage = "اسم المنتج طويل جداً الحد الأقصى 100 حرف")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "الرجاء تحديد سعر المنتج")]
        [Range(0.01, 10000.00, ErrorMessage = "يجب أن يكون السعر قيمة موجبة بين 0.01 و 10000")]
        public decimal Price { get; set; }

       public string Image { get; set; } = string.Empty;

[NotMapped]
[Required(ErrorMessage = "يرجى اختيار صورة المنتج")]
public IFormFile? ImageFile { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Categories? Category { get; set; }

        public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    }
}