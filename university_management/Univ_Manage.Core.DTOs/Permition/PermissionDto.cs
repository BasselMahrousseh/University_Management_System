namespace Univ_Manage.Core.DTOs.Permission
{
    public class PermissionDto
    {
        public PermissionDto()
        {
            Contents = new List<ContentDto>();
        }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<ContentDto> Contents { get; set; }
    }
}
