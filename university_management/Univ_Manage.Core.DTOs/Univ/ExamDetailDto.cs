using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ_Manage.Core.DTOs.Univ
{
    public class ExamDetailDto
    {
        public string SubjectName { get; set; }
        public int SubjectId { get; set; }
        public string DepartmentName { get; set; }
        public int NumberOfParticipants { get; set; }
        public double SuccsessRatio { get; set; }
    }
}
