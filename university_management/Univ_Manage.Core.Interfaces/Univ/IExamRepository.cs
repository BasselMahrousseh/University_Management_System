using Univ_Manage.Core.DTOs.Security.User;
using Univ_Manage.Core.DTOs.Univ;
using Univ_Manage.SharedKernal.OperationResults;

namespace Univ_Manage.Core.Interfaces.Univ
{
    public interface IExamRepository
    {
        public OperationResult<HttpStatusCode, StudentMarks> RegisterMarks(StudentMarks dto);
        public OperationResult<HttpStatusCode, List<ExamDto>> GetExamByStudentId(int UserId);
        public OperationResult<HttpStatusCode, List<StudentDto>> GetValidStudents(int subjectId);
        public OperationResult<HttpStatusCode, ExamDto> GetStudentExamBySubject(int studentId, int subjectId);
        public string GetSchedulingName(int SchedulingId);
        public OperationResult<HttpStatusCode, List<ExamDetailDto>> GetSchedulingDetails(int SchedulingId);
        public OperationResult<HttpStatusCode, List<ExamDto>> GetSubjectExamResults(int SubjectId, int SchedulingId);

    }
}
