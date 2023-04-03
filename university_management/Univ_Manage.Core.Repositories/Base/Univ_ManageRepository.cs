using Univ_Manage.Core.DTOs.Supervision;
using Univ_Manage.Core.Interfaces.Security;
using Univ_Manage.Core.Interfaces.Supervision;
using Univ_Manage.Core.Repositories.Supervision;
using Univ_Manage.Infrastructure.SqlServer;
using Univ_Manage.SharedKernal.Enums;

namespace Univ_Manage.Core.Repositories.Base
{
    public abstract class Univ_ManageRepository
    {
        #region Properties and constructors
        protected Univ_ManageDBContext Context { get; set; }
        protected IUserRepository _userRepository;
        protected ITransactionRepository _transactionRepository;

        public Univ_ManageRepository(Univ_ManageDBContext context, IUserRepository userRepository,  ITransactionRepository transactionRepository)
        {
            Context = context;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        #endregion

        #region Methods
        /*
        public async Task<bool> AddTransactionAsync(int recordId, TableNameEnum tableNameEnum,
            OperationNameEnum operationNameEnum)
        {
            try
            {
                var currentUserId = _userRepository.CurrentUserId();
                var result = await _transactionRepository.AddTransactionAsync(new TranactionDto()
                {
                    Operation = operationNameEnum,
                    RecordId = recordId,
                    Table = tableNameEnum,
                    UserId = currentUserId
                });
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }*/
        #endregion

    }
}
