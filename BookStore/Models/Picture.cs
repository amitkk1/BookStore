using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Picture
    {
        public int ID { get; set; }
        
        //public byte[] PictureData { get; set; }

        public string url { get; set; }

        public bool IsValid { get; set; } = true;
    }
}
