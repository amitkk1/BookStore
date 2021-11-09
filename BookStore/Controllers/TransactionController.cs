using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using BookStore.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var transaction = from t in _context.Transactions
                              select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                transaction = transaction.Where(t => t.CustomerID.ToString().Contains(searchString)
                                        || t.ID.ToString().Contains(searchString));
                                       
            }

            return View(await transaction
                           .Include(s => s.Status)
                           .Include(p => p.PickupLocation)
                           .AsNoTracking()
                           .ToListAsync());
        }

        public IActionResult Create()
        {
            PopulatePickupLocationDropDownList();
            PopulateTransactionStatuseDropDownList();
            PopulateUserDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("ID,Description,DateOfPurchase,Price," +
            "CustomerID,StatusID,PickupLocationID")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulatePickupLocationDropDownList(transaction.PickupLocationID);
            PopulateTransactionStatuseDropDownList(transaction.StatusID);
            PopulateUserDropDownList(transaction.CustomerID);
            return View(transaction);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.ID == id.Value);
            if (transaction == null)
            {
                return NotFound();
            }
            PopulatePickupLocationDropDownList(transaction.PickupLocationID);
            PopulateTransactionStatuseDropDownList(transaction.StatusID);
            PopulateUserDropDownList(transaction.CustomerID);
            return View(transaction);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionToUpdate = await _context.Transactions.Include(p => p.PickupLocation).FirstOrDefaultAsync(b => b.ID == id);
            if (await TryUpdateModelAsync<Transaction>(transactionToUpdate,
                "",
                b => b.Price, b => b.DateOfPurchase, 
                b => b.CustomerID, b => b.PickupLocationID, b => b.StatusID))
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
            PopulatePickupLocationDropDownList(transactionToUpdate.PickupLocationID);
            PopulateTransactionStatuseDropDownList(transactionToUpdate.StatusID);
            PopulateUserDropDownList(transactionToUpdate.CustomerID);
            return View(transactionToUpdate);
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
                float totalPrice = (await _context.Books
                    .ToListAsync())
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

        
        private void PopulatePickupLocationDropDownList(object selectedDistrict = null)
        {

            var districtQuery = from d in _context.PickupLocations
                             select d;
            ViewBag.DistrictID = new SelectList(districtQuery.AsNoTracking(), "ID", "Name", selectedDistrict);
        }
        private void PopulateTransactionStatuseDropDownList(object selectedTransactionStatuse = null)
        {

            var transactionStatuseQuery = from t in _context.TransactionStatuses
                        orderby t.Name
                        select t;
            ViewBag.TransactionStatuseID = new SelectList(transactionStatuseQuery.AsNoTracking(), "ID", "City", selectedTransactionStatuse);
        }
        private void PopulateUserDropDownList(object selectedUser = null)
        {

            var userQuery = from u in _context.Users
                             orderby u.FirstName
                        select u;
            ViewBag.UserID = new SelectList(userQuery.AsNoTracking(), "ID", "FirstName", selectedUser);
        }
       



    }
}
