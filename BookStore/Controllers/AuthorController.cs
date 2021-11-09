using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;




namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        private readonly BookStoreContext _context;

        public AuthorController(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var author = from a in _context.Authors
                           select a;
            if (!String.IsNullOrEmpty(searchString))
            {
                author = author.Where(a => a.Name.Contains(searchString));
                                      
            }
           
            return View(await author
                           .Where(a => a.IsValid == true)
                           .AsNoTracking()
                           .ToListAsync());
        }

        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("ID,Name,Description,BirthDate,DeathDate")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(author);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ID == id.Value);
            if (author == null)
            {
                return NotFound();
            }
          
            return View(author);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorToUpdate = await _context.Authors.FirstOrDefaultAsync(a => a.ID == id);
            if (await TryUpdateModelAsync<Author>(authorToUpdate,
                "",
                a => a.Name, a => a.IsValid, a => a.BirthDate, a => a.DeathDate))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
    
            return View(authorToUpdate);
        }

        

        

    }

    
}
