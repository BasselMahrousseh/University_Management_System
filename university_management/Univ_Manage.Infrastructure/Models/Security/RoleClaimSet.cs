
namespace Univ_Manage.Infrastructure.Models.Security
{
    public class RoleClaimSet : IdentityRoleClaim<int>, IBaseEntity
    {
        public bool IsDeleted { get; set; }
    }
}
