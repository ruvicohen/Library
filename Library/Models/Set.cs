using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Set
    {
        
        public int Id { get; set; }
        [Display(Name = "שם הסט")]
        public string NameSet { get; set; }
        [Display(Name = "שם הזאנר")]
        public string Genre { get; set; }
        [NotMapped]
        public int Height
        {
            get
            {
                return Books != null && Books.Any() ? Books.Max(b => b.Height) : 0; 
            }
        }


        public List<Book>? Books { get; set; }
    }
}
