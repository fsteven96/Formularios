using Microsoft.EntityFrameworkCore;
using DynamicFormsAPI.Models;

namespace DynamicFormsAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Formulario> Formularios { get; set; }
        public DbSet<Campo> Campos { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Campo>()
                .HasOne(c => c.Formulario)
                .WithMany(f => f.Campos)
                .HasForeignKey(c => c.FormularioId);
        }
    }
}
