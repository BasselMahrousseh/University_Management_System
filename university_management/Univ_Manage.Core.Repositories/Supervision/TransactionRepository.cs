using Univ_Manage.Core.Interfaces.Supervision;
using Univ_Manage.Infrastructure.Models.Supervision;
using Univ_Manage.Core.Repositories.Base;
using Univ_Manage.Core.DTOs.Supervision;
using Univ_Manage.Infrastructure.SqlServer;

namespace Univ_Manage.Core.Repositories.Supervision
{
    public class TransactionRepository : ITransactionRepository
    {
        #region Properties and constructors

        protected Univ_ManageDBContext Context { get; set; }

        public TransactionRepository(Univ_ManageDBContext context)
        {
            Context = context;
        }
        #endregion

        #region Set

        public async Task<bool> AddTransactionAsync(TranactionDto dto)
        {
            try
            {
                if (dto.UserId == 0 || dto.RecordId == 0)
                    return false;

                var transactionEntity = new TransactionSet()
                {
                    Date = DateTime.Now,
                    Operation = dto.Operation,
                    RecordId = dto.RecordId,
                    Table = dto.Table,
                    UserId = dto.UserId
                };
                await Context.Transactions.AddAsync(transactionEntity);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
