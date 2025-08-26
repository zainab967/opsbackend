using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Entities
{
    public enum MaintenanceType { Repair, Replacement, Upgrade, Inspection }
    public enum MaintenanceStatus { Pending, InProgress, Completed, Cancelled }
    public enum PriorityLevel { Low, Medium, High, Critical }

    public class AssetMaintenance
    {
        public Guid Id { get; set; }
        
        public Guid AssetId { get; set; }
        public Asset Asset { get; set; }
        
        public Guid ReportedBy { get; set; }
        public User ReportedByUser { get; set; }
        
        [Required]
        public string IssueDescription { get; set; }
        
        public MaintenanceType MaintenanceType { get; set; }
        
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
        
        public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Pending;
        
        public Guid? AssignedTo { get; set; }
        public User AssignedToUser { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? EstimatedCost { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? ActualCost { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }
        
        public DateTime Date { get; set; }
        
        public DateTime? CompletedAt { get; set; }
        
        public string Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
