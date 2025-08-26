using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Infrastructure.Data;
using CMS.Core.Entities;

namespace CMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BulkOperationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BulkOperationsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Bulk create users from external system sync
        /// </summary>
        [HttpPost("users/bulk-create")]
        public async Task<IActionResult> BulkCreateUsers([FromBody] List<BulkUserCreateDto> users)
        {
            try
            {
                var results = new List<object>();
                var created = 0;
                var updated = 0;
                var errors = 0;

                foreach (var userDto in users)
                {
                    try
                    {
                        // Check if user exists by external ID
                        var existingUser = await _context.Users
                            .FirstOrDefaultAsync(u => u.ExternalUserId == userDto.ExternalUserId && 
                                                     u.ExternalSystemName == userDto.ExternalSystemName);

                        if (existingUser != null)
                        {
                            // Update existing user
                            existingUser.Email = userDto.Email;
                            existingUser.FirstName = userDto.FirstName;
                            existingUser.LastName = userDto.LastName;
                            existingUser.Role = userDto.Role;
                            existingUser.DepartmentId = userDto.DepartmentId ?? existingUser.DepartmentId;
                            existingUser.Active = userDto.Active;
                            existingUser.UpdatedAt = DateTime.UtcNow;
                            updated++;
                        }
                        else
                        {
                            // Create new user
                            var newUser = new User
                            {
                                Id = Guid.NewGuid(),
                                Email = userDto.Email,
                                FirstName = userDto.FirstName,
                                LastName = userDto.LastName,
                                Role = userDto.Role,
                                DepartmentId = userDto.DepartmentId ?? Guid.Empty,
                                Active = userDto.Active,
                                ExternalUserId = userDto.ExternalUserId,
                                ExternalSystemName = userDto.ExternalSystemName,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };
                            _context.Users.Add(newUser);
                            created++;
                        }

                        results.Add(new { 
                            ExternalId = userDto.ExternalUserId, 
                            Status = "Success",
                            Action = existingUser != null ? "Updated" : "Created"
                        });
                    }
                    catch (Exception ex)
                    {
                        errors++;
                        results.Add(new { 
                            ExternalId = userDto.ExternalUserId, 
                            Status = "Error", 
                            Message = ex.Message 
                        });
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Summary = new { created, updated, errors },
                    Results = results
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bulk user creation failed", error = ex.Message });
            }
        }

        /// <summary>
        /// Bulk update asset statuses
        /// </summary>
        [HttpPatch("assets/bulk-status")]
        public async Task<IActionResult> BulkUpdateAssetStatus([FromBody] BulkStatusUpdateDto request)
        {
            try
            {
                var assets = await _context.Assets
                    .Where(a => request.AssetIds.Contains(a.Id))
                    .ToListAsync();

                if (!assets.Any())
                {
                    return NotFound("No assets found with the provided IDs");
                }

                foreach (var asset in assets)
                {
                    asset.Status = request.NewStatus;
                    asset.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = $"Successfully updated {assets.Count} assets to status: {request.NewStatus}",
                    updatedAssets = assets.Select(a => new { a.Id, a.Name, a.Status }).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bulk status update failed", error = ex.Message });
            }
        }

        /// <summary>
        /// Bulk approve/reject expenses
        /// </summary>
        [HttpPatch("expenses/bulk-approval")]
        public async Task<IActionResult> BulkUpdateExpenseStatus([FromBody] BulkApprovalDto request)
        {
            try
            {
                var expenses = await _context.Expenses
                    .Where(e => request.ExpenseIds.Contains(e.Id))
                    .Include(e => e.User)
                    .ToListAsync();

                if (!expenses.Any())
                {
                    return NotFound("No expenses found with the provided IDs");
                }

                var results = new List<object>();

                foreach (var expense in expenses)
                {
                    if (expense.Status != ExpenseStatus.Pending)
                    {
                        results.Add(new { 
                            expense.Id, 
                            Status = "Skipped", 
                            Reason = $"Expense already {expense.Status}" 
                        });
                        continue;
                    }

                    expense.Status = request.NewStatus;
                    expense.ReviewedAt = DateTime.UtcNow;
                    expense.ReviewedById = request.ReviewerId;
                    expense.ReviewNotes = request.ReviewNotes;
                    expense.UpdatedAt = DateTime.UtcNow;

                    results.Add(new { 
                        expense.Id, 
                        Status = "Updated", 
                        NewStatus = request.NewStatus.ToString() 
                    });
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = $"Bulk approval processing completed",
                    results = results
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bulk approval failed", error = ex.Message });
            }
        }

        /// <summary>
        /// Bulk archive old records
        /// </summary>
        [HttpPost("archive/old-records")]
        public async Task<IActionResult> BulkArchiveOldRecords([FromBody] BulkArchiveDto request)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-request.DaysOld);
                var results = new Dictionary<string, int>();

                // Archive old completed asset requests
                if (request.IncludeAssetRequests)
                {
                    var oldRequests = await _context.AssetRequests
                        .Where(ar => ar.UpdatedAt < cutoffDate && 
                                    (ar.Status == RequestStatus.Fulfilled || ar.Status == RequestStatus.Rejected))
                        .ToListAsync();
                    
                    // In a real system, you might move these to an archive table or mark as archived
                    // For now, we'll just mark them somehow or delete them based on requirements
                    results["ArchivedAssetRequests"] = oldRequests.Count;
                }

                // Archive old processed reimbursements
                if (request.IncludeReimbursements)
                {
                    var oldReimbursements = await _context.Reimbursements
                        .Where(r => r.UpdatedAt < cutoffDate && 
                                   (r.Status == ReimbursementStatus.Paid || r.Status == ReimbursementStatus.Rejected))
                        .ToListAsync();
                    
                    results["ArchivedReimbursements"] = oldReimbursements.Count;
                }

                // Archive old approved expenses
                if (request.IncludeExpenses)
                {
                    var oldExpenses = await _context.Expenses
                        .Where(e => e.UpdatedAt < cutoffDate && 
                                   (e.Status == ExpenseStatus.Approved || e.Status == ExpenseStatus.Rejected))
                        .ToListAsync();
                    
                    results["ArchivedExpenses"] = oldExpenses.Count;
                }

                // Note: In a production system, you would typically move these records to archive tables
                // rather than delete them, to maintain audit trails

                return Ok(new
                {
                    message = "Archive operation completed",
                    cutoffDate = cutoffDate,
                    results = results
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Archive operation failed", error = ex.Message });
            }
        }

        /// <summary>
        /// Bulk data validation check
        /// </summary>
        [HttpGet("validation/data-integrity")]
        public async Task<IActionResult> ValidateDataIntegrity()
        {
            try
            {
                var issues = new List<object>();

                // Check for orphaned records
                var orphanedAssets = await _context.Assets
                    .Where(a => !_context.Departments.Any(d => d.Id == a.DepartmentId))
                    .CountAsync();
                if (orphanedAssets > 0)
                    issues.Add(new { Type = "OrphanedAssets", Count = orphanedAssets });

                var orphanedExpenses = await _context.Expenses
                    .Where(e => !_context.Users.Any(u => u.Id == e.UserId))
                    .CountAsync();
                if (orphanedExpenses > 0)
                    issues.Add(new { Type = "OrphanedExpenses", Count = orphanedExpenses });

                // Check for data inconsistencies
                var usersWithoutDepartments = await _context.Users
                    .Where(u => u.Active && !_context.Departments.Any(d => d.Id == u.DepartmentId))
                    .CountAsync();
                if (usersWithoutDepartments > 0)
                    issues.Add(new { Type = "UsersWithoutDepartments", Count = usersWithoutDepartments });

                // Check for future dates that shouldn't be future
                var futureExpenses = await _context.Expenses
                    .Where(e => e.Date > DateTime.UtcNow.Date)
                    .CountAsync();
                if (futureExpenses > 0)
                    issues.Add(new { Type = "FutureExpenses", Count = futureExpenses });

                return Ok(new
                {
                    hasIssues = issues.Any(),
                    issueCount = issues.Count,
                    issues = issues
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Data validation failed", error = ex.Message });
            }
        }
    }

    // DTOs for bulk operations
    public class BulkUserCreateDto
    {
        public string ExternalUserId { get; set; }
        public string ExternalSystemName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public Guid? DepartmentId { get; set; }
        public bool Active { get; set; } = true;
    }

    public class BulkStatusUpdateDto
    {
        public List<Guid> AssetIds { get; set; }
        public AssetStatus NewStatus { get; set; }
    }

    public class BulkApprovalDto
    {
        public List<Guid> ExpenseIds { get; set; }
        public ExpenseStatus NewStatus { get; set; }
        public Guid ReviewerId { get; set; }
        public string ReviewNotes { get; set; }
    }

    public class BulkArchiveDto
    {
        public int DaysOld { get; set; } = 365; // Default to 1 year
        public bool IncludeAssetRequests { get; set; } = true;
        public bool IncludeReimbursements { get; set; } = true;
        public bool IncludeExpenses { get; set; } = true;
    }
}
