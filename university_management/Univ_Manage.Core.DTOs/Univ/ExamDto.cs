
namespace Univ_Manage.Core.DTOs.Univ
{
    public class ExamDto
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int SubjectId { get; set; }
        public int SchedulingId { get; set; }
        public string SchedulingName { get; set; }
        public string SubjectName { get; set; }

    }
}
