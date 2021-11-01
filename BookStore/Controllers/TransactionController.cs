using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models.DTO;
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

        public async Task<ActionResult<float>> GetCartPrice([FromBody] IEnumerable<CartPriceQueryDTO> cart)
        {
            try
            {
                float totalPrice = _context.Books
                    .AsEnumerable()
                    .Join(cart,
                        dbBook => dbBook.ID,
                        cartBook => cartBook.ID,
                        (dbBook, cartBook) => new { Price = dbBook.Price, Count = cartBook.Count })
                    .Select(a => a.Count * a.Price)
                    .Sum();

                return new ActionResult<float>(totalPrice);
            } catch(Exception ex)
            {
                return new ActionResult<float>(-1);
            }
        }


    }
}
