using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ_Manage.Infrastructure.Models.Univ
{
    [Table("Years", Schema = "Univ")]
    public class YearSet : BaseEntityName
    {
        #region Properties

        #endregion

        #region Navigation Properties
        public ICollection<SemesterSet> Semesters { get; set; }
        #endregion
    }
}
