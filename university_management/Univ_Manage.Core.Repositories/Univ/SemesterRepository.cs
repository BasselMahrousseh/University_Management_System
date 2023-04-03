using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Univ_Manage.Core.DTOs.Univ;
using Univ_Manage.Core.Interfaces.Security;
using Univ_Manage.Core.Interfaces.Supervision;
using Univ_Manage.Core.Interfaces.Univ;
using Univ_Manage.Infrastructure.Models.Security;
using Univ_Manage.Infrastructure.SqlServer;
using Univ_Manage.SharedKernal.OperationResults;


namespace Univ_Manage.Core.Repositories.Univ
{
    public class SemesterRepository : Univ_ManageRepository, ISemesterRepository
    {
        #region Properties and constructors
       
        public SemesterRepository(Univ_ManageDBContext context, IUserRepository userrepository
            , ITransactionRepository transactionRepository)
            : base(context, userrepository, transactionRepository)
        {

        }
        #endregion

        #region Set
        public OperationResult<HttpStatusCode, UserSemesterDto> SetUserSemester(UserSemesterDto userSemesterDto)
        {
            var result = new OperationResult<HttpStatusCode, UserSemesterDto>();
            try
            {
                var userSemesterEnitiy = Context.UserSemesters.FirstOrDefault(userSemester => userSemester.Id == userSemesterDto.Id);
                var operation = OperationNameEnum.Update;
                if (userSemesterEnitiy == null)
                {
                    userSemesterEnitiy = new UserSemesterSet();
                    operation = OperationNameEnum.Create;
                }

                userSemesterEnitiy.Name = userSemesterDto.Name;
                userSemesterEnitiy.UserId = userSemesterDto.UserId;
                userSemesterEnitiy.SemesterId = userSemesterDto.SemesterId;

                if (operation == OperationNameEnum.Update)
                {
                    Context.UserSemesters.Update(userSemesterEnitiy);
                }
                else
                {
                    Context.UserSemesters.Add(userSemesterEnitiy);
                }
                Context.SaveChanges();

                userSemesterDto.Id = userSemesterEnitiy.Id;
                result.EnumResult = HttpStatusCode.OK;
                result.Result = userSemesterDto;
                return result;
            }
            catch (Exception)
            {
                result.AddError("Error while adding the new UserSemester");
                result.EnumResult = HttpStatusCode.InternalServerError;
                return result;
            }

        }
        #endregion

        #region Get
        #region Get by Id

        public OperationResult<HttpStatusCode, UserSemesterDto> GetUserSemesterById(int Id)
        {
            var result = new OperationResult<HttpStatusCode, UserSemesterDto>();
            try
            {
                if (Id == 0)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("UserSemester doesn't exist or has been deleted");
                }
                else
                {
                    var UserSemester = Context.UserSemesters.Where(e => !e.IsDeleted
                    && e.Id == Id).Select(e => new UserSemesterDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        UserId = e.UserId,
                        SemesterId = e.SemesterId,
                    }).FirstOrDefault();

                    if (UserSemester == null)
                    {
                        result.EnumResult = HttpStatusCode.BadRequest;
                        result.AddError("UserSemester doesn't exist or has been deleted");
                    }
                    else
                    {
                        result.Result = UserSemester;
                        result.EnumResult = HttpStatusCode.OK;
                    }
                }
            }
            catch (Exception)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Something went wrong..! Please contact the developer");
            }
            return result;

        }
        #endregion
        #region Get by UserId

        public OperationResult<HttpStatusCode, UserSemesterDto> GetUserSemesterByStudentId(int UserId)
        {
            var result = new OperationResult<HttpStatusCode, UserSemesterDto>();
            try
            {
                if (UserId == 0)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("UserSemester doesn't exist or has been deleted");
                }
                else
                {
                    var UserSemester = Context.UserSemesters.Where(e => !e.IsDeleted
                    && e.UserId == UserId).Select(e => new UserSemesterDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        UserId = e.UserId,
                        SemesterId = e.SemesterId,
                    }).FirstOrDefault();

                    if (UserSemester == null)
                    {
                        result.EnumResult = HttpStatusCode.BadRequest;
                        result.AddError("UserSemester doesn't exist or has been deleted");
                    }
                    else
                    {
                        result.Result = UserSemester;
                        result.EnumResult = HttpStatusCode.OK;
                    }
                }
            }
            catch (Exception)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Something went wrong..! Please contact the developer");
            }
            return result;

        }
        #endregion
        #region Get by AdminId

        public OperationResult<HttpStatusCode, UserSemesterDto> GetUserSemesterByStudentAdminId(int AdminId)
        {
            var result = new OperationResult<HttpStatusCode, UserSemesterDto>();
            try
            {
                if (AdminId == 0)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("UserSemester doesn't exist or has been deleted");
                }
                else
                {
                    var UserSemester = Context.UserSemesters.Where(e => !e.IsDeleted
                    ).Select(e => new UserSemesterDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        UserId = e.UserId,
                        SemesterId = e.SemesterId,
                    }).FirstOrDefault();

                    if (UserSemester == null)
                    {
                        result.EnumResult = HttpStatusCode.BadRequest;
                        result.AddError("UserSemester doesn't exist or has been deleted");
                    }
                    else
                    {
                        result.Result = UserSemester;
                        result.EnumResult = HttpStatusCode.OK;
                    }
                }
            }
            catch (Exception)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Something went wrong..! Please contact the developer");
            }
            return result;

        }

        public int GetUserCountInSpecificYear(int yearId)
        {

            var count = Context.UserSemesters.Count(entity => !entity.IsDeleted
                            && entity.Semester.YearId == yearId);
            return count;
        }
        public int GetUserCountInSpecificSemester(int semesterId)
        {

            var count = Context.UserSemesters.Count(entity => !entity.IsDeleted
                            && entity.SemesterId == semesterId);
            return count;
        }

        public IEnumerable<UserSemesterSet> GetStudentsInSpecificYear(int yearId)
        {

            var students = Context.UserSemesters.Where(entity => !entity.IsDeleted
                            && entity.Semester.YearId == yearId);
            return students;
        }
        public IEnumerable<UserSemesterSet> GetStudentsInSpecificSemester(int semesterId)
        {

            var students = Context.UserSemesters.Where(entity => !entity.IsDeleted
                            && entity.SemesterId == semesterId);
            return students;
        }

        public OperationResult<HttpStatusCode,List<SemesterDto>> GetAllSemesters()
        {
            var result = new OperationResult<HttpStatusCode,List<SemesterDto>>();
            try
            {
                result.Result = Context.Semesters.Select(s => new SemesterDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    YearId = s.YearId
                }).ToList();
                result.EnumResult = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError(e.Message+"حدث خطأ ما يرجى إعلام الدعم الفني بالخطأ التالي");
            }
            return result;
        }
        #endregion


        #endregion

        #region Remove

        #endregion

    }
}
