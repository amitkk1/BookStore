using BookStore.Models;
using BookStore.Types.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data
{
    public class DbInitializer
    {
        public static void Initialize(BookStoreContext context)
        {
            context.Database.EnsureCreated();

            //if the database already has data, returns
            if (context.Roles.Any())
            {
                return;
            }


            Genre novel = new Genre { Name = " רומן " };
            Genre thriller = new Genre { Name = "מותחן" };

            context.Genres.AddRange(novel, thriller); 
            context.SaveChanges();

            #region Roles
            Role adminRole = new Role() { Name = RoleTypes.Admin };
            Role customerRole = new Role { Name = RoleTypes.Customer };

            context.Roles.AddRange(adminRole, customerRole);
            #endregion

            #region Districts
            District centerDistrict = new District { Name = "מרכז" };
            District northDistrict = new District { Name = "צפון" };
            District haifaDistrict = new District { Name = "חיפה" };
            District telAvivDistrict = new District { Name = "תל אביב" };
            District jerusalemDistrict = new District { Name = "ירושלים" };
            District southDistrict = new District { Name = "דרום" };

            context.Districts.AddRange(
                centerDistrict,
                northDistrict,
                haifaDistrict,
                telAvivDistrict,
                jerusalemDistrict,
                southDistrict
                );
            context.SaveChanges();
            #endregion

            #region Languages
            Language hebrewLang = new Language { Name = "עברית" };
            Language russianLang = new Language { Name = "רוסית" };
            Language englishLang = new Language { Name = "אנגלית" };

            context.AddRange(hebrewLang, englishLang, russianLang);
            context.SaveChanges();
            #endregion

            #region Users
            User user = new User();
            user.Email = "admin@admin.com";
            user.FirstName = "עמית";
            user.LastName = "קסטל";
            user.PhoneNumber = "0545358441";
            user.Role = context.Roles.First(a => a.Name == RoleTypes.Admin);
            user.PasswordSalt = "abcdefg";
            user.EncryptedPassword = "391D04361CE0DDD96B24FA34C0EAA82881D79FD35854C52608FDF38B8223F2DB";
            context.Users.Add(user);
            context.SaveChanges();
            #endregion


        }

    }
}
