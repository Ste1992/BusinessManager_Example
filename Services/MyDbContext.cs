using BusinessManager.Models;
using BusinessManager.Models.DipendenteModels;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Services
{
    public class MyDbContext : DbContext
    {
        public DbSet<Dipendente> Dipendenti { get; set; }
        public DbSet<Timbratura> Timbrature { get; set; }
        public DbSet<BustaPaga> BustaPaga { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=/directory/to/database.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Timbratura>()
                .HasKey(t => t.Id); // Imposta Id come chiave primaria di Timbratura   

            modelBuilder.Entity<Timbratura>()
                .HasOne(t => t.Dipendente)
                .WithMany(d => d.Timbrature) // Assicurati che Timbrature sia la proprietà di navigazione nella classe Dipendente
                .HasForeignKey(t => t.DipendenteId); // Imposta DipendenteId come chiave esterna in Timbratura

            modelBuilder.Entity<BustaPaga>()
                .HasKey(bp => bp.DipendenteId); // Imposta DipendenteId come chiave primaria di BustaPaga

            modelBuilder.Entity<Dipendente>()
                .HasOne(d => d.BustaPaga)
                .WithOne(bp => bp.Dipendente)
                .HasForeignKey<BustaPaga>(bp => bp.DipendenteId); // Assicurati che DipendenteId sia il nome della chiave esterna in BustaPaga        
        }

    }
}
