using BookStore.Models;
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
            if (context.Districts.Any())
            {
                return;
            }

        }

    }
}
