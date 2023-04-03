using System.Net;
using Univ_Manage.Core.DTOs.Univ;
using Univ_Manage.Core.Interfaces.Security;
using Univ_Manage.Core.Interfaces.Supervision;
using Univ_Manage.Core.Interfaces.Univ;
using Univ_Manage.Infrastructure.SqlServer;
using Univ_Manage.SharedKernal.OperationResults;

namespace Univ_Manage.Core.Repositories.Univ
{
    public class DepartmentRepository : Univ_ManageRepository,IDepartmentRepository
    {
        #region Properties and constructors

        public DepartmentRepository(Univ_ManageDBContext context, IUserRepository userRepository, ITransactionRepository transactionRepository) : base(context, userRepository, transactionRepository)
        {
        }
        #endregion

        #region Set

        #endregion

        #region Get
        public OperationResult<HttpStatusCode, List<YearDto>> GetYears()
        {
            var result = new OperationResult<HttpStatusCode, List<YearDto>>();
            try
            {
                result.Result = Context.Years.Select(y => new YearDto
                {
                    Id = y.Id,
                    Name = y.Name
                }).ToList();
                result.EnumResult = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Something went wrong..! Please contact developer");
            }
            return result;
        }
        public OperationResult<HttpStatusCode, List<DepartmentDto>> GetDepartments()
        {
            var result = new OperationResult<HttpStatusCode, List<DepartmentDto>>();
            try
            {
                result.Result = Context.Departments.Select(y => new DepartmentDto
                {
                    Id = y.Id,
                    Name = y.Name,
                    Description = y.Description
                }).ToList();
                result.EnumResult = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Something went wrong..! Please contact developer");
            }
            return result;
        }
        #endregion

        #region Remove

        #endregion


    }
}
