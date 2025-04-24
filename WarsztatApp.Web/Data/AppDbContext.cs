using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarsztatApp.Web.Models;

namespace WarsztatApp.Web.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Magazyn> Magazyny { get; set; }
        public DbSet<Warsztat> Warsztaty { get; set; }
        public DbSet<Przedmiot> Przedmioty { get; set; }
        public DbSet<Zlecenie> Zlecenia { get; set; }
        public DbSet<ZleceniePrzedmiot> ZleceniePrzedmioty { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // relacja Zlecenie <-> ZleceniePrzedmiot
            modelBuilder.Entity<ZleceniePrzedmiot>()
                .HasKey(zp => new { zp.ZlecenieId, zp.PrzedmiotId });

            modelBuilder.Entity<ZleceniePrzedmiot>()
                .HasOne(zp => zp.Zlecenie)
                .WithMany(z => z.ZleceniePrzedmioty)
                .HasForeignKey(zp => zp.ZlecenieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ZleceniePrzedmiot>()
                .HasOne(zp => zp.Przedmiot)
                .WithMany(p => p.ZleceniePrzedmioty)
                .HasForeignKey(zp => zp.PrzedmiotId)
                .OnDelete(DeleteBehavior.Restrict);

            // relacja Przedmiot -> Magazyn
            modelBuilder.Entity<Przedmiot>()
                .HasOne(p => p.Magazyn)
                .WithMany(m => m.Przedmioty)
                .HasForeignKey(p => p.MagazynId)
                .OnDelete(DeleteBehavior.Restrict);

            // relacja Zlecenie -> Warsztat
            modelBuilder.Entity<Zlecenie>()
                .HasOne(z => z.Warsztat)
                .WithMany(w => w.Zlecenia)
                .HasForeignKey(z => z.WarsztatId)
                .OnDelete(DeleteBehavior.Cascade);

            // relacja Warsztat -> Magazyn
            modelBuilder.Entity<Warsztat>()
                .HasOne(w => w.Magazyn)
                .WithMany()
                .HasForeignKey(w => w.MagazynId)
                .OnDelete(DeleteBehavior.Restrict);

            // relacja Warsztat -> IdentityUser
            modelBuilder.Entity<Warsztat>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
