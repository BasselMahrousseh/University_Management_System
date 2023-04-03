using Univ_Manage.SharedKernal.Enums;

namespace Univ_Manage.Core.DTOs.Permission
{
    public class ContentDto
    {
        public ContentNameEnum Content { get; set; }
        public string ContentName { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDownload { get; set; }
        public bool CanDelete { get; set; }
    }
}
