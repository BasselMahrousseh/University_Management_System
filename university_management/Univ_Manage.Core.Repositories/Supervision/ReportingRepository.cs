using Univ_Manage.Core.Interfaces.Security;
using Univ_Manage.Core.Interfaces.Supervision;
using Univ_Manage.Core.Repositories.Base;
using Univ_Manage.Infrastructure.SqlServer;

namespace Univ_Manage.Core.Repositories.Supervision
{
    public class ReportingRepository : Univ_ManageRepository, IReportingRepository
    {
        #region Properties and constructors

        public ReportingRepository(Univ_ManageDBContext context,
            ITransactionRepository transactionRepository,
            IUserRepository userRepository) : base(context, userRepository, transactionRepository)
        {
        }
        #endregion
    }
}
