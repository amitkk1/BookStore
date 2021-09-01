using System;
using System.Collections;
using System.Collections.Generic;

namespace BookStore.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public float Price { get; set; }
        public Picture Picture { get; set; }
        public int NumberOfPages { get; set; }
        public int QuantityInStock { get; set; }
        public AgeCategory AgeCategory { get; set; }
        public Genre Genre { get; set; }
        public Author Author { get; set; }
        public Language Language { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public bool IsValid { get; set; } = true;
    }
}