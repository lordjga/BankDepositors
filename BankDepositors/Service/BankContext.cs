using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using BankDepositors;
using Microsoft.EntityFrameworkCore;

namespace BankDepositors
{
   public class BankContext : System.Data.Entity.DbContext
    {
        public BankContext() : base("DefaultConnection")
        {

        }
        public System.Data.Entity.DbSet<Client> Clients { get; set; }
        public System.Data.Entity.DbSet<Deposit> Deposits { get; set; }
        public System.Data.Entity.DbSet<Staff> Staffs { get; set; }
        public System.Data.Entity.DbSet<Currency> Currencies { get; set; }
        public System.Data.Entity.DbSet<DepositClient> DepositClients { get; set; }
        
        protected void OnModelCreating(ModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Entity<DepositClient>()
                .HasKey(i => new {i.Client_Id, i.Deposit_Id});
            dbModelBuilder.Entity<DepositClient>()
                .HasOne(bc => bc.Client)
                .WithMany(b => b.DepositClients)
                .HasForeignKey(bc => bc.Client_Id);
            dbModelBuilder.Entity<DepositClient>()
                .HasOne(bc => bc.Client)
                .WithMany(b => b.DepositClients)
                .HasForeignKey(bc => bc.Client_Id);
        
            /*
                .HasOne(bc => bc.Client)
                .WithMany(b => b.DepositClients)
                .HasForeignKey(bc => bc.Client_Id);
            dbModelBuilder.Entity<DepositClient>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(bc => bc.CategoryId);
            /*
            dbModelBuilder.Entity<Deposit>()
                .HasMany(c => c.Clients)
                .WithMany((s => s.Deposits));
            */
        }

        /*
        modelBuilder
            .Entity<Course>()
            .HasMany(c => c.Students)
            .WithMany(s => s.Courses)
            .UsingEntity > Enrollment < (
            j => j
                .HasOne(pt => pt.Student)
                .WithMany(t => t.Enrollments)
                .HasForeignKey(pt => pt.StudentId),
            j => j
                .HasOne(pt => pt.Course)
                .WithMany(p => p.Enrollments)
                .HasForeignKey(pt => pt.CourseId),
            j =>
            {
                j.Property(pt => pt.EnrollmentDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                j.Property(pt => pt.Mark).HasDefaultValue(3);
                j.HasKey(t => new { t.CourseId, t.StudentId });
                j.ToTable("Enrollments");
            }
        );*/
    }

}



