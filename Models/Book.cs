using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Library.Models
{
    public class Book
    {
        public int BookId { get; set; }

        public string Name { get; set; }


        [ValidateNever]
        public string? ImagePath { get; set; }

        [ValidateNever]
        [NotMapped]
        [DisplayName("Image")]
        public IFormFile ImageFile { get; set; }

        public string Author { get; set; }
        
        public string genre { get; set; }
        
        public bool statue { get; set; }

        public ICollection<AccountBook> AccountBooks { get; set; } = new List<AccountBook>();
    }
}
