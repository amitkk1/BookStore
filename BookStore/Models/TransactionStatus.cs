﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class TransactionStatus
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
