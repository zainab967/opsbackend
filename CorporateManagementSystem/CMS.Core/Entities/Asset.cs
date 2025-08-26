using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Entities
{
    public enum AssetStatus { Assigned, Unassigned, Maintenance, Retired }
    public enum AssetCondition { Excellent, Good, Fair, Poor }

    public class Asset
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Category { get; set; }
        
        [StringLength(100)]
        public string SerialNumber { get; set; }
        
        public Guid? AssignedTo { get; set; }
        public User AssignedUser { get; set; }
        
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        
        public AssetStatus Status { get; set; } = AssetStatus.Unassigned;
        
        [Range(0, double.MaxValue)]
        public decimal Value { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal PurchasePrice { get; set; }
        
        public DateTime PurchaseDate { get; set; }
        
        public AssetCondition Condition { get; set; }
        
        [StringLength(255)]
        public string Location { get; set; }
        
        public DateTime? WarrantyExpiry { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<AssetMaintenance> AssetMaintenances { get; set; } = new List<AssetMaintenance>();
        public ICollection<AssetLog> AssetLogs { get; set; } = new List<AssetLog>();
    }
}
