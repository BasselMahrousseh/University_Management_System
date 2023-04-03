using System.Net;
using Univ_Manage.Core.DTOs.Security.Role;
using Univ_Manage.Core.DTOs.Security.User;
using Univ_Manage.Core.DTOs.Univ;
using Univ_Manage.SharedKernal.OperationResults;

namespace Univ_Manage.Core.Interfaces.Security
{
    public interface IUserRepository
    {
        #region Get
        Task<(int, string)> CheckUser(UserDto dto);
        public int CurrentUserId();
        public OperationResult<HttpStatusCode, IEnumerable<RoleDto>> GetRoles();
        public UserRoleStatusDto GetUserRoleStatus(int userId);
        public OperationResult<HttpStatusCode, List<StudentDto>> GetStudents();
        public double GetAverage(int userId);
        public double GetAnualAverage(int userId, int yearId, bool anual);
        #endregion

        #region Set
        public Task<OperationResult<HttpStatusCode, UserDto>> SetUser(UserDto dto);
        public OperationResult<HttpStatusCode, List<SubjectDto>> GetUserSubjects(int UserId);

        #endregion

        #region Remove

        #endregion
    }
}
