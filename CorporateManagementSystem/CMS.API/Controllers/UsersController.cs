using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using CMS.Infrastructure.Data;
using CMS.Core.Entities;
using CMS.API.DTOs.Auth;

namespace CMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all users with optional filtering
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserQueryDto query)
        {
            try
            {
                var usersQuery = _context.Users
                    .Include(u => u.Department)
                    .AsQueryable();

                // Apply filters
                if (query.Role.HasValue)
                {
                    usersQuery = usersQuery.Where(u => u.Role == query.Role.Value);
                }

                if (query.DepartmentId.HasValue)
                {
                    usersQuery = usersQuery.Where(u => u.DepartmentId == query.DepartmentId.Value);
                }

                if (query.Active.HasValue)
                {
                    usersQuery = usersQuery.Where(u => u.Active == query.Active.Value);
                }

                if (!string.IsNullOrEmpty(query.Search))
                {
                    usersQuery = usersQuery.Where(u => 
                        u.FirstName.Contains(query.Search) ||
                        u.LastName.Contains(query.Search) ||
                        u.Email.Contains(query.Search));
                }

                var totalCount = await usersQuery.CountAsync();

                var users = await usersQuery
                    .Skip((query.Page - 1) * query.Limit)
                    .Take(query.Limit)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Role = u.Role,
                        DepartmentId = u.DepartmentId,
                        DepartmentName = u.Department.Name,
                        Active = u.Active,
                        CreatedAt = u.CreatedAt,
                        UpdatedAt = u.UpdatedAt
                    })
                    .ToListAsync();

                var response = new
                {
                    success = true,
                    data = new
                    {
                        users = users,
                        pagination = new
                        {
                            page = query.Page,
                            limit = query.Limit,
                            total = totalCount,
                            totalPages = (int)Math.Ceiling((double)totalCount / query.Limit)
                        }
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = new
                    {
                        code = "INTERNAL_SERVER_ERROR",
                        message = "An error occurred while fetching users",
                        details = ex.Message
                    },
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Get a specific user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Department)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        error = new
                        {
                            code = "USER_NOT_FOUND",
                            message = "User not found"
                        },
                        timestamp = DateTime.UtcNow
                    });
                }

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    DepartmentId = user.DepartmentId,
                    DepartmentName = user.Department?.Name,
                    Active = user.Active,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

                return Ok(new
                {
                    success = true,
                    data = userDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = new
                    {
                        code = "INTERNAL_SERVER_ERROR",
                        message = "An error occurred while fetching the user",
                        details = ex.Message
                    },
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Create a new user (for integration with external system)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == createUserDto.Email.ToLower());

                if (existingUser != null)
                {
                    return Conflict(new
                    {
                        success = false,
                        error = new
                        {
                            code = "USER_EXISTS",
                            message = "A user with this email already exists"
                        },
                        timestamp = DateTime.UtcNow
                    });
                }

                // Check if department exists
                var department = await _context.Departments.FindAsync(createUserDto.DepartmentId);
                if (department == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        error = new
                        {
                            code = "DEPARTMENT_NOT_FOUND",
                            message = "Specified department does not exist"
                        },
                        timestamp = DateTime.UtcNow
                    });
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = createUserDto.Email.ToLower(),
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.LastName,
                    Role = createUserDto.Role,
                    DepartmentId = createUserDto.DepartmentId,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    DepartmentId = user.DepartmentId,
                    DepartmentName = department.Name,
                    Active = user.Active,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new
                {
                    success = true,
                    data = userDto,
                    message = "User created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = new
                    {
                        code = "INTERNAL_SERVER_ERROR",
                        message = "An error occurred while creating the user",
                        details = ex.Message
                    },
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Sync user from external system (creates or updates user)
        /// </summary>
        [HttpPost("sync")]
        public async Task<IActionResult> SyncUser([FromBody] SyncUserDto syncUserDto)
        {
            try
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == syncUserDto.Email.ToLower() ||
                                           u.ExternalUserId == syncUserDto.ExternalUserId);

                if (existingUser != null)
                {
                    // Update existing user
                    existingUser.FirstName = syncUserDto.FirstName;
                    existingUser.LastName = syncUserDto.LastName;
                    existingUser.Email = syncUserDto.Email.ToLower();
                    existingUser.Role = syncUserDto.Role;
                    existingUser.DepartmentId = syncUserDto.DepartmentId;
                    existingUser.ExternalUserId = syncUserDto.ExternalUserId;
                    existingUser.ExternalSystemName = syncUserDto.ExternalSystemName;
                    existingUser.Active = syncUserDto.Active;
                    existingUser.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        success = true,
                        message = "User updated from external system"
                    });
                }
                else
                {
                    // Create new user
                    var newUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = syncUserDto.Email.ToLower(),
                        FirstName = syncUserDto.FirstName,
                        LastName = syncUserDto.LastName,
                        Role = syncUserDto.Role,
                        DepartmentId = syncUserDto.DepartmentId,
                        ExternalUserId = syncUserDto.ExternalUserId,
                        ExternalSystemName = syncUserDto.ExternalSystemName,
                        Active = syncUserDto.Active,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, new
                    {
                        success = true,
                        message = "User created from external system"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = new
                    {
                        code = "INTERNAL_SERVER_ERROR",
                        message = "An error occurred while syncing the user",
                        details = ex.Message
                    },
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }

    // DTO for syncing users from external system
    public class SyncUserDto
    {
        public string ExternalUserId { get; set; }
        public string ExternalSystemName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public Guid DepartmentId { get; set; }
        public bool Active { get; set; } = true;
    }
}
