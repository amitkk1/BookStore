using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Picture
    {
        [Key]
        public int ID { get; set; }
        
        //public byte[] PictureData { get; set; }

        public string Url { get; set; }

        public bool IsValid { get; set; } = true;
    }
}
