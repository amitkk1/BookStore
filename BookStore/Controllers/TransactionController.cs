using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class TransactionController : Controller
    {
       
        private readonly BookStoreContext _context;

        public TransactionController(BookStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Cart()
        {
            return View(await _context.Transactions.ToListAsync());
        }

        public async Task<IActionResult> List()
        {
            return View(await _context.Transactions.ToListAsync());
        }
    }
}
