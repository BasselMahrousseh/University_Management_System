using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ_Manage.Infrastructure.Models.Univ
{
    [Table("Subjects", Schema = "Univ")]
    public class SubjectSet:BaseEntityName
    {
        #region Properties
        public string Description { get; set; }
        #endregion

        #region Navigation Properties

        public int DepartmentId { get; set; }
        public DepartmetSet Department { get; set; }
        public int SemesterId { get; set; }
        public SemesterSet Semester { get; set; }

        public ICollection<ExamSet> Exams { get; set; }
        #endregion
    }
}
