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
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DepartmentsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAll() => await _context.Departments.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetById(Guid id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();
            return dept;
        }

        [HttpPost]
        public async Task<ActionResult<Department>> Create(Department dept)
        {
            dept.Id = Guid.NewGuid();
            dept.CreatedAt = DateTime.UtcNow;
            dept.UpdatedAt = DateTime.UtcNow;
            _context.Departments.Add(dept);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dept.Id }, dept);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Department dept)
        {
            if (id != dept.Id) return BadRequest();
            dept.UpdatedAt = DateTime.UtcNow;
            _context.Entry(dept).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();
            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
