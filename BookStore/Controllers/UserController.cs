using BookStore.Data;
using BookStore.Models;
using BookStore.Types.Attributes;
using BookStore.Types.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        BookStoreContext _dbContext;
        public UserController(BookStoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            
            
            User user = _dbContext.Users.Include(a => a.Role).FirstOrDefault(dbUser => 
                dbUser.Email.ToLower().Equals(email.ToLower()));

            //if the user doesn't exists in the database, or gave the wrong password
            if(user == null || 
                EncryptWithSalt(password, user.PasswordSalt) != user.EncryptedPassword)
            {
                TempData["ErrorMessage"] = "מייל או סיסמה לא תקינים";
                return Redirect("/user/login");
            }
            else
            {
                var claimPrincipal = GetUserClaimPricipal(user);
                await HttpContext.SignInAsync(claimPrincipal);
            }

            return Redirect("/");
        }

        [HttpPost]
        [Roles(RoleTypes.Customer, RoleTypes.Admin)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string firstName, string lastName, string email, string phoneNumber, string password)
        {
            User user = new User();
            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.PhoneNumber = phoneNumber;
            user.Role = _dbContext.Roles.First(a => a.Name == RoleTypes.Customer);
            user.PasswordSalt = Guid.NewGuid().ToString();
            user.EncryptedPassword = EncryptWithSalt(password, user.PasswordSalt);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return await Login(email, password);
        }

        //calculate a hash for password + salt
        private string EncryptWithSalt(string password, string salt)
        {
            HashAlgorithm sha256 = new SHA256Managed();
            byte[] saltAndPasswordBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hash = sha256.ComputeHash(saltAndPasswordBytes);
            string result = Convert.ToBase64String(hash);
            return result;
        }
        private ClaimsPrincipal GetUserClaimPricipal(User user)
        {

            //claim is basically a key value pair. 
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };
            //claims identity is basically just a way to "group" a list of claims, and saying how those claims should be authenticated
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //claims principal is the security context. since we only have to authenticate the user, it only holds the user claims
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
        
        public async Task<IActionResult> Profile()
        {
            return View();
        }

        public async Task<IActionResult> New()
        {
            return View();
        }
    }
}
