using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Core.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetMaintenanceController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AssetMaintenanceController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetMaintenance>>> GetAll() => await _context.AssetMaintenances.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<AssetMaintenance>> GetById(Guid id)
        {
            var maint = await _context.AssetMaintenances.FindAsync(id);
            if (maint == null) return NotFound();
            return maint;
        }

        [HttpPost]
        public async Task<ActionResult<AssetMaintenance>> Create(AssetMaintenance maintenance)
        {
            maintenance.Id = Guid.NewGuid();
            maintenance.CreatedAt = DateTime.UtcNow;
            maintenance.UpdatedAt = DateTime.UtcNow;
            _context.AssetMaintenances.Add(maintenance);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = maintenance.Id }, maintenance);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, AssetMaintenance maintenance)
        {
            if (id != maintenance.Id) return BadRequest();
            maintenance.UpdatedAt = DateTime.UtcNow;
            _context.Entry(maintenance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var maint = await _context.AssetMaintenances.FindAsync(id);
            if (maint == null) return NotFound();
            _context.AssetMaintenances.Remove(maint);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
