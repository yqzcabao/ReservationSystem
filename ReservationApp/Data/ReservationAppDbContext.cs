using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationApp.Data
{
    public class ReservationAppDbContext: IdentityDbContext<ApplicationUser>
    {

        public ReservationAppDbContext(DbContextOptions<ReservationAppDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sitting>(builder =>
            {
                // Date is a DateOnly property and date on database
               // builder.Property(x => x.Date)
               //     .HasConversion<DateOnlyConverter, DateOnlyComparer>();

                // Time is a TimeOnly property and time on database
                builder.Property(x => x.SittingStartTime)
                    .HasConversion<TimeOnlyConverter, TimeOnlyComparer>();
                builder.Property(x => x.SittingEndTime)
                   .HasConversion<TimeOnlyConverter, TimeOnlyComparer>();
            });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ReservationApp.Models.Product> Product { get; set; }= default;
        public DbSet<ReservationApp.Models.Sitting> Sitting { get; set; } = default!;
        public DbSet<ReservationApp.Models.SittingTable> SittingTable { get; set; } = default!;
        public DbSet<ReservationApp.Models.Reservation> Reservation { get; set; } = default!;
        public DbSet<ReservationApp.Models.ReservationSitting> ReservationSitting { get; set; } = default!;
        public DbSet<ReservationApp.Models.Order> Order { get; set; } = default;
        public DbSet<ReservationApp.Models.Stock> Stock { get; set; } = default!;

       
       

    }
}

