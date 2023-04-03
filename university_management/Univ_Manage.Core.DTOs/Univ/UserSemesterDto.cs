using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ_Manage.Core.DTOs.Univ
{
    public class UserSemesterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SemesterId { get; set; }
        public int YearId { get; set; }
        public int UserId { get; set; }

    }
}
