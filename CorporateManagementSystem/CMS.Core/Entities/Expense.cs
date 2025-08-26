using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Entities
{
    public enum ExpenseStatus { Pending, Approved, Rejected }
    public enum ExpenseType { OneTime, Recurring }

    public class Expense
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
        
        public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending;
        
        public ExpenseType Type { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Category { get; set; }
        
        public string Description { get; set; }
        
        [StringLength(500)]
        public string ReceiptUrl { get; set; }
        
        public Guid? ApprovedBy { get; set; }
        public User ApprovedByUser { get; set; }
        
        public DateTime? ApprovedAt { get; set; }
        public string RejectionReason { get; set; }

        public DateTime? ReviewedAt { get; set; }
        public Guid? ReviewedById { get; set; }
        public User ReviewedByUser { get; set; }
        public string ReviewNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<ExpenseDocument> ExpenseDocuments { get; set; } = new List<ExpenseDocument>();
    }
}
