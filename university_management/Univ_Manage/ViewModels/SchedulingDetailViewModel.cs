using Univ_Manage.Core.DTOs.Univ;

namespace Univ_Manage.ViewModels
{
    public class SchedulingDetailViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<ExamDetailDto> Exams { get; set; }
    }
}
