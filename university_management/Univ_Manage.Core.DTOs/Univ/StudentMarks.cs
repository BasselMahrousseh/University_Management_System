using Univ_Manage.Core.DTOs.Security.User;

namespace Univ_Manage.Core.DTOs.Univ
{
    public class StudentMarks
    {
        public int SubjectId { get; set; }
        public List<StudentMarkDto> StudentsMarks { get; set; }
    }
}
