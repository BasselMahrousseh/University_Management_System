using System.Net;
using Univ_Manage.Core.Interfaces.Security;
using Univ_Manage.Core.Interfaces.Supervision;
using Univ_Manage.Core.Repositories.Base;
using Univ_Manage.Infrastructure.SqlServer;
using Univ_Manage.SharedKernal.OperationResults;

namespace Univ_Manage.Core.Repositories.Supervision
{
    public class SettingRepository : Univ_ManageRepository, ISettingRepository
    {
        #region Properties and constructors

        public SettingRepository(Univ_ManageDBContext context,
            ITransactionRepository transactionRepository,
            IUserRepository userRepository) : base(context, userRepository, transactionRepository)
        {
        }
        #endregion

        #region Set

        public OperationResult<HttpStatusCode, SettingDto> SetSetting(SettingDto settingDto)
        {
            var result = new OperationResult<HttpStatusCode, SettingDto>();
            try
            {
                var settingSet = Context.Settings.FirstOrDefault(setting => setting.Id == settingDto.Id);
                if (settingSet == null)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("Adding new settings is not allowed");
                    return result;
                }
                settingSet.Value = settingDto.Value;
                Context.Settings.Update(settingSet);
                Context.SaveChanges();
                result.EnumResult = HttpStatusCode.OK;
                result.AddError("Updated Successfully");
                return result;
            }
            catch (Exception)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Something went wrong..! Please contact developers");
                return result;
            }
        }
        #endregion

        #region Get

        public OperationResult<HttpStatusCode, IEnumerable<SettingDto>> GetSettings()
        {
            var result = new OperationResult<HttpStatusCode, IEnumerable<SettingDto>>();
            try
            {
                result.Result = Context.Settings.Where(e => !e.IsDeleted)
                    .Select(setting => new SettingDto
                    {
                        Id = setting.Id,
                        Name = setting.Name,
                        Description = setting.Description,
                        Value = setting.Value,
                    });
                result.EnumResult = HttpStatusCode.OK;
                return result;
            }
            catch (Exception)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Something went wrong..! Please contact developer");
                return result;
            }
        }
        public OperationResult<HttpStatusCode, SettingDto> GetSettingByKey(string key)
        {
            var result = new OperationResult<HttpStatusCode, SettingDto>();
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    result.AddError("Invalid Settings key");
                    result.EnumResult=HttpStatusCode.BadRequest;
                    return result;
                }
                result.Result=Context.Settings.Where(s=>s.Name==key).Select(sdto=>new SettingDto()
                {
                    Value=sdto.Value,
                    Description=sdto.Description,
                    Name=sdto.Name,
                }).FirstOrDefault();
            }
            catch(Exception ex)
            {
                result.AddError("Something went wrong..! Please contact developer and inform him with the following error " + ex.Message);
                result.EnumResult = HttpStatusCode.InternalServerError;
            }
            return result;
        }
        #endregion
    }
}
