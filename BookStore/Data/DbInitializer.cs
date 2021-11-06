
using BookStore.Models;
using BookStore.Types.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            HashAlgorithm sha256 = new SHA256Managed();
            byte[] saltAndPasswordBytes = Encoding.UTF8.GetBytes("password" + user.PasswordSalt);
            byte[] hash = sha256.ComputeHash(saltAndPasswordBytes);
            user.EncryptedPassword = Convert.ToBase64String(hash);
            context.Users.Add(user);
            context.SaveChanges();

            
            #endregion

            #region Genre
            Genre novel = new Genre { Name = " רומן " };
            Genre thriller = new Genre { Name = "מותחן" };
            Genre adventures = new Genre { Name = "הרפתקאות" };

            context.Genres.AddRange(novel, thriller, adventures);
            context.SaveChanges();
            #endregion

            #region Picture
            Picture percyJacksonPicture = new Picture();
            percyJacksonPicture.Url = "https://upload.wikimedia.org/wikipedia/he/thumb/5/56/%D7%A4%D7%A8%D7%A1%D7%99_%D7%92%27%D7%A7%D7%A1%D7%95%D7%9F_%D7%95%D7%94%D7%90%D7%9C%D7%95%D7%9E%D7%A4%D7%99%D7%99%D7%9D.jpg/200px-%D7%A4%D7%A8%D7%A1%D7%99_%D7%92%27%D7%A7%D7%A1%D7%95%D7%9F_%D7%95%D7%94%D7%90%D7%9C%D7%95%D7%9E%D7%A4%D7%99%D7%99%D7%9D.jpg";

            Picture theLionPicture = new Picture();
            theLionPicture.Url = "https://upload.wikimedia.org/wikipedia/he/thumb/1/1e/%D7%94%D7%90%D7%A8%D7%99%D7%94_%D7%A9%D7%90%D7%94%D7%91_%D7%AA%D7%95%D7%AA.jpg/375px-%D7%94%D7%90%D7%A8%D7%99%D7%94_%D7%A9%D7%90%D7%94%D7%91_%D7%AA%D7%95%D7%AA.jpg";

            Picture goneForGoodPicture = new Picture();
            goneForGoodPicture.Url = "https://upload.wikimedia.org/wikipedia/he/b/b6/GONEFORGOOD.jpg";

            context.Pictures.AddRange(percyJacksonPicture, theLionPicture, goneForGoodPicture);
            context.SaveChanges();
            #endregion

            #region Author
            Author Rick = new Author();
            Rick.Name = "ריק ריירדן";
            Rick.BirthDate = new DateTime(1964, 6, 5, 0, 0, 0);
            Rick.Description = "ריק ריירדן הוא סופר נוער אמריקני עטור פרסים. כנער הִרבּה לקרוא סיפורי מיתולוגיה וספרי פנטזיה ומדע בדיוני.";

            Author Thircha = new Author();
            Thircha.Name = "תרצה אלתרמן";
            Thircha.BirthDate = new DateTime(1941, 1, 27, 0, 0, 0);
            Thircha.DeathDate = new DateTime(1977, 9, 8, 0, 0, 0);
            Thircha.Description = "אתר כתבה שישה ספרי ילדים  שני מחזות וכן תרגמה יותר משלושים מחזות, ואלה הועלו על הבמות בישראל.";

            Author Harlan = new Author();
            Harlan.Name = "הרלן קובן";
            Harlan.BirthDate = new DateTime(1962, 1, 4, 0, 0, 0);
            Harlan.Description = "הרלן סופר מתח יהודי, ספרו הראשון הופיע בשנת 1990. עד 2008 פרסם 17 ספרי מתח שהפכו לרבי-מכר, שניים מהם צעדו ברשימת רבי-המכר היוקרתית של הניו יורק טיימס. ספריו, המתרחשים בדרך כלל בפרוורים הבורגניים של אזור ניו ג'רזי, מצטיינים בעלילה מהירה ורבת תפניות ובסוף מפתיע.";

            context.Authors.AddRange(Rick, Thircha, Harlan);
            context.SaveChanges();
            #endregion

            #region Age Catagory
            AgeCategory teens = new AgeCategory();
            teens.Name = "נוער";

            AgeCategory kids = new AgeCategory();
            kids.Name = "ילדים";

            AgeCategory adults = new AgeCategory();
            adults.Name = "מבוגרים";
            context.AgeCategories.AddRange(teens, kids, adults);
            context.SaveChanges();
            #endregion

            #region Books
            Book percyJackson = new Book();
            percyJackson.Author = Rick;
            percyJackson.AgeCategory = teens;
            percyJackson.Name = "פרסי גקסון וגנב הברק";
            percyJackson.Picture = percyJacksonPicture;
            percyJackson.Description = "הספר הוא הרומן הראשון בסדרת פרסי ג'קסון והאולימפיים המבוססת על המיתולוגיה היוונית, שמספרת את עלילותיו הבדיוניות של האל למחצה – פרסי ג'קסון.";
            percyJackson.PublishDate = new DateTime(2008, 4, 6, 0, 0, 0);
            percyJackson.Price = 113;
            percyJackson.NumberOfPages = 375;
            percyJackson.QuantityInStock = 4;
            percyJackson.Language = hebrewLang;
            percyJackson.Genre = novel;

            Book theLion = new Book();
            theLion.Author = Thircha;
            theLion.AgeCategory = kids;
            theLion.Name = "האריה שאהב תות";
            theLion.Picture = theLionPicture;
            theLion.Description = "הסיפור מתמקד באריה המבקש לאכול תותים, ותותים בלבד, אך ביער שבו הוא גר אין תותים. בעוד שאמו הלביאה מנסה לשכנע אותו לאכול מאכלים אחרים, האריה מתעקש לאכול רק תות. כאשר ילדים מטיילים ביער ובאמתחתם תות, מבקש האריה לטעום מהמאכל, אולם הילדים נבהלים ובורחים מהמקום. האריה טועם מהתות שמצא בתיקיהם שנותרו מאחור ואוכל את כל התותים שהם השאירו, כתוצאה מכך הוא החליט שהוא לא אוהב תותים. החוויה משנה את גישתו של האריה ומאותו יום ואילך הוא אוכל בתיאבון את האוכל שנותנת לו אמו.";
            theLion.PublishDate = new DateTime(1971, 4, 1, 0, 0, 0);
            theLion.Price = 42;
            theLion.NumberOfPages = 15;
            theLion.QuantityInStock = 3;
            theLion.Language = hebrewLang;
            theLion.Genre = adventures;

            Book goneForGood = new Book();
            goneForGood.Author = Harlan;
            goneForGood.AgeCategory = adults;
            goneForGood.Name = "הנעלמים";
            goneForGood.Picture = goneForGoodPicture;
            goneForGood.Description = "הספר עוקב אחר קורות אדם שאימו הלכה לעולמה. נדמה כי חייו עלו על שרטון. אחיו, המואשם באונס ורצח חברתו ג'ולי, בורח מהחוק כבר יותר מ-10 שנים, חברתו שילה נעלמת ללא הסבר ומשאירה המון שאלות ללא תשובות ואם לא די בכך לפתע נכנס לחייו רוצח סדרתי אשר עוקב אחר כל אחד מצעדיו. אט אט נחשפים בעלילה רמזים שיובילו לגילוי האמת בנוגע לכל ההתרחשויות החריגות.";
            goneForGood.PublishDate = new DateTime(2004, 3, 3, 0, 0, 0);
            goneForGood.Price = 105;
            goneForGood.NumberOfPages = 420;
            goneForGood.QuantityInStock = 7;
            goneForGood.Language = hebrewLang;
            goneForGood.Genre = thriller;

            context.Books.AddRange(percyJackson,theLion,goneForGood);
            context.SaveChanges();
            #endregion
        }

    }
}
