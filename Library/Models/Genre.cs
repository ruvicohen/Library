using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;


namespace Library.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Display(Name = "שם הזאנר")]
        public string Name { get; set; }
        
        public List<Shelf>?  Shelves { get; set; }
    }
}
