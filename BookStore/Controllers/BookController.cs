﻿using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly BookStoreContext _context;

        public BookController(BookStoreContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Index(int genreFilter, int ageFilter)
        {
            var books = await _context.Books
                .Include(img => img.Picture)
                .Include(a => a.Author)
                .ToListAsync();
            return View(books);
        }
            
        [HttpPost]  
        public async Task<IActionResult> ByGenre(int genreFilter, int ageFilter)
        {
            var books = await _context.Books
                .Include(img => img.Picture)
                .Include(a => a.Author)
                .ToListAsync();
            if (genreFilter > 0 && ageFilter > 0)
            {
                books = await _context.Books
               .Include(img => img.Picture)
               .Include(a => a.Author)
               .Include(g => g.Genre).Where(b => b.Genre.ID == genreFilter)
               .Include(g => g.AgeCategory).Where(b => b.AgeCategory.ID == ageFilter)
               .ToListAsync();

            }else if(genreFilter > 0)
            {
                books = await _context.Books
               .Include(img => img.Picture)
               .Include(a => a.Author)
               .Include(g => g.Genre).Where(b => b.Genre.ID == genreFilter)
               .ToListAsync();
            }
            else if (ageFilter > 0)
            {
                books = await _context.Books
               .Include(img => img.Picture)
               .Include(a => a.Author)
               .Include(g => g.AgeCategory).Where(b => b.AgeCategory.ID == ageFilter)
               .ToListAsync();
            }
            return PartialView("listPartial",books);
        }

        public JsonResult GetGenreList()
        {
            List<Genre> genreList = _context.Genres.ToList();
            return Json(genreList);
        }

        public JsonResult GetAgeCategoryList()
        {
            List<AgeCategory> ageCategoryList = _context.AgeCategories.ToList();
            return Json(ageCategoryList);
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(img => img.Picture)
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


    }
}
