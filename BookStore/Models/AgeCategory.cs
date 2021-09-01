using System.Collections.Generic;

namespace BookStore.Models
{
    public class AgeCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
