using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using CMS.Infrastructure.Data;
using CMS.Core.Entities;

namespace CMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get comprehensive dashboard statistics
        /// </summary>
        [HttpGet("overview")]
        public async Task<IActionResult> GetDashboardOverview()
        {
            try
            {
                var overview = new
                {
                    Users = new
                    {
                        Total = await _context.Users.CountAsync(),
                        Active = await _context.Users.CountAsync(u => u.Active),
                        ByRole = await _context.Users
                            .GroupBy(u => u.Role)
                            .Select(g => new { Role = g.Key.ToString(), Count = g.Count() })
                            .ToListAsync()
                    },
                    Assets = new
                    {
                        Total = await _context.Assets.CountAsync(),
                        ByStatus = await _context.Assets
                            .GroupBy(a => a.Status)
                            .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                            .ToListAsync(),
                        TotalValue = await _context.Assets.SumAsync(a => a.Value)
                    },
                    Expenses = new
                    {
                        ThisMonth = await _context.Expenses
                            .Where(e => e.CreatedAt.Month == DateTime.UtcNow.Month && 
                                       e.CreatedAt.Year == DateTime.UtcNow.Year)
                            .SumAsync(e => e.Amount),
                        PendingApproval = await _context.Expenses
                            .CountAsync(e => e.Status == ExpenseStatus.Pending)
                    },
                    Departments = new
                    {
                        Total = await _context.Departments.CountAsync()
                    }
                };

                return Ok(overview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving dashboard data", error = ex.Message });
            }
        }
    }
}
