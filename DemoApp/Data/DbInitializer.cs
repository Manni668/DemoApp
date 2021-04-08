using System;
using System.Linq;
using DemoApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApp.Data
{
    public static class DbInitializer
    {
        public static void Initializer(IServiceProvider serviceProvider)
        {
            using (var context = new DemoDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<DemoDbContext>>()))
            {
                if (context.Accounts.Any())
                {
                    return;
                }

                context.Accounts.AddRange
                (
                new Account{UserId=1234, UserName="Jay"},
                new Account{UserId=12341, UserName = "Lee"}
                );
                context.SaveChanges();

            }
        }
    }

}
