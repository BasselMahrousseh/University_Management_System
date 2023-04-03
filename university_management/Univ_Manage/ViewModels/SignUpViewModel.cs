using Univ_Manage.Core.DTOs.Univ;

namespace Univ_Manage.ViewModels
{
    public class SignUpViewModel
    {
        public List<YearDto> Years { get; set; }
        public List<DepartmentDto> Departments { get; set; }
    }
}
