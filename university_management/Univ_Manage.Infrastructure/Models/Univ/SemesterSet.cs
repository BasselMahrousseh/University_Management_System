using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Univ_Manage.SharedKernal.Models;

namespace Univ_Manage.Infrastructure.Models.Univ
{
    [Table("Semesters", Schema = "Univ")]

    public class SemesterSet :BaseEntityName
    {
        #region Properties

        #endregion

        #region Navigation Properties
        public int YearId { get; set; }
        public YearSet Year { get; set; }
        public ICollection<UserSemesterSet> UserSemesters { get; set; }
        public ICollection<SubjectSet> Subjects { get; set; }
        #endregion
    }
}