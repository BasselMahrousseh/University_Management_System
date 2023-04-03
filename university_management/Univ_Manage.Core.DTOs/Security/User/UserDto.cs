using Univ_Manage.SharedKernal.Enums;

namespace Univ_Manage.Core.DTOs.Security.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public RoleEnum Role { get; set; }
        public string RoleName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public int YearId { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
