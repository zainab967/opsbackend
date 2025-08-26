using System;
using System.Collections.Generic;
using CMS.Core.Entities;

namespace CMS.API.DTOs
{
    // Common DTOs
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ErrorResponse
    {
        public bool Success { get; set; } = false;
        public ErrorDetails Error { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ErrorDetails
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public List<FieldError> Details { get; set; } = new List<FieldError>();
    }

    public class FieldError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }

    public class PaginationResponse<T>
    {
        public List<T> Items { get; set; }
        public PaginationInfo Pagination { get; set; }
    }

    public class PaginationInfo
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
    }

    // User DTOs
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public UserRole Role { get; set; }
        public string Department { get; set; }
        public Guid DepartmentId { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateUserDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public Guid DepartmentId { get; set; }
        public string Password { get; set; }
        public bool SendWelcomeEmail { get; set; }
    }

    public class UpdateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public Guid DepartmentId { get; set; }
        public bool Active { get; set; }
    }

    // Authentication DTOs
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
    }

    // Department DTOs
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ManagerId { get; set; }
        public string ManagerName { get; set; }
        public decimal Budget { get; set; }
        public decimal CurrentSpending { get; set; }
        public int EmployeeCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateDepartmentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ManagerId { get; set; }
        public decimal Budget { get; set; }
    }
}
