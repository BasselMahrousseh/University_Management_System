using Univ_Manage.Core.DTOs.Univ;
using Univ_Manage.SharedKernal.OperationResults;

namespace Univ_Manage.Core.Interfaces.Univ
{
    public interface ISubjectRepository
    {
        public OperationResult<HttpStatusCode, List<SubjectDto>> GetAllSubjects();
        public OperationResult<HttpStatusCode, List<SubjectDto>> GetSemesterSubjects(int SemesterId);
        public OperationResult<HttpStatusCode, List<SubjectDto>> GetDepartmentSubjects(int DepartmentId);
    }
}
