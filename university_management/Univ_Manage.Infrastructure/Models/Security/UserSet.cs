using System.ComponentModel.DataAnnotations;
using Univ_Manage.Infrastructure.Models.Univ;

namespace Univ_Manage.Infrastructure.Models.Security
{
    public class UserSet : IdentityUser<int>, IBaseEntity
    {
        #region Properties 

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public bool IsDeleted { get; set; } = false;
        #endregion

        #region Navigation Properties

        public ICollection<UserRoleSet> UserRoles { get; set; }
        [AllowNull]
        public int? DepartmentId { get; set; }
        public DepartmetSet Department { get; set; }
        public ICollection<UserSemesterSet> UserSemesters { get; set; }
        #endregion
    }
}
