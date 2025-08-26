using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Entities
{
    public class Department
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public Guid? ManagerId { get; set; }
        public User Manager { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Budget { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Asset> Assets { get; set; } = new List<Asset>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Reimbursement> Reimbursements { get; set; } = new List<Reimbursement>();
        public ICollection<AssetRequest> AssetRequests { get; set; } = new List<AssetRequest>();
        public ICollection<ComplaintSuggestion> ComplaintSuggestions { get; set; } = new List<ComplaintSuggestion>();
        public ICollection<LedgerEntry> LedgerEntries { get; set; } = new List<LedgerEntry>();
    }
}
