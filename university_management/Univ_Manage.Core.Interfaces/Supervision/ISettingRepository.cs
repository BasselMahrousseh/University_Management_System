using Univ_Manage.Core.DTOs.Supervision;
using Univ_Manage.SharedKernal.OperationResults;

namespace Univ_Manage.Core.Interfaces.Supervision
{
    public interface ISettingRepository
    {
        #region Get
        OperationResult<HttpStatusCode, IEnumerable<SettingDto>> GetSettings();
        OperationResult<HttpStatusCode, SettingDto> GetSettingByKey(string key);
        #endregion

        #region Set

        OperationResult<HttpStatusCode, SettingDto> SetSetting(SettingDto settingDto);
        #endregion
    }
}