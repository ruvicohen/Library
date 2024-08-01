using Library.Models;

namespace Library.ViewModel
{
    public class SetViewModel
    {
        public Set Set { get; set; }
        public List<Book> Books { get; set; }

        public SetViewModel()
        {
            Set = new Set();
            Books = new List<Book>();
        }
    }
}
