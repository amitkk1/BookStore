using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Genre
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
