using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.DTO
{
    public class CartPriceQueryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
