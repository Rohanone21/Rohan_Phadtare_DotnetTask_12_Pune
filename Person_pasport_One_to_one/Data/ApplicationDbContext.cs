using Microsoft.EntityFrameworkCore;
using Person_pasport_One_to_one.Models;

namespace Person_pasport_One_to_one.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { } 
         public DbSet<Person> people { get; set; }
        public DbSet<Passport> passport { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-one relationship with optional dependent
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Passport)
                .WithOne(p => p.Person)
                .HasForeignKey<Passport>(p => p.PersonId)
                .IsRequired(false); // Make foreign key optional

            // Configure Person
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(p => p.PersonId);
                entity.Property(p => p.PersonName)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            // Configure Passport
            modelBuilder.Entity<Passport>(entity =>
            {
                entity.HasKey(p => p.PersonId);
                entity.Property(p => p.PassportNumber)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(p => p.IssueDate)
                      .IsRequired();
                entity.Property(p => p.ExpiryDate)
                      .IsRequired();
            });
        }
    }
}
