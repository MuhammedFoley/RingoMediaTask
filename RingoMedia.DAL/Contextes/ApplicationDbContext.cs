using Microsoft.EntityFrameworkCore;
using RingoMedia.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingoMedia.DAL.Contextes
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.LogoPath)
                    .HasMaxLength(200);
                entity.HasOne(e => e.ParentDepartment)
                    .WithMany(d => d.SubDepartments)
                    .HasForeignKey(e => e.ParentDepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Reminder>(entity =>
            {
                entity.Property(e => e.ReminderDateTime)
                    .HasColumnType("datetime2")
                    .IsRequired();
            });
        }
    }

}
