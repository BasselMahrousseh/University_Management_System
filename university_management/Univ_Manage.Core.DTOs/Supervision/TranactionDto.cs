using Univ_Manage.SharedKernal.Enums;

namespace Univ_Manage.Core.DTOs.Supervision
{
    public class TranactionDto
    {
        public int UserId { get; set; }
        public int RecordId { get; set; }
        public TableNameEnum Table { get; set; }
        public OperationNameEnum Operation { get; set; }
    }
}
