using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CMS.Infrastructure.Data;
using CMS.Core.Entities;

namespace CMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LedgerController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LedgerController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LedgerEntry>>> GetAll() => await _context.LedgerEntries.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<LedgerEntry>> GetById(Guid id)
        {
            var ledger = await _context.LedgerEntries.FindAsync(id);
            if (ledger == null) return NotFound();
            return ledger;
        }

        [HttpPost]
        public async Task<ActionResult<LedgerEntry>> Create(LedgerEntry ledger)
        {
            ledger.Id = Guid.NewGuid();
            ledger.CreatedAt = DateTime.UtcNow;
            _context.LedgerEntries.Add(ledger);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = ledger.Id }, ledger);
        }


    }
}
