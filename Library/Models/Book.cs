using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Display(Name="שם הספר")]
        public string Name { get; set; }
        [Display(Name = "זאנר")]
        public string? Genre { get; set; }
        [Display(Name = "גובה הספר")]
        public int Height { get; set; }
        [Display(Name = "עובי הספר")]
        public int Width { get; set; }
        [Display(Name = "מספר מדף")]
        public int? ShelfId { get; set; }
        [Display(Name = "מספר מדף")]
        public Shelf? Shelf { get; set; }

        [Display(Name = "שם הסט")]
        public int? SetId { get; set; }
        [Display(Name = "שם הסט")]
        public Set? Set { get; set; }

    }
}
