using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using BookStore.Data;
using Microsoft.EntityFrameworkCore;


namespace BookStore.Controllers
{
    public class PickUpLocationsController : Controller
    {
        private readonly BookStoreContext _dbContext;

        public PickUpLocationsController(BookStoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
