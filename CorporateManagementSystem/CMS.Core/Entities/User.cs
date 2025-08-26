using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Entities
{
    public enum UserRole { Admin, Manager, HR, Employee }

    public class User
    {
        public Guid Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        // Removed password field since authentication is handled externally
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        
        public UserRole Role { get; set; } = UserRole.Employee;
        
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // External system integration fields
        [StringLength(255)]
        public string ExternalUserId { get; set; } // ID from the external authentication system
        
        [StringLength(255)]
        public string ExternalSystemName { get; set; } // Name of the external system

        // Navigation Properties
        public ICollection<Asset> AssignedAssets { get; set; } = new List<Asset>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Reimbursement> Reimbursements { get; set; } = new List<Reimbursement>();
        public ICollection<AssetRequest> AssetRequests { get; set; } = new List<AssetRequest>();
        public ICollection<ComplaintSuggestion> ComplaintSuggestions { get; set; } = new List<ComplaintSuggestion>();
        public ICollection<AssetMaintenance> ReportedMaintenances { get; set; } = new List<AssetMaintenance>();
        public ICollection<AssetMaintenance> AssignedMaintenances { get; set; } = new List<AssetMaintenance>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}
