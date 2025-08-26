using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Entities
{
    public enum DurationType { Permanent, Temporary }
    public enum RequestStatus { Pending, Approved, Rejected, Fulfilled }

    public class AssetRequest
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        [Required]
        [StringLength(255)]
        public string AssetName { get; set; }
        
        public DateTime RequestedDate { get; set; }
        
        [Required]
        public string Reason { get; set; }
        
        public DurationType DurationType { get; set; }
        
        public DateTime? ReturnDate { get; set; }
        
        public string Specifications { get; set; }
        
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        
        public Guid? ApprovedBy { get; set; }
        public User ApprovedByUser { get; set; }
        
        public DateTime? ApprovedAt { get; set; }
        public string RejectionReason { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
