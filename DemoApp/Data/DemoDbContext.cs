using System;
using DemoApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Data
{
    public class DemoDbContext: DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options)
             : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Balance> Balances { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Login>().ToTable("Logins");
            modelBuilder.Entity<Balance>().ToTable("Balances");

        }
    }
}
