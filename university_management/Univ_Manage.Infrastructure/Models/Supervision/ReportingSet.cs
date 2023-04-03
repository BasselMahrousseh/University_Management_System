using Univ_Manage.Infrastructure.Models.Security;

namespace Univ_Manage.Infrastructure.Models.Supervision
{
    [Table("Reporings", Schema = "Supervision")]
    public class ReportingSet : BaseEntityName
    {
        public string Query { get; set; }
        public int UserId { get; set; }
        public UserSet User { get; set; }
    }
}
