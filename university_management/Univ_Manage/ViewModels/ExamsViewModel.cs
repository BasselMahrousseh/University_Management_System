using Microsoft.AspNetCore.Mvc.Rendering;
using Univ_Manage.Core.DTOs.Security.User;
using Univ_Manage.Core.DTOs.Univ;

namespace Univ_Manage.ViewModels
{
    public class ExamsViewModel:BasicViewModel
    {
        public List<StudentDto> Students { get; set; }
        public List<SubjectDto> Subjects { get; set; }
        public List<YearDto> Years { get; set; }
        public List<DepartmentDto> Departments { get; set; }
    }
}
