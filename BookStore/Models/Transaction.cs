using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        [Required]
        public float Price { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime DateOfPurchase { get; set; }
        [Required]
        public int StatusID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public int PickupLocationID { get; set; }
        public ICollection<Book> PurchasedBooks { get; set; }

        public User Customer { get; set; }
        public PickupLocation PickupLocation { get; set; }
        public TransactionStatus Status { get; set; }


    }
}
