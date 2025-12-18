using Microsoft.EntityFrameworkCore;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class DbAppContext : DbContext
    {
        public DbSet<Phone> Phones { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Phone;Username=postgres;Password=C0d38_50AdM1Nn6");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Phone>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.ToTable("phone");

                entity.Property(e => e.Title)
                    .HasColumnName("title");

                entity.Property(e => e.Company)
                    .HasColumnName("company");

                entity.Property(e => e.Price)
                    .HasColumnName("price");
            });
        }
    }
}
