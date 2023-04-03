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
    public class SubjectRepository: Univ_ManageRepository, ISubjectRepository
    {
        #region Properties and constructors
        
        public SubjectRepository(Univ_ManageDBContext context, IUserRepository userrepository
            , ITransactionRepository transactionRepository)
            : base(context, userrepository, transactionRepository)
        {

        }
        #endregion

        #region Set
        public OperationResult<HttpStatusCode, SubjectDto> SetSubject(SubjectDto subjectDto)
        {
            var result = new OperationResult<HttpStatusCode, SubjectDto>();
            try
            {
                var subjectEnitiy = Context.Subjects.FirstOrDefault(subject => subject.Id == subjectDto.Id);
                var operation = OperationNameEnum.Update;
                if (subjectEnitiy == null)
                {
                    subjectEnitiy = new SubjectSet();
                    operation = OperationNameEnum.Create;
                }

                subjectEnitiy.Name = subjectDto.Name;
                subjectEnitiy.DepartmentId = subjectDto.DepartmentId;
                subjectEnitiy.SemesterId = subjectDto.SemesterId;

                if (operation == OperationNameEnum.Update)
                {
                    Context.Subjects.Update(subjectEnitiy);
                }
                else
                {
                    Context.Subjects.Add(subjectEnitiy);
                }
                Context.SaveChanges();

                subjectDto.Id = subjectEnitiy.Id;
                result.EnumResult = HttpStatusCode.OK;
                result.Result = subjectDto;
                return result;
            }
            catch (Exception)
            {
                result.AddError("Error while adding the new Subject");
                result.EnumResult = HttpStatusCode.InternalServerError;
                return result;
            }

        }
        #endregion

        public OperationResult<HttpStatusCode, List<SubjectDto>> GetAllSubjects()
        {
            var result = new OperationResult<HttpStatusCode, List<SubjectDto>>();
            try
            {
                result.Result = Context.Subjects.Select(s => new SubjectDto
                {
                    DepartmentId = s.DepartmentId,
                    SemesterId = s.SemesterId,
                    Id = s.Id,
                    Description = s.Description,
                    Name = s.Name,
                    YearId = s.Semester.YearId
                }).ToList();
                result.EnumResult = HttpStatusCode.OK;
            }
            catch(Exception e)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Something went wrong..! Please contact developer and informhim with the following error " + e.Message);
            }
            return result;
        }
        public OperationResult<HttpStatusCode, List<SubjectDto>> GetSemesterSubjects(int SemesterId)
        {
            var result = new OperationResult<HttpStatusCode, List<SubjectDto>>();
            if (SemesterId == 0)
            {
                result.EnumResult = HttpStatusCode.BadRequest;
                result.AddError("Invalid semester ID");
                return result;
            }
            try
            {
                result.Result = Context.Subjects.Where(s => s.SemesterId == SemesterId).Select(s => new SubjectDto
                {
                    SemesterId=s.SemesterId,
                    DepartmentId = s.DepartmentId,
                    Description = s.Description,
                    Id = s.Id,
                    Name = s.Name,
                    YearId = s.Semester.YearId
                }).ToList();
                result.EnumResult = HttpStatusCode.OK;
            }
            catch(Exception e)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Something went wrong..! Please contact developer");
            }
            return result;
        }
        public OperationResult<HttpStatusCode, List<SubjectDto>> GetDepartmentSubjects(int DepartmentId)
        {
            var result = new OperationResult<HttpStatusCode, List<SubjectDto>>();
            if (DepartmentId == 0)
            {
                result.EnumResult = HttpStatusCode.BadRequest;
                result.AddError("معرف القسم غير صحيح");
                return result;
            }
            try
            {
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
                result.AddError(e.Message+"حدث خطأ ما يرجى إعلام الدعم الفني بالخطأ التالي");
            }
            return result;
        }
    }
}
