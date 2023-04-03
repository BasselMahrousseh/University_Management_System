using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ_Manage.Infrastructure.Models.Univ
{
    [Table("UserSemesters", Schema = "Univ")]
    public class UserSemesterSet: BaseEntityName
    {
        #region Properties

        #endregion

        #region Navigation Properties
        public int SemesterId { get; set; }
        public SemesterSet Semester { get; set; }
        public int UserId { get; set; }
        public UserSet User { get; set; }
        #endregion
    }
}
