using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Book
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }
        public float Price { get; set; }
        public int NumberOfPages { get; set; }
        public int QuantityInStock { get; set; }
        public bool IsValid { get; set; } = true;
        public int PictureID { get; set; }
        public int AgeCategoryID { get; set; }
        public int GenreID { get; set; }
        public int AuthorID { get; set; }
        public int LanguageID { get; set; }
        public ICollection<Transaction> Transactions { get; set; }



        public Picture Picture { get; set; }
        public AgeCategory AgeCategory { get; set; }
        public Genre Genre { get; set; }
        public Author Author { get; set; }
        public Language Language { get; set; }
    }
}