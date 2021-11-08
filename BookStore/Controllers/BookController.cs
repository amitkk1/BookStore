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
    public class BookController : Controller
    {
        private readonly BookStoreContext _context;

        public BookController(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var book = from b in _context.Books
                           select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                 book = book.Where(b => b.Name.Contains(searchString)
                                       || b.Author.Name.Contains(searchString));
            }
           
            return View(await book
                           .Include(img => img.Picture)
                           .Include(a => a.Author)
                           .AsNoTracking()
                           .ToListAsync());
        }

        public IActionResult Create()
        {
            PopulateGenreDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind("ID,Name,Description,PublishDate,Price,NumberOfPages," +
            "QuantityInStock,AgeCategoryID,GenreID,AuthorID,LanguageID,IsValid")] Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Books.Add(book);
                    _context.SaveChanges();
                    
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateGenreDropDownList(book.Genre);
            return View(book);
        }
        
                   

        public async Task<IActionResult> List(int genreFilter, int ageFilter ,string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var book = from b in _context.Books
                       select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                book = book.Where(b => b.Name.Contains(searchString)
                                      || b.Author.Name.Contains(searchString));
            }
            return View(await book
                          .Include(img => img.Picture)
                          .Include(a => a.Author)
                          .AsNoTracking()
                          .ToListAsync());
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

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(img => img.Picture)
                .Include(a => a.Author)
                .Include(g => g.Genre)
                .Include(c => c.AgeCategory)
                .Include(l => l.Language)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.ID == id.Value); 
            if (book == null)
            {
                return NotFound();
            }
            PopulateGenreDropDownList(book.Genre.ID);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost ]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,PublishDate,Price,NumberOfPages," +
            "QuantityInStock,AgeCategory,Genre,Author,Language,IsValid")] Book book)
        {
            if (id != book.ID)
            {
                return NotFound();
            }
            PopulateGenreDropDownList(book.Genre);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(book).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DataException)
                {
                    //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            
                return RedirectToAction(nameof(Index));
            }
            PopulateGenreDropDownList(book.Genre);
            return View(book);

        }

       /* [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bookToUpdate = _context.Books.Find(id);
            if (TryUpdateModelAsync(bookToUpdate, "",new string[] { "Title", "Credits", "DepartmentID" }))
            {
                try
                {
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Infrastructure.RetryLimitExceededException *//* dex *//*)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateGenreDropDownList(bookToUpdate.Genre.ID);
            return View(bookToUpdate);
        }
*/


        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.ID == id);
        }

        private void PopulateGenreDropDownList(object selectedGenre = null)
        {

            var genreQuery = from b in _context.Books
                             orderby b.Genre.Name
                             select b.Genre;
            ViewBag.GenreID = new SelectList(genreQuery, "ID", "Name", selectedGenre);
        }

    }

    
}
