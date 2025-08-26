using CMS.Core.Entities;
using CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.API.Services
{
    public class DataSeedingService
    {
        private readonly AppDbContext _context;

        public DataSeedingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedDataAsync()
        {
            // Check if data already exists
            if (await _context.Departments.AnyAsync())
            {
                Console.WriteLine("Database already contains data. Skipping seeding.");
                return;
            }

            Console.WriteLine("Seeding database with sample data...");

            // Seed Departments
            var departments = new List<Department>
            {
                new Department
                {
                    Id = Guid.NewGuid(),
                    Name = "Information Technology",
                    Description = "Technology and software development",
                    Budget = 500000,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Department
                {
                    Id = Guid.NewGuid(),
                    Name = "Human Resources",
                    Description = "Employee management and operations",
                    Budget = 200000,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Department
                {
                    Id = Guid.NewGuid(),
                    Name = "Finance",
                    Description = "Financial planning and accounting",
                    Budget = 300000,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Department
                {
                    Id = Guid.NewGuid(),
                    Name = "Marketing",
                    Description = "Marketing and business development",
                    Budget = 250000,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await _context.Departments.AddRangeAsync(departments);
            await _context.SaveChangesAsync();

            // Seed Users
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "john.doe@company.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Role = UserRole.Admin,
                    DepartmentId = departments[0].Id, // IT
                    Active = true,
                    ExternalUserId = "ext_001",
                    ExternalSystemName = "ExternalAuth",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "jane.smith@company.com",
                    FirstName = "Jane",
                    LastName = "Smith",
                    Role = UserRole.Manager,
                    DepartmentId = departments[1].Id, // HR
                    Active = true,
                    ExternalUserId = "ext_002",
                    ExternalSystemName = "ExternalAuth",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "mike.johnson@company.com",
                    FirstName = "Mike",
                    LastName = "Johnson",
                    Role = UserRole.Employee,
                    DepartmentId = departments[2].Id, // Finance
                    Active = true,
                    ExternalUserId = "ext_003",
                    ExternalSystemName = "ExternalAuth",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "sarah.wilson@company.com",
                    FirstName = "Sarah",
                    LastName = "Wilson",
                    Role = UserRole.Employee,
                    DepartmentId = departments[3].Id, // Marketing
                    Active = true,
                    ExternalUserId = "ext_004",
                    ExternalSystemName = "ExternalAuth",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            // Seed Assets
            var assets = new List<Asset>
            {
                new Asset
                {
                    Id = Guid.NewGuid(),
                    Name = "Dell Laptop XPS 15",
                    Category = "Computer",
                    SerialNumber = "DL001234",
                    DepartmentId = departments[0].Id,
                    AssignedTo = users[0].Id,
                    Status = AssetStatus.Assigned,
                    Value = 1500,
                    PurchaseDate = DateTime.UtcNow.AddMonths(-6),
                    Condition = AssetCondition.Excellent,
                    Location = "IT Office - Desk 1",
                    WarrantyExpiry = DateTime.UtcNow.AddYears(2),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Asset
                {
                    Id = Guid.NewGuid(),
                    Name = "iPhone 14 Pro",
                    Category = "Mobile Device",
                    SerialNumber = "IP001234",
                    DepartmentId = departments[1].Id,
                    AssignedTo = users[1].Id,
                    Status = AssetStatus.Assigned,
                    Value = 1000,
                    PurchaseDate = DateTime.UtcNow.AddMonths(-3),
                    Condition = AssetCondition.Good,
                    Location = "HR Office",
                    WarrantyExpiry = DateTime.UtcNow.AddYears(1),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Asset
                {
                    Id = Guid.NewGuid(),
                    Name = "Conference Room Projector",
                    Category = "AV Equipment",
                    SerialNumber = "PROJ001",
                    DepartmentId = departments[3].Id,
                    Status = AssetStatus.Unassigned,
                    Value = 800,
                    PurchaseDate = DateTime.UtcNow.AddMonths(-12),
                    Condition = AssetCondition.Good,
                    Location = "Conference Room A",
                    WarrantyExpiry = DateTime.UtcNow.AddMonths(6),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await _context.Assets.AddRangeAsync(assets);
            await _context.SaveChangesAsync();

            // Seed Asset Requests
            var assetRequests = new List<AssetRequest>
            {
                new AssetRequest
                {
                    Id = Guid.NewGuid(),
                    AssetName = "Development Laptop",
                    Reason = "Need a new laptop for development work",
                    Specifications = "High-performance laptop with 16GB RAM and SSD",
                    UserId = users[2].Id,
                    DepartmentId = departments[2].Id,
                    Status = RequestStatus.Pending,
                    DurationType = DurationType.Permanent,
                    RequestedDate = DateTime.UtcNow.AddDays(-2),
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new AssetRequest
                {
                    Id = Guid.NewGuid(),
                    AssetName = "External Monitor",
                    Reason = "Additional monitor for productivity",
                    Specifications = "24-inch 1080p monitor",
                    UserId = users[3].Id,
                    DepartmentId = departments[3].Id,
                    Status = RequestStatus.Approved,
                    DurationType = DurationType.Permanent,
                    RequestedDate = DateTime.UtcNow.AddDays(-5),
                    ApprovedBy = users[1].Id,
                    ApprovedAt = DateTime.UtcNow.AddDays(-1),
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            await _context.AssetRequests.AddRangeAsync(assetRequests);
            await _context.SaveChangesAsync();

            // Seed Expenses
            var expenses = new List<Expense>
            {
                new Expense
                {
                    Id = Guid.NewGuid(),
                    Name = "Software License",
                    Amount = 299.99m,
                    UserId = users[0].Id,
                    DepartmentId = departments[0].Id,
                    Date = DateTime.UtcNow.AddDays(-10),
                    Status = ExpenseStatus.Approved,
                    Type = ExpenseType.OneTime,
                    Category = "Software",
                    Description = "Annual license for development tools",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Expense
                {
                    Id = Guid.NewGuid(),
                    Name = "Team Lunch",
                    Amount = 150.00m,
                    UserId = users[1].Id,
                    DepartmentId = departments[1].Id,
                    Date = DateTime.UtcNow.AddDays(-3),
                    Status = ExpenseStatus.Pending,
                    Type = ExpenseType.OneTime,
                    Category = "Meals",
                    Description = "Team building lunch for HR department",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                }
            };

            await _context.Expenses.AddRangeAsync(expenses);
            await _context.SaveChangesAsync();

            // Seed Reimbursements
            var reimbursements = new List<Reimbursement>
            {
                new Reimbursement
                {
                    Id = Guid.NewGuid(),
                    Name = "Travel Expenses",
                    Amount = 450.00m,
                    UserId = users[2].Id,
                    DepartmentId = departments[2].Id,
                    Date = DateTime.UtcNow.AddDays(-7),
                    Status = ReimbursementStatus.Approved,
                    Type = ReimbursementType.Travel,
                    Description = "Flight and hotel for client meeting",
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                }
            };

            await _context.Reimbursements.AddRangeAsync(reimbursements);
            await _context.SaveChangesAsync();

            // Seed Complaint/Suggestions
            var complaints = new List<ComplaintSuggestion>
            {
                new ComplaintSuggestion
                {
                    Id = Guid.NewGuid(),
                    Title = "Improve Office Wi-Fi",
                    Description = "The office Wi-Fi is very slow and affects productivity",
                    Category = "IT Infrastructure",
                    Type = FeedbackType.Suggestion,
                    Status = FeedbackStatus.Open,
                    Priority = PriorityLevel.Medium,
                    SubmittedBy = users[3].Id,
                    DepartmentId = departments[3].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                }
            };

            await _context.ComplaintSuggestions.AddRangeAsync(complaints);
            await _context.SaveChangesAsync();

            Console.WriteLine("Database seeded successfully!");
        }
    }
}
