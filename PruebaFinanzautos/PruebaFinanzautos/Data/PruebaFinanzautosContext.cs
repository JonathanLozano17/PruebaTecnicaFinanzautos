using Microsoft.EntityFrameworkCore;
using PruebaFinanzautos.Models;

namespace PruebaFinanzautos.Data
{
    public class PruebaFinanzautosContext : DbContext
    {
        public PruebaFinanzautosContext(DbContextOptions<PruebaFinanzautosContext> options) : base(options) { }

   

        public DbSet<Student> Students {  get; set; }
        public DbSet<Teacher> teachers {  get; set; }
        public DbSet<Course> Courses {  get; set; }
        public DbSet<Grade> Grades {  get; set; }
        public DbSet<Usuario> Usuarios { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .Property(s => s.Address)
                .HasMaxLength(64);


            modelBuilder.Entity<Teacher>()
                .Property(t => t.Address)
                .HasMaxLength(64);


            modelBuilder.Entity<Course>()
                .Property(c => c.CorseName)
                .HasMaxLength(100);


            modelBuilder.Entity<Grade>()
                .Property(g => g.GradeId)
                .HasMaxLength(100);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Correo)
                .IsUnique();

        }

    }

}
