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




            #region Roles
            Role adminRole = new Role() { Name = RoleTypes.Admin };
            Role customerRole = new Role() { Name = RoleTypes.Customer };

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


            #region Genre
            Genre novel = new Genre { Name = " רומן " };
            Genre thriller = new Genre { Name = "מותחן" };

            context.Genres.AddRange(novel, thriller);
            context.SaveChanges();
            #endregion

            #region Picture
            Picture picture = new Picture();
            picture.Url = "https://upload.wikimedia.org/wikipedia/he/thumb/5/56/%D7%A4%D7%A8%D7%A1%D7%99_%D7%92%27%D7%A7%D7%A1%D7%95%D7%9F_%D7%95%D7%94%D7%90%D7%9C%D7%95%D7%9E%D7%A4%D7%99%D7%99%D7%9D.jpg/200px-%D7%A4%D7%A8%D7%A1%D7%99_%D7%92%27%D7%A7%D7%A1%D7%95%D7%9F_%D7%95%D7%94%D7%90%D7%9C%D7%95%D7%9E%D7%A4%D7%99%D7%99%D7%9D.jpg";
            context.Pictures.AddRange(picture);
            context.SaveChanges();
            #endregion

            #region Author
            Author author = new Author();
            author.Name = "ריק ריירדן";
            author.BirthDate = new DateTime(1964, 6, 5, 0, 0, 0);
            author.Description = "ריק ריירדן הוא סופר נוער אמריקני עטור פרסים. כנער הִרבּה לקרוא סיפורי מיתולוגיה וספרי פנטזיה ומדע בדיוני.";
            context.Authors.Add(author);
            context.SaveChanges();
            #endregion

            #region Age Catagory
            AgeCategory ageCategory = new AgeCategory();
            ageCategory.Name = "נוער";
            context.AgeCategories.Add(ageCategory);
            context.SaveChanges();
            #endregion

            #region Books
            Book book = new Book();
            book.AgeCategory = ageCategory;
            book.Name = "פרסי גקסון וגנב הברק";
            book.Picture = new Picture();
            book.Description = "הספר הוא הרומן הראשון בסדרת פרסי ג'קסון והאולימפיים המבוססת על המיתולוגיה היוונית, שמספרת את עלילותיו הבדיוניות של האל למחצה – פרסי ג'קסון.";
            book.PublishDate = new DateTime(2008, 6, 28, 0, 0, 0);
            book.Price = 113;
            book.NumberOfPages = 375;
            book.QuantityInStock = 4;
            book.Language = hebrewLang;
            book.Genre = novel;
            context.Books.Add(book);
            context.SaveChanges();
            #endregion
        }

    }
}