namespace Univ_Manage.Infrastructure.Models.Supervision
{
    [Table("Settings", Schema = "Supervision")]
    public class SettingSet : BaseEntity
    {
        [Editable(false), Required]
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
