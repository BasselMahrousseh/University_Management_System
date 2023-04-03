using Univ_Manage.Core.DTOs.Permission;
using Univ_Manage.SharedKernal.OperationResults;

namespace Univ_Manage.Core.Interfaces.Permission
{
    public interface IPermissionRepository
    {
        #region Get

        IEnumerable<string> GetContent(string tableName, string action);
        Task<OperationResult<HttpStatusCode, IEnumerable<PermissionDto>>> GetRolesPermissionsAsync();
        #endregion

        #region Set

        Task<OperationResult<HttpStatusCode, PermissionDto>> SetPermissionAsync(PermissionDto dto);
        #endregion
    }
}
