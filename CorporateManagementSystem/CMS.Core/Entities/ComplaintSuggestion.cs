using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Entities
{
    public enum FeedbackType { Complaint, Suggestion }
    public enum FeedbackStatus { Open, InProgress, Resolved, Closed }

    public class ComplaintSuggestion
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public FeedbackType Type { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Category { get; set; }
        
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
        
        public FeedbackStatus Status { get; set; } = FeedbackStatus.Open;
        
        public Guid SubmittedBy { get; set; }
        public User SubmittedByUser { get; set; }
        
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        
        public Guid? AssignedTo { get; set; }
        public User AssignedToUser { get; set; }
        
        public string Resolution { get; set; }
        
        public DateTime? ResolvedAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
