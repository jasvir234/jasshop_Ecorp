using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Jasshop.Models;

namespace Jasshop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Jasshop.Models.Items> Items { get; set; }
        public DbSet<Jasshop.Models.Sales> Sales { get; set; }
        public DbSet<Jasshop.Models.ShoppingCart> ShoppingCart { get; set; }
    }
}
