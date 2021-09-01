using BookStore.Enums;
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
        public byte[] EncryptedPassword { get; set; }
        public byte[] PasswordSalt { get; set; }
        public EPermission Permissions { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
