using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ_Manage.Infrastructure.Models.Univ
{
    [Table("Schedulings", Schema = "Univ")]
    public class SchedulingSet:BaseEntityName
    {
        #region Properties

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        #endregion

        #region Navigation Properties

        public ICollection<ExamSet> Exams { get; set; }
        #endregion

    }
}
