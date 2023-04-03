namespace Univ_Manage.Infrastructure.Models.Security
{
    public class UserTokenSet : IdentityUserToken<int>, IBaseEntity
    {
        public bool IsDeleted { get; set; }
    }
}
