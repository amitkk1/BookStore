using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Language
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
