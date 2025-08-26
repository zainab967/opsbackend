using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMS.Infrastructure.Data;
using CMS.Core.Entities;



namespace CMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintSuggestionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ComplaintSuggestionsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComplaintSuggestion>>> GetAll() => await _context.ComplaintSuggestions.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<ComplaintSuggestion>> GetById(Guid id)
        {
            var cs = await _context.ComplaintSuggestions.FindAsync(id);
            if (cs == null) return NotFound();
            return cs;
        }

        [HttpPost]
        public async Task<ActionResult<ComplaintSuggestion>> Create(ComplaintSuggestion cs)
        {
            cs.Id = Guid.NewGuid();
            cs.CreatedAt = DateTime.UtcNow;
            cs.UpdatedAt = DateTime.UtcNow;
            _context.ComplaintSuggestions.Add(cs);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = cs.Id }, cs);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ComplaintSuggestion cs)
        {
            if (id != cs.Id) return BadRequest();
            cs.UpdatedAt = DateTime.UtcNow;
            _context.Entry(cs).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cs = await _context.ComplaintSuggestions.FindAsync(id);
            if (cs == null) return NotFound();
            _context.ComplaintSuggestions.Remove(cs);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
