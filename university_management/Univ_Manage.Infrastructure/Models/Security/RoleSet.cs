namespace Univ_Manage.Infrastructure.Models.Security
{
    public class RoleSet : IdentityRole<int>, IBaseEntity
    {
        #region Properties

        public bool IsDeleted { get; set; } = false;
        #endregion

        #region Navigatoin properties

        public ICollection<UserRoleSet> UserRoles { get; set; }
        #endregion
    }
}
