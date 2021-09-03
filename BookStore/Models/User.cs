using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string EncryptedPassword { get; set; }
        public string PasswordSalt { get; set; }
        public Role Role { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
