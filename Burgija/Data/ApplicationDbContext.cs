using Burgija.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Burgija.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<Courier> Courier { get; set; }
        public DbSet<Delivery> Delivery { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<RegisteredUser> RegisteredUser { get; set; }
        public DbSet<Rent> Rent { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<Tool> Tool { get; set; }
        public DbSet<ToolType> ToolType { get; set; }
        public DbSet<ToolType> ToolTypes { get; set ; }

        public async Task<List<ToolType>> GetToolTypesAsync()
        {
            return await ToolTypes.ToListAsync();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Administrator>().ToTable(nameof(Administrator));
            builder.Entity<Courier>().ToTable(nameof(Courier));
            builder.Entity<Delivery>().ToTable(nameof(Delivery));
            builder.Entity<Discount>().ToTable(nameof(Discount));
            builder.Entity<Location>().ToTable(nameof(Location));
            builder.Entity<RegisteredUser>().ToTable(nameof(RegisteredUser));
            builder.Entity<Rent>().ToTable(nameof(Rent));
            builder.Entity<Review>().ToTable(nameof(Review));
            builder.Entity<Store>().ToTable(nameof(Store));
            builder.Entity<Tool>().ToTable(nameof(Tool));
            builder.Entity<ToolType>().ToTable(nameof(ToolType));
            base.OnModelCreating(builder);
        }
    }
}
