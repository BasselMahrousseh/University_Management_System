using System;
using System.Collections.Generic;
using System.Linq;
namespace Univ_Manage.Core.DTOs.Univ
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public int SemesterId { get; set; }
        public int YearId { get; set; }
    }
}
