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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.Include(a => a.Author).Include(p => p.Picture).ToListAsync());
            
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            return View(await _context.Books.ToListAsync());
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
                .Include(a => a.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


    }
}
