using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Core.Entities;

namespace CMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AssetRequestsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetRequest>>> GetAll() => await _context.AssetRequests.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<AssetRequest>> GetById(Guid id)
        {
            var req = await _context.AssetRequests.FindAsync(id);
            if (req == null) return NotFound();
            return req;
        }

        [HttpPost]
        public async Task<ActionResult<AssetRequest>> Create(AssetRequest request)
        {
            request.Id = Guid.NewGuid();
            request.CreatedAt = DateTime.UtcNow;
            request.UpdatedAt = DateTime.UtcNow;
            _context.AssetRequests.Add(request);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, AssetRequest request)
        {
            if (id != request.Id) return BadRequest();
            request.UpdatedAt = DateTime.UtcNow;
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var req = await _context.AssetRequests.FindAsync(id);
            if (req == null) return NotFound();
            _context.AssetRequests.Remove(req);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
