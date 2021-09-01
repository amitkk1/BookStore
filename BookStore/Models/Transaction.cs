using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        public float Price { get; set; }
        public TransactionStatus Status { get; set; }
        public User Customer { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public PickupLocation PickupLocation { get; set; }
        public ICollection<Book> PurchasedBooks { get; set; }


    }
}
