using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Entities
{
    public enum EntryType { Expense, Reimbursement, AssetPurchase, Adjustment }

    public class LedgerEntry
    {
        public Guid Id { get; set; }
        
        public Guid ReferenceId { get; set; } // Can reference expense, reimbursement, etc.
        
        public EntryType ReferenceType { get; set; }
        
        [Required]
        [StringLength(20)]
        public string AccountCode { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal DebitAmount { get; set; } = 0;
        
        [Range(0, double.MaxValue)]
        public decimal CreditAmount { get; set; } = 0;
        
        public Guid? DepartmentId { get; set; }
        public Department Department { get; set; }
        
        public Guid PostedBy { get; set; }
        public User PostedByUser { get; set; }
        
        public DateTime PostingDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
