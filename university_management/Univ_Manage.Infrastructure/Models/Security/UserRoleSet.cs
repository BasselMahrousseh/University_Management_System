//using Univ_Manage.Infrastructure.Models.Zoning;

using Univ_Manage.Infrastructure.Models.Univ;

namespace Univ_Manage.Infrastructure.Models.Security
{
    public class UserRoleSet : IdentityUserRole<int>, IBaseEntity
    {
        #region Properties

        public int Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Required, NotNull]
        public DateTime StartDate { get; set; }
        [AllowNull]
        public DateTime EndDate { get; set; }
        #endregion

        #region Navigatoin properties

        public UserSet User { get; set; }
        public RoleSet Role { get; set; }

        #endregion
    }
}
