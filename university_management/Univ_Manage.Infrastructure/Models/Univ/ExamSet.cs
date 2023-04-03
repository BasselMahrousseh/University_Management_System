using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ_Manage.Infrastructure.Models.Univ
{
    [Table("Exams", Schema = "Univ")]
    public class ExamSet:BaseEntity
    {
        #region Properties
        public int Grade { get; set; }
        #endregion

        #region Navigation Properties

        public int UserId { get; set; }
        public UserSet User { get; set; }
        public int SubjectId { get; set; }
        public SubjectSet Subject { get; set; }
        public int SchedulingId { get; set; }
        public SchedulingSet Scheduling { get; set; }
        #endregion
    }
}
