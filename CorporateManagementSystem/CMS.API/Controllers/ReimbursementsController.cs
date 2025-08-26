using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Infrastructure.Data;
using CMS.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace CMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReimbursementsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ReimbursementsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reimbursement>>> GetAll() => await _context.Reimbursements.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Reimbursement>> GetById(Guid id)
        {
            var reimb = await _context.Reimbursements.FindAsync(id);
            if (reimb == null) return NotFound();
            return reimb;
        }

        [HttpPost]
        public async Task<ActionResult<Reimbursement>> Create(Reimbursement reimbursement)
        {
            reimbursement.Id = Guid.NewGuid();
            reimbursement.CreatedAt = DateTime.UtcNow;
            reimbursement.UpdatedAt = DateTime.UtcNow;
            _context.Reimbursements.Add(reimbursement);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = reimbursement.Id }, reimbursement);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Reimbursement reimbursement)
        {
            if (id != reimbursement.Id) return BadRequest();
            reimbursement.UpdatedAt = DateTime.UtcNow;
            _context.Entry(reimbursement).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var reimb = await _context.Reimbursements.FindAsync(id);
            if (reimb == null) return NotFound();
            _context.Reimbursements.Remove(reimb);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
