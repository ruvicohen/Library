using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.Models;
using System.Reflection.Emit;

namespace Library.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext (DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Genre>()
                .HasIndex(u => u.Name)
                .IsUnique();

            builder.Entity<Set>()
            .HasMany(s => s.Books)
            .WithOne(b => b.Set)
            .HasForeignKey(b => b.SetId)
            .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);

        }


        public DbSet<Library.Models.Book> Book { get; set; } = default!;
        public DbSet<Library.Models.Genre> Genre { get; set; } = default!;
        public DbSet<Library.Models.Set> Set { get; set; } = default!;
        public DbSet<Library.Models.Shelf> Shelf { get; set; } = default!;
    }
}
