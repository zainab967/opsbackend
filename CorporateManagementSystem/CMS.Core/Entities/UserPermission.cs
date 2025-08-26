using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Core.Entities
{
    public class UserPermission
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Permission { get; set; }
        
        public Guid? GrantedBy { get; set; }
        public User GrantedByUser { get; set; }
        
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    }
}
