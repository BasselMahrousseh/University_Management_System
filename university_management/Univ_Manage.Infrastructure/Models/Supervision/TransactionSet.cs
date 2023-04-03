using Univ_Manage.SharedKernal.Enums;

namespace Univ_Manage.Infrastructure.Models.Supervision
{
    [Table("Transactions", Schema = "Supervision")]

    public class TransactionSet
    {
        #region Properties

        public uint Id { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int RecordId { get; set; }
        #endregion

        #region Navigation Properties

        [Required, NotNull]
        public TableNameEnum Table { get; set; }
        public OperationNameEnum Operation { get; set; }
        #endregion
    }
}
