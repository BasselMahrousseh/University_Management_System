using Univ_Manage.Core.DTOs.Univ;
using Univ_Manage.SharedKernal.OperationResults;

namespace Univ_Manage.Core.Interfaces.Univ
{
    public interface IDepartmentRepository
    {
        public OperationResult<HttpStatusCode, List<YearDto>> GetYears();
        public OperationResult<HttpStatusCode, List<DepartmentDto>> GetDepartments();
    }
}
