using BookStore.Data;
using BookStore.Models;
using BookStore.Types.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly BookStoreContext _context;

        public AnalyticsController(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (!User.IsInRole(RoleTypes.Admin))
            {
                return Redirect("/");
            }

            int totalBooksPurchased = _context.Transactions
                .Include(a => a.PurchasedBooks)
                .ToList()
                .Sum(a => a.PurchasedBooks.Count);

            var TopBooks = _context.Transactions
                .Include(a => a.PurchasedBooks)
                .SelectMany(a => a.PurchasedBooks)
                .GroupBy(a => a.ID)
                .Select(a => new { ID = a.Key, Count = a.Count() })
                .OrderByDescending(a => a.Count)
                .Take(10)
                .ToList()
                .Join(_context.Books,
                    a => a.ID,
                    b => b.ID,
                    (countResult, bookResult) => new
                    {
                        Count = countResult.Count,
                        Name = bookResult.Name
                    })
                .ToList();


            TopBooks.Add(new {Count = totalBooksPurchased - TopBooks.Count, Name = "שאר הספרים" });
            TempData["topBooks"] = JsonConvert.SerializeObject(TopBooks);

            var topGenres = _context.Transactions
                .Include(a => a.PurchasedBooks)
                .ToList()
                .SelectMany(a => a.PurchasedBooks)
                .GroupBy(a => a.Genre)
                .Select(a => new { Name = a.Key, Count = a.Count() })
                .OrderByDescending(a => a.Count);

            TempData["topGenres"] = JsonConvert.SerializeObject(topGenres);


            return View();
        }
    }
}
