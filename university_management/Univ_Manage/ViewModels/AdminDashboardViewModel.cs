using Univ_Manage.Core.DTOs.Univ;

namespace Univ_Manage.ViewModels
{
    public class AdminDashboardViewModel : BasicViewModel
    {
        public int FirstYearCount { get; set; }
        public int SecondYearCount { get; set; }
        public int ThirdYearCount { get; set; }
        public int FourthYearCount { get; set; }
        public List<SchedulingDto> Last4Exams { get; set; }
    }
}
