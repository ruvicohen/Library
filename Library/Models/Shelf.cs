using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Shelf
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "גובה המדף")]
        public int Height {  get; set; }
        [Display(Name = "אורך המדף")]
        public int Width { get; set; }
        [Display(Name = "זאנר")]
        public int GenreId { get; set; }
        [Display(Name = "זאנר")]
        public Genre? Genre { get; set; }
        public List<Book>? Books { get; set; }
    }
}
