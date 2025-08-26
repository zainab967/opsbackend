using System;
using System.ComponentModel.DataAnnotations;
using CMS.Core.Entities;

namespace CMS.API.DTOs.Auth
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public UserRole Role { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        // Removed password field since authentication is handled externally
        
        // External system integration fields
        [StringLength(255)]
        public string ExternalUserId { get; set; }
        
        [StringLength(255)]
        public string ExternalSystemName { get; set; }

        public bool SendWelcomeEmail { get; set; } = true;
    }

    public class UpdateUserDto
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        public bool Active { get; set; }
    }

    public class UserPermissionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Permission { get; set; }
        public Guid? GrantedBy { get; set; }
        public string GrantedByName { get; set; }
        public DateTime GrantedAt { get; set; }
    }

    public class AssignPermissionDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Permission { get; set; }
    }

    public class UserQueryDto
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 20;
        public UserRole? Role { get; set; }
        public Guid? DepartmentId { get; set; }
        public string Search { get; set; }
        public bool? Active { get; set; }
    }
}
