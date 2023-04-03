using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ_Manage.Infrastructure.Models.Univ
{
    [Table("Departments", Schema = "Univ")]
    public class DepartmetSet:BaseEntityName
    {
        #region Properties
        public string Description { get; set; }
        #endregion

        #region Navigation Properties

        public ICollection<SubjectSet> Subjects { get; set; }
        public ICollection<UserSet> Users { get; set; }
        #endregion
    }
}
