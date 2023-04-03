namespace Univ_Manage.Infrastructure.Models.Security
{
    public class UserClaimSet : IdentityUserClaim<int>, IBaseEntity
    {
        #region Properties

        public bool IsDeleted { get; set; }
        #endregion]
    }
}
