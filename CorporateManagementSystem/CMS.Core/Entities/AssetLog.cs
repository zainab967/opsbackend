using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Entities
{
    public class AssetLog
    {
        public Guid Id { get; set; }
        
        public Guid AssetId { get; set; }
        public Asset Asset { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Action { get; set; }  // e.g., "Created", "Assigned", "Transferred", "Maintenance"
        
        public Guid PerformedBy { get; set; }
        public User PerformedByUser { get; set; }
        
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;
        
        public string Notes { get; set; }   // Optional: details of the action
        
        public string PreviousValue { get; set; }  // For tracking changes
        public string NewValue { get; set; }       // For tracking changes
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
