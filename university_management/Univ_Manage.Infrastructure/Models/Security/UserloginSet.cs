namespace Univ_Manage.Infrastructure.Models.Security
{
    public class UserloginSet : IdentityUserLogin<int>, IBaseEntity
    {
        public bool IsDeleted { get; set; }
    }
}
