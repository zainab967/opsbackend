using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Entities
{
    public enum ReimbursementStatus { Pending, Approved, Rejected, Paid }
    public enum ReimbursementType { Medical, Travel, Equipment, Other }

    public class Reimbursement
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        
        public DateTime Date { get; set; }
        
        public ReimbursementStatus Status { get; set; } = ReimbursementStatus.Pending;
        
        public ReimbursementType Type { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Category { get; set; }
        
        public string Description { get; set; }
        
        public string[] ReceiptUrls { get; set; }
        
        public Guid? ApprovedBy { get; set; }
        public User ApprovedByUser { get; set; }
        
        public DateTime? ApprovedAt { get; set; }
        public string RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
