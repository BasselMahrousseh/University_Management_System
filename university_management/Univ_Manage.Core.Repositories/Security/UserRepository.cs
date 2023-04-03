using Univ_Manage.Core.DTOs.Security.Role;
using Univ_Manage.Core.Interfaces.Security;
using Univ_Manage.Core.Interfaces.Supervision;
using Univ_Manage.Infrastructure.Models.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Univ_Manage.Infrastructure.SqlServer;
using Univ_Manage.SharedKernal.OperationResults;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Univ_Manage.Core.DTOs.Security.User;
using Univ_Manage.Core.DTOs.Univ;

namespace Univ_Manage.Core.Repositories.Security
{
    public class UserRepository : IUserRepository
    {
        #region Properties and constructors

        private readonly UserManager<UserSet> _userManager;
        private readonly SignInManager<UserSet> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITransactionRepository _transactionRepository;

        protected Univ_ManageDBContext Context { get; set; }

        public UserRepository(Univ_ManageDBContext context,
            ITransactionRepository transactionRepository,
            UserManager<UserSet> userManager,
            SignInManager<UserSet> signInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            Context = context;
            _transactionRepository = transactionRepository;
        }
        #endregion

        #region Set
        public async Task<OperationResult<HttpStatusCode, UserDto>> SetUser(UserDto dto)
        {
            var result = new OperationResult<HttpStatusCode, UserDto>();

            #region validation

            if (string.IsNullOrEmpty(dto.Username))
            {
                result.AddError("username cannot be null");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }
            if (dto.Password != dto.PasswordConfirmation)
            {
                result.AddError("Password and Password confirmation does not match");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }
            var sameUsernameCount = GetUsernameCount(dto.Username, dto.UserId);

            if (sameUsernameCount > 0)
            {
                result.EnumResult = HttpStatusCode.BadRequest;
                result.AddError("cannot repeate username");
                return result;
            }

            var role = await Context.Roles.FirstOrDefaultAsync(e => e.Name == dto.Role.ToString());
            if (role is null)
            {
                result.EnumResult = HttpStatusCode.BadRequest;
                result.AddError("role does not exist");
                return result;
            }

            if (dto.UserId > 0)
            {
                var userEntity = await _userManager.FindByNameAsync("dev");
                if (dto.UserId == userEntity.Id)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("you cannot edit this user");
                    return result;
                }
            }
            #endregion

