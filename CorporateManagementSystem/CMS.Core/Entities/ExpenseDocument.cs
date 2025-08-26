using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Entities
{
    public class ExpenseDocument
    {
        public Guid Id { get; set; }
        
        public Guid ExpenseId { get; set; }
        public Expense Expense { get; set; }
        
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
        
        [Required]
        [StringLength(500)]
        public string FileUrl { get; set; }
        
        [Required]
        [StringLength(50)]
        public string FileType { get; set; }
        
        public int? FileSize { get; set; }
        
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
