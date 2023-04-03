

using Univ_Manage.Core.DTOs.Supervision;

namespace Univ_Manage.Core.Interfaces.Supervision
{
    public interface ITransactionRepository
    {
        #region Set

        Task<bool> AddTransactionAsync(TranactionDto dto);
        #endregion
    }
}
