using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class PickupLocation
    {
        public int ID { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public District District { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