            using (var trans = await Context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userEntity = await _userManager.FindByIdAsync(dto.UserId.ToString());
                    if (userEntity == null)
                    {
                        if (string.IsNullOrEmpty(dto.Password))
                        {
                            await trans.RollbackAsync();
                            result.AddError("كلمة السر لا يجب أن تكون فارغة");
                            result.EnumResult = HttpStatusCode.BadRequest;
                            return result;
                        }
                        foreach (var validator in _userManager.PasswordValidators)
                        {
                            var validPasswordResult = await validator.ValidateAsync(_userManager, null, dto.Password);
                            if (!validPasswordResult.Succeeded)
                            {
                                await trans.RollbackAsync();

                                foreach (var error in validPasswordResult.Errors)
                                {
                                    result.AddError(error.Description);
                                }
                                result.EnumResult = HttpStatusCode.BadRequest;
                                return result;
                            }
                        }
                        #region adding user

                        userEntity = new UserSet()
                        {
                            FirstName = dto.FirstName,
                            LastName = dto.LastName,
                            UserName = dto.Username,
                            Email = dto.Email,
                            EmailConfirmed = true,
                            DepartmentId = dto.DepartmentId,
                        };
                        var userRoles = GetRoles();
                        var userResult = await _userManager.CreateAsync(userEntity, dto.Password);
                        if (!userResult.Succeeded)
                        {
                            await trans.RollbackAsync();
                            foreach (var error in userResult.Errors)
                            {
                                result.AddError(error.Description);
                            }
                            result.EnumResult = HttpStatusCode.BadRequest;

                            return result;
                        }
                        Context.UserSemesters.Add(new UserSemesterSet()
                        {
                            SemesterId = Context.Semesters.FirstOrDefault(s => s.YearId == dto.YearId).Id,
                            UserId = userEntity.Id,
                            Name = dto.Username + Context.Semesters.FirstOrDefault(s => s.YearId == dto.YearId).Name
                        });
                        Context.UserRoles.Add(new UserRoleSet()
                        {
                            UserId = userEntity.Id,
                            RoleId = userRoles.Result.FirstOrDefault(r => r.RoleName == Enum.GetName(typeof(RoleEnum), dto.Role)).RoleId
                        });
                        Context.SaveChanges();
                        #endregion
                    }
                    dto.UserId = userEntity.Id;
                    result.EnumResult = HttpStatusCode.OK;
                    result.Result = dto;
                    await trans.CommitAsync();
                    return result;
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    result.EnumResult = HttpStatusCode.InternalServerError;
                    result.AddError("حدث خطأ ما يرجى إعلام الدعم الفني بالخطأ التالي:" + e.Message);
                    return result;
                }
            }
        }

        #endregion

        #region Get
        public async Task<(int, string)> CheckUser(UserDto dto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(user => !user.IsDeleted
            && (user.UserName == dto.Username || user.Email == dto.Username));
            if (user is null)
            {
                return (0, "لا يوجد مستخدم بهذا الاسم");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            dto.UserId = user.Id;
            if (result.Succeeded)
            {
                var userRoleStatus = Context.UserRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.Role.Name).FirstOrDefault();
                dto.Role = userRoleStatus == "Admin" ? RoleEnum.Admin
                    : userRoleStatus == "Professor" ? RoleEnum.Professor
                    : userRoleStatus == "Student" ? RoleEnum.Student
                    : 0;
                return (user.Id, "");
            }
            else
                return (0, "اسم المستخدم أو كلمة السر خاطئة");
        }
        public int StudentCount()
        {
            var count = Context.UserRoles.Count(entity => !entity.IsDeleted
                            && entity.Role.Name == RoleEnum.Student.ToString());
            return count;
        }
        public OperationResult<HttpStatusCode, List<StudentDto>> GetStudents()
        {
            var result = new OperationResult<HttpStatusCode, List<StudentDto>>();
            try
            {
                result.Result = Context.Users.Where(entity => !entity.IsDeleted
                    && entity.UserRoles.Any(e => e.Role.Name == RoleEnum.Student.ToString()))
                .Select(e => new StudentDto
                {
                    Id = e.Id,
                    Name = e.UserName,
                    AcademicYear = e.UserSemesters.Select(e => e.Semester.YearId).FirstOrDefault(),
                    SectionId = (int)e.DepartmentId,
                    DepatmentName = Context.Departments.FirstOrDefault(d=>d.Id ==(int) e.DepartmentId).Name,
                }).ToList();

            }
            catch (Exception e)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError(e.Message + "حدث خطأ ما يرجى إعلام الدعم الفني بالخطأ التالي ");
            }
            return result;
        }
        public int CurrentUserId()
        {
            try
            {
                var _httpcontext = _httpContextAccessor.HttpContext;
                if (_httpcontext != null
                    && _httpcontext.User != null
                    && _httpcontext.User.Identity != null
                    && _httpcontext.User.Identity.IsAuthenticated)
                {
                    var userId = _httpcontext.User
                        .FindFirstValue(ClaimTypes.NameIdentifier); // should work here same DI 
                    return userId is null ? 0 : int.Parse(userId);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        public int GetUsernameCount(string username, int userId)
        {
            var count = _userManager.Users.Count(entity => !entity.IsDeleted
                            && entity.UserName == username && entity.Id != userId);
            return count;
        }
        public UserRoleStatusDto GetUserRoleStatus(int userId)
        {
            var dto = Context.Users.Where(entity => !entity.IsDeleted && entity.Id == userId)
                .Select(entity => new UserRoleStatusDto
                {
                    IsAdmin = entity.UserRoles.Any(e => !e.IsDeleted && e.Role.Name == nameof(RoleEnum.Admin)),
                    IsStudent = entity.UserRoles.Any(e => !e.IsDeleted && e.Role.Name == nameof(RoleEnum.Student)),
                    IsProfessor = entity.UserRoles.Any(e => !e.IsDeleted && e.Role.Name == nameof(RoleEnum.Professor)),
                }).FirstOrDefault();
            return dto ?? new UserRoleStatusDto();
        }
        public OperationResult<HttpStatusCode, IEnumerable<RoleDto>> GetRoles()
        {
            var result = new OperationResult<HttpStatusCode, IEnumerable<RoleDto>>();
            try
            {
                var roles = Context.Roles.Where(entity => !entity.IsDeleted).Select(
                    entity => new RoleDto
                    {
                        RoleId = entity.Id,
                        RoleName = entity.Name
                    }).ToList();
                result.EnumResult = HttpStatusCode.OK;
                result.Result = roles;
            }
            catch (Exception)
            {
                result.AddError("something went wrong please contact developer");
                result.EnumResult = HttpStatusCode.InternalServerError;
            }
            return result;
        }
        public OperationResult<HttpStatusCode, List<SubjectDto>> GetUserSubjects(int UserId)
        {
            var result = new OperationResult<HttpStatusCode, List<SubjectDto>>();
            if (UserId == 0)
            {
                result.EnumResult = HttpStatusCode.BadRequest;
                result.AddError("معرف القسم غير صحيح");
                return result;
            }
            try
            {
                var DepartmentId = Context.Users.FirstOrDefault(u => u.Id == UserId).DepartmentId;
                result.Result = Context.Subjects.Where(s => s.DepartmentId == DepartmentId).Select(s => new SubjectDto
                {
                    SemesterId = s.SemesterId,
                    DepartmentId = s.DepartmentId,
                    Description = s.Description,
                    Id = s.Id,
                    Name = s.Name,
                    YearId = s.Semester.YearId
                }).ToList();
                result.EnumResult = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError(e.Message + "حدث خطأ ما يرجى إعلام الدعم الفني بالخطأ التالي");
            }
            return result;
        }
        public double GetAverage(int userId)
        {
            double result = 0;
            try
            {
                if (userId == 0)
                {
                    return 0;
                }
                var count = Context.Exams.Where(e => e.UserId == userId && e.Grade >= 60).Count();
                result = Context.Exams.Where(e => e.UserId == userId && e.Grade >= 60).Sum(e => e.Grade)/count;
            }
            catch (Exception e)
            {
                return 0;
            }
            return result;
        }
        public double GetAnualAverage(int userId,int yearId , bool anual)
        {
            double result = 0;
            try
            {
                if(userId == 0 || yearId == 0)
                {
                    return 0;
                }
                var subjectCount = new List<int>();
                if (anual)
                    subjectCount = Context.Subjects.Where(s => s.Semester.YearId == yearId && s.DepartmentId == Context.Users.FirstOrDefault(u => u.Id == userId).DepartmentId).Select(s=>s.Id).ToList();
                else
                    subjectCount = Context.Subjects.Where(s => s.SemesterId == yearId && s.DepartmentId == Context.Users.FirstOrDefault(u => u.Id == userId).DepartmentId).Select(s => s.Id).ToList();
                var examCount = Context.Exams.Where(e=> subjectCount.Contains(e.SubjectId) && e.UserId==userId && e.Grade>=60).Count();
                result = Context.Exams.Where(e => subjectCount.Contains(e.SubjectId) && e.UserId == userId && e.Grade >= 60).Sum(e=>e.Grade)/examCount;
            }
            catch (Exception e)
            {
                return 0;
            }
            return result;
        }

        #endregion

        #region Remove

        public async Task<OperationResult<HttpStatusCode, bool>> RemoveUser(int userId)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            var currentUserId = CurrentUserId();
            if (userId == currentUserId)
            {
                result.Result = false;
                result.AddError("user cannot delete his own account");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }

            var devEntity = await _userManager.FindByNameAsync("dev");
            if (devEntity != null && userId == devEntity.Id)
            {
                result.Result = false;
                result.AddError("you cannot delete this user");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }
            using (var trans = await Context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userEntity = await Context.Users.FirstOrDefaultAsync(entity => !entity.IsDeleted
                    && entity.Id == userId);
                    if (userEntity == null)
                    {
                        await trans.RollbackAsync();
                        result.Result = false;
                        result.AddError("user does not exist");
                        result.EnumResult = HttpStatusCode.BadRequest;
                        return result;
                    }
                    userEntity.IsDeleted = true;
                    Context.Users.Update(userEntity);
                    await Context.SaveChangesAsync();

                    #region adding remove user operation to transaction

                    var transResult = await _transactionRepository.AddTransactionAsync(new TranactionDto()
                    {
                        Operation = OperationNameEnum.Delete,
                        RecordId = userEntity.Id,
                        Table = TableNameEnum.Users,
                        UserId = currentUserId
                    });

                    if (!transResult)
                    {
                        throw new Exception();
                    }
                    #endregion

                    #region Removing user roles

                    var userRoles = await Context.UserRoles.Where(e => !e.IsDeleted
                    && e.UserId == userId).ToListAsync();
                    if (userRoles == null || userRoles.Count() == 0)
                    {
                        await trans.RollbackAsync();
                        result.Result = false;
                        result.AddError("user does not exist");
                        result.EnumResult = HttpStatusCode.BadRequest;
                        return result;
                    }
                    userRoles.ForEach(e => e.IsDeleted = true);
                    Context.UserRoles.UpdateRange(userRoles);
                    await Context.SaveChangesAsync();
                    #endregion

                    #region adding remove user roles operation to transaction

                    foreach (var item in userRoles)
                    {
                        var transactionResult = await _transactionRepository.AddTransactionAsync(new TranactionDto()
                        {
                            Operation = OperationNameEnum.Delete,
                            RecordId = item.Id,
                            Table = TableNameEnum.UserRoles,
                            UserId = currentUserId
                        });

                        if (!transResult)
                        {
                            throw new Exception();
                        }
                    }
                    #endregion

                    await trans.CommitAsync();
                    result.Result = true;
                    result.EnumResult = HttpStatusCode.OK;
                    return result;
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                    result.Result = false;
                    result.EnumResult = HttpStatusCode.InternalServerError;
                    return result;
                }
            }
        }
        #endregion

        #region Helper methods


        #endregion
    }
}