using Univ_Manage.Core.DTOs.Univ;
using Univ_Manage.SharedKernal.OperationResults;

namespace Univ_Manage.Core.Interfaces.Univ
{
    public interface ISemesterRepository
    {
        public int GetUserCountInSpecificYear(int yearId);
        public OperationResult<HttpStatusCode, List<SemesterDto>> GetAllSemesters();
    }
}
