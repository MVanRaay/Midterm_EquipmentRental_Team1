using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Data;

public class AppDbContext : DbContext
{
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipment>().HasData(
            new Equipment
            {
                Id = 1,
                Name = "Laser Level",
                Description = "For when you absolutely need something to be level.",
                RentalPrice = 55.95m,
                IsAvailable = false,
                Category = EquipmentCategory.Surveying,
                Condition = EquipmentCondition.Good,
                CreatedAt = DateTime.Now
            },
            new Equipment
            {
                Id = 2,
                Name = "Mini Excavator",
                Description = "1-Ton Kubota K008-3 Mini Excavator is small and maneuverable enough to fit anywhere, and capable enough to help you accomplish your backyard landscaping dreams.",
                RentalPrice = 299.99m,
                IsAvailable = true,
                Category = EquipmentCategory.HeavyMachinery,
                Condition = EquipmentCondition.Fair,
                CreatedAt = DateTime.Now
            },
            new Equipment
            {
                Id = 3,
                Name = "Wood Chipper / Trailer Combo",
                Description = "An effective and mobile solution for removing trees, without the mess. Comes With the wood chipper fixed to a collection trailer, which attaches to a standard trailer hitch.",
                RentalPrice = 139.99m,
                IsAvailable = false,
                Category = EquipmentCategory.Vehicles,
                Condition = EquipmentCondition.Excellent,
                CreatedAt = DateTime.Now
            },
            new Equipment
            {
                Id = 4,
                Name = "Power Drill & Impact Driver with Socket Set",
                Description = "With this Milwaulkee M18 1/4\" Power Drill / Impact Driver & Socket Set, you can finally finish that pesky item on your to-do list.",
                RentalPrice = 14.95m,
                IsAvailable = false,
                Category = EquipmentCategory.PowerTools,
                Condition = EquipmentCondition.New,
                CreatedAt = DateTime.Now
            },
            new Equipment
            {
                Id = 5,
                Name = "Hard Hat",
                Description = "Keep your 'noggin safe.",
                RentalPrice = 5.95m,
                IsAvailable = true,
                Category = EquipmentCategory.Safety,
                Condition = EquipmentCondition.Poor,
                CreatedAt = DateTime.Now
            }
        );

        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = 1,
                Name = "Eleonora Lavoie",
                Email = "elavoie@email.com",
                Username = "admin",
                Password = "admin",
                Role = "Admin",
                HasActiveRental = false
            },
            new Customer
            {
                Id = 2,
                Name = "Gotthard Hayes",
                Email = "ghayes@email.com",
                Username = "GotthardHayes",
                Password = "Gotthard",
                Role = "User",
                HasActiveRental = true
            },
            new Customer
            {
                Id = 3,
                Name = "Francisca Manco",
                Email = "fmanco@email.com",
                Username = "FranciscaManco",
                Password = "Francisca",
                Role = "User",
                HasActiveRental = false
            },
            new Customer
            {
                Id = 4,
                Name = "Chase Leonardi",
                Email = "cleonardi@email.com",
                Username = "ChaseLeonardi",
                Password = "Chase",
                Role = "User",
                HasActiveRental = false
            },
            new Customer
            {
                Id = 5,
                Name = "Nura Edson",
                Email = "nedson@email.com",
                Username = "NuraEdson",
                Password = "Nura",
                Role = "User",
                HasActiveRental = true
            },
            new Customer
            {
                Id = 6,
                Name = "Viviane Xun",
                Email = "vxun@email.com",
                Username = "VivianeXun",
                Password = "Viviane",
                Role = "User",
                HasActiveRental = true
            }
        );

        modelBuilder.Entity<Rental>().HasData(
            new Rental
            {
                Id = 1,
                EquipmentId = 1,
                CustomerId = 2,
                Status = true,
                IssuedAt = DateTime.Now,
                DueDate = DateTime.Now.Date.AddDays(3).AddHours(23).AddMinutes(59).AddSeconds(59)
            },
            new Rental
            {
                Id = 2,
                EquipmentId = 4,
                CustomerId = 3,
                Status = false,
                IssuedAt = DateTime.Parse("2025/8/25 15:26:13"),
                DueDate = DateTime.Parse("2025/8/28 23:59:59"),
                ReturnedAt = DateTime.Parse("2025/8/30 17:48:09")
            },
            new Rental
            {
                Id = 3,
                EquipmentId = 4,
                CustomerId = 5,
                Status = true,
                IssuedAt = DateTime.Now,
                DueDate = DateTime.Now.Date.AddDays(5).AddHours(23).AddMinutes(59).AddSeconds(59)
            },
            new Rental
            {
                Id = 4,
                EquipmentId = 2,
                CustomerId = 4,
                Status = false,
                IssuedAt = DateTime.Parse("2025/6/13 11:04:39"),
                DueDate = DateTime.Parse("2025/6/16 23:59:59"),
                ReturnedAt = DateTime.Parse("2025/6/16 18:14:52")
            },
            new Rental
            {
                Id = 5,
                EquipmentId = 1,
                CustomerId = 4,
                Status = false,
                IssuedAt = DateTime.Parse("2025/6/16 18:18:51"),
                DueDate = DateTime.Parse("2025/6/19 23:59:59"),
                ReturnedAt = DateTime.Parse("2025/6/18 14:19:12")
            },
            new Rental
            {
                Id = 6,
                EquipmentId = 3,
                CustomerId = 6,
                Status = true,
                IssuedAt = DateTime.Parse("2025/10/14 10:35:18"),
                DueDate = DateTime.Parse("2025/10/17 23:59:59"),
            }
        );
    }
}
