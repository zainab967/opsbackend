using Microsoft.AspNetCore.Mvc;
using CMS.API.Services;
using CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly DataSeedingService _seedingService;
        private readonly AppDbContext _context;

        public SeedController(DataSeedingService seedingService, AppDbContext context)
        {
            _seedingService = seedingService;
            _context = context;
        }

        /// <summary>
        /// Seed the database with sample data for testing
        /// </summary>
        [HttpPost("sample-data")]
        public async Task<IActionResult> SeedSampleData()
        {
            try
            {
                await _seedingService.SeedDataAsync();
                return Ok(new
                {
                    success = true,
                    message = "Sample data has been seeded successfully",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = new
                    {
                        code = "SEEDING_ERROR",
                        message = "Failed to seed sample data",
                        details = ex.Message
                    },
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Clear all data from the database (USE WITH CAUTION!)
        /// </summary>
        [HttpDelete("clear-data")]
        public async Task<IActionResult> ClearAllData()
        {
            try
            {
                // Clear in reverse order of dependencies
                _context.AssetRequests.RemoveRange(_context.AssetRequests);
                _context.AssetMaintenances.RemoveRange(_context.AssetMaintenances);
                _context.AssetLogs.RemoveRange(_context.AssetLogs);
                _context.ExpenseDocuments.RemoveRange(_context.ExpenseDocuments);
                _context.Expenses.RemoveRange(_context.Expenses);
                _context.Reimbursements.RemoveRange(_context.Reimbursements);
                _context.ComplaintSuggestions.RemoveRange(_context.ComplaintSuggestions);
                _context.LedgerEntries.RemoveRange(_context.LedgerEntries);
                _context.Assets.RemoveRange(_context.Assets);
                _context.UserPermissions.RemoveRange(_context.UserPermissions);
                _context.Users.RemoveRange(_context.Users);
                _context.Departments.RemoveRange(_context.Departments);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "All data has been cleared from the database",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = new
                    {
                        code = "CLEARING_ERROR",
                        message = "Failed to clear data",
                        details = ex.Message
                    },
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Get current database statistics
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetDatabaseStats()
        {
            try
            {
                var stats = new
                {
                    Departments = await _context.Departments.CountAsync(),
                    Users = await _context.Users.CountAsync(),
                    Assets = await _context.Assets.CountAsync(),
                    AssetRequests = await _context.AssetRequests.CountAsync(),
                    Expenses = await _context.Expenses.CountAsync(),
                    Reimbursements = await _context.Reimbursements.CountAsync(),
                    ComplaintSuggestions = await _context.ComplaintSuggestions.CountAsync(),
                    LedgerEntries = await _context.LedgerEntries.CountAsync()
                };

                return Ok(new
                {
                    success = true,
                    data = stats,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = new
                    {
                        code = "STATS_ERROR",
                        message = "Failed to get database statistics",
                        details = ex.Message
                    },
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
