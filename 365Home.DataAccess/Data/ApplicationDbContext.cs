using System;
using System.Collections.Generic;
using System.Text;
using _365Home.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _365Home.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Frequency> Frequency { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<WebImages> WebImages { get; set; }

        public DbSet<Location> Location { get; set; }

        public DbSet<Province> Province { get; set; }

        public DbSet<District> District { get; set; }

        public DbSet<UserAddress> UserAddress { get; set; }

        public DbSet<Ward> Ward { get; set; }

        public DbSet<LocationType> LocationType { get; set; }

    }
}
