using System.Collections.Generic;

namespace BookStore.Models
{
    public class District
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<PickupLocation> PickupLocations { get; set; }
    }
}
