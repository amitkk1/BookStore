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
            PopulateLanguageDropDownList();
            PopulateAgeCategoryDropDownList();
            PopulateAuthorDropDownList();
            PopulatePictureDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("ID,Name,Description,PublishDate,Price,NumberOfPages," +
            "QuantityInStock,AgeCategoryID,GenreID,AuthorID,LanguageID,PictureID")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateGenreDropDownList(book.GenreID);
            PopulateLanguageDropDownList(book.LanguageID);
            PopulateAgeCategoryDropDownList(book.AgeCategoryID);
            PopulateAuthorDropDownList(book.AuthorID);
            PopulatePictureDropDownList(book.PictureID);
            return View(book);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.ID == id.Value);
            if (book == null)
            {
                return NotFound();
            }
            PopulateGenreDropDownList(book.GenreID);
            PopulateLanguageDropDownList(book.LanguageID);
            PopulateAgeCategoryDropDownList(book.AgeCategoryID);
            PopulateAuthorDropDownList(book.AuthorID);
            PopulatePictureDropDownList(book.PictureID);
            return View(book);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookToUpdate = await _context.Books.FirstOrDefaultAsync(b => b.ID == id);
            if (await TryUpdateModelAsync<Book>(bookToUpdate,
                "",
                b => b.Name, b => b.Description, b => b.PublishDate, b => b.Price,
                b => b.NumberOfPages, b => b.QuantityInStock, b => b.AuthorID, b => b.AgeCategoryID,
                b => b.GenreID, b => b.IsValid, b => b.PictureID, b => b.LanguageID))
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
            PopulateGenreDropDownList(bookToUpdate.GenreID);
            PopulateLanguageDropDownList(bookToUpdate.LanguageID);
            PopulateAgeCategoryDropDownList(bookToUpdate.AgeCategoryID);
            PopulateAuthorDropDownList(bookToUpdate.AuthorID);
            PopulatePictureDropDownList(bookToUpdate.PictureID);
            return View(bookToUpdate);
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
       
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.ID == id);
        }

        private void PopulateGenreDropDownList(object selectedGenre = null)
        {

            var genreQuery = from g in _context.Genres
                             orderby g.Name
                             select g;
            ViewBag.GenreID = new SelectList(genreQuery.AsNoTracking(), "ID", "Name", selectedGenre);
        }

        private void PopulateAuthorDropDownList(object selectedGenre = null)
        {

            var genreQuery = from g in _context.Authors
                             orderby g.Name
                             select g;
            ViewBag.AuthorID = new SelectList(genreQuery.AsNoTracking(), "ID", "Name", selectedGenre);
        }

        private void PopulateAgeCategoryDropDownList(object selectedGenre = null)
        {

            var genreQuery = from g in _context.AgeCategories
                             orderby g.Name
                             select g;
            ViewBag.AgeCategoryID = new SelectList(genreQuery.AsNoTracking(), "ID", "Name", selectedGenre);
        }
        private void PopulateLanguageDropDownList(object selectedGenre = null)
        {

            var genreQuery = from g in _context.Languages
                             orderby g.Name
                             select g;
            ViewBag.LanguageID = new SelectList(genreQuery.AsNoTracking(), "ID", "Name", selectedGenre);
        }
        private void PopulatePictureDropDownList(object selectedGenre = null)
        {

            var genreQuery = from g in _context.Pictures
                             orderby g.Url
                             select g;
            ViewBag.PictureID = new SelectList(genreQuery.AsNoTracking(), "ID", "Url", selectedGenre);
        }

    }

    
}
