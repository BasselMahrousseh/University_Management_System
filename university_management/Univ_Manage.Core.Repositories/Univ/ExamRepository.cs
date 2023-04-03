using System.Net;
using Univ_Manage.Core.DTOs.Security.User;
using Univ_Manage.Core.DTOs.Univ;
using Univ_Manage.Core.Interfaces.Security;
using Univ_Manage.Core.Interfaces.Supervision;
using Univ_Manage.Core.Interfaces.Univ;
using Univ_Manage.Infrastructure.SqlServer;
using Univ_Manage.SharedKernal.OperationResults;

namespace Univ_Manage.Core.Repositories.Univ
{
    public class ExamRepository : Univ_ManageRepository, IExamRepository
    {
        #region Properties and constructors
        public ExamRepository(Univ_ManageDBContext context , IUserRepository userrepository
            , ITransactionRepository transactionRepository)
            :base(context , userrepository , transactionRepository)
        {

        }
        #endregion

        #region Set
        public OperationResult<HttpStatusCode, StudentMarks> RegisterMarks(StudentMarks dto)
        {
            var result= new OperationResult<HttpStatusCode, StudentMarks>();
            try
            {
                if (dto.SubjectId == 0)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("Invalid Subject");
                    return result;
                }
                if (dto.StudentsMarks == null)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("No marks to be added to database");
                    return result;
                }
                var schedulingId = Context.Schedulings.OrderBy(s=>s.Id).Last().Id;
                foreach (var exam in dto.StudentsMarks)
                {
                    SetExam(new ExamDto
                    {
                        SubjectId = dto.SubjectId,
                        UserId = exam.StudentId,
                        SchedulingId = schedulingId,
                        Grade=exam.Mark
                    });
                }
                result.EnumResult = HttpStatusCode.OK;
                return result;
            }
            catch(Exception e)
            {
                result.AddError("Internal server error");
                result.EnumResult = HttpStatusCode.InternalServerError;
                return result;
            }
        }
        public OperationResult<HttpStatusCode, ExamDto> SetExam(ExamDto examDto)
        {
            var result = new OperationResult<HttpStatusCode, ExamDto>();
            try
            {
                var examEnitiy = Context.Exams.FirstOrDefault(exam => exam.Id == examDto.Id);
                var operation = OperationNameEnum.Update;
                if (examEnitiy == null)
                {
                    examEnitiy = new ExamSet();
                    operation = OperationNameEnum.Create;
                }

                examEnitiy.SubjectId = examDto.SubjectId;
                examEnitiy.SchedulingId = examDto.SchedulingId;
                examEnitiy.UserId = examDto.UserId;
                examEnitiy.Grade = examDto.Grade;
                if (operation == OperationNameEnum.Update)
                {
                    Context.Exams.Update(examEnitiy);
                }
                else
                {
                    Context.Exams.Add(examEnitiy);
                }
                Context.SaveChanges();

                examDto.Id = examEnitiy.Id;
                result.EnumResult = HttpStatusCode.OK;
                result.Result = examDto;
                return result;
            }
            catch (Exception)
            {
                result.AddError("Error while adding the new Exam");
                result.EnumResult = HttpStatusCode.InternalServerError;
                return result;
            }

        }
        #endregion

        #region Get
        #region Get by Id

        public OperationResult<HttpStatusCode, ExamDto> GetExamById(int Id)
        {
            var result = new OperationResult<HttpStatusCode, ExamDto>();
            try
            {
                if (Id == 0)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("Exam doesn't exist or has been deleted");
                }
                else
                {
                    var Exam = Context.Exams.Where(e => !e.IsDeleted
                    && e.Id == Id).Select(e => new ExamDto
                    {
                        Id = e.Id,
                        SchedulingId = e.SchedulingId,
                        SubjectId = e.SubjectId,
                        UserId = e.UserId
                    }).FirstOrDefault();

                    if (Exam == null)
                    {
                        result.EnumResult = HttpStatusCode.BadRequest;
                        result.AddError("Exam doesn't exist or has been deleted");
                    }
                    else
                    {
                        result.Result = Exam;
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

        public OperationResult<HttpStatusCode, List<ExamDto>> GetExamByStudentId(int UserId)
        {
            var result = new OperationResult<HttpStatusCode, List<ExamDto>>();
            try
            {
                if (UserId == 0)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("Exam doesn't exist or has been deleted");
                }
                else
                {
                    var Exam = Context.Exams.Where(e => !e.IsDeleted
                    && e.UserId == UserId).Select(e => new ExamDto
                    {
                        Id = e.Id,
                        SchedulingId = e.SchedulingId,
                        SchedulingName = Context.Schedulings.FirstOrDefault(s=>s.Id ==e.SchedulingId).Name,
                        SubjectId = e.SubjectId,
                        SubjectName = Context.Subjects.FirstOrDefault(s => s.Id == e.SubjectId).Name,
                        UserId = e.UserId,
                        Grade = e.Grade
                    }).ToList();

                    if (Exam == null)
                    {
                        result.EnumResult = HttpStatusCode.BadRequest;
                        result.AddError("الطالب لم يقدم أي امتحانات بعد");
                    }
                    else
                    {
                        result.Result = Exam;
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

        public OperationResult<HttpStatusCode, ExamDto> GetExamByStudentAdminId(int AdminId)
        {
            var result = new OperationResult<HttpStatusCode, ExamDto>();
            try
            {
                if (AdminId == 0)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("Exam doesn't exist or has been deleted");
                }
                else
                {
                    var Exam = Context.Exams.Where(e => !e.IsDeleted
                    ).Select(e => new ExamDto
                    {
                        Id = e.Id,
                        SchedulingId = e.SchedulingId,
                        SubjectId = e.SubjectId,
                        UserId = e.UserId,
                        Grade = e.Grade
                    }).FirstOrDefault();

                    if (Exam == null)
                    {
                        result.EnumResult = HttpStatusCode.BadRequest;
                        result.AddError("Exam doesn't exist or has been deleted");
                    }
                    else
                    {
                        result.Result = Exam;
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

        public OperationResult<HttpStatusCode,List<ExamDto>> GetStudentLastExams(int studentId)
        {
            var result = new OperationResult<HttpStatusCode, List<ExamDto>>();
            if(studentId == 0)
            {
                result.EnumResult=HttpStatusCode.BadRequest;
                result.AddError("Invalid student ID");
                return result;
            }
            try
            {
                var schedulingId = Context.Schedulings.Last().Id;
                result.Result = Context.Exams.Where(e => e.SchedulingId == schedulingId && !e.IsDeleted && e.UserId == studentId).Select(e => new ExamDto
                {
                    SubjectId = e.SubjectId,
                    Grade = e.Grade,
                    SchedulingId = schedulingId,
                    Id = e.Id,
                    UserId = studentId
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

        public OperationResult<HttpStatusCode, ExamDto> GetStudentExamBySubject(int studentId , int subjectId)
        {
            var result = new OperationResult<HttpStatusCode, ExamDto>();
            if (studentId == 0 || subjectId==0)
            {
                result.EnumResult = HttpStatusCode.BadRequest;
                result.AddError("معلومات خاطئة");
                return result;
            }
            try
            {
                result.Result = Context.Exams.Where(e => !e.IsDeleted && e.UserId == studentId && e.SubjectId == subjectId).OrderBy(e => e.Id).Select(e => new ExamDto
                {
                    SubjectId = e.SubjectId,
                    Grade = e.Grade,
                    SchedulingId = e.SchedulingId,
                    Id = e.Id,
                    UserId = studentId
                }).Last();
                result.EnumResult = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Something went wrong..! Please contact developer");
            }
            return result;
        }
        public OperationResult<HttpStatusCode,List<StudentDto>> GetValidStudents(int subjectId)
        {
            var result = new OperationResult<HttpStatusCode, List<StudentDto>>();
            try
            {
                if(subjectId == 0)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("معرف المادة غير مقبول");
                    return result;
                }
                var yearId = Context.Subjects.Where(s => s.Id == subjectId).Select(s => s.Semester.YearId).FirstOrDefault();
                var departmentId = Context.Subjects.FirstOrDefault(a => a.Id == subjectId).DepartmentId;
                var schedulingId = Context.Schedulings.OrderBy(s => s.Id).Last().Id;
                var registeredStudents = Context.Exams.Where(e => e.SchedulingId == schedulingId && e.SubjectId == subjectId).Select(e => e.UserId).ToList();
                result.Result = Context.Users.Where(s => s.UserSemesters.FirstOrDefault(us=>us.UserId==s.Id).Semester.YearId==yearId && s.DepartmentId == departmentId && !registeredStudents.Contains(s.Id)).Select(s => new StudentDto()
                {
                    SectionId = departmentId,
                    AcademicYear = Context.Subjects.Where(sb => sb.Id == subjectId).Select(sb => sb.Semester.YearId).FirstOrDefault(),
                    Id = s.Id,
                    Name = s.UserName
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
        public string GetSchedulingName(int SchedulingId)
        {
            var result = Context.Schedulings.FirstOrDefault(s => s.Id == SchedulingId).Name;
            return result;
        }
        public OperationResult<HttpStatusCode,List<ExamDto>> GetSubjectExamResults(int SubjectId,int SchedulingId)
        {
            var result = new OperationResult<HttpStatusCode,List<ExamDto>>();
            try
            {
                if(SubjectId == 0)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
                if (SchedulingId == 0)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
                result.Result = Context.Exams.Where(e => e.SubjectId == SubjectId && e.SchedulingId == SchedulingId).Select(e => new ExamDto()
                {
                    Grade = e.Grade,
                    UserName = e.User.UserName
                }).ToList();
                result.EnumResult = HttpStatusCode.OK;
            }
            catch (Exception e)
            {

                
            }
            return result;
        }
        public OperationResult<HttpStatusCode,List<ExamDetailDto>> GetSchedulingDetails(int SchedulingId)
        {
            var result = new OperationResult<HttpStatusCode,List<ExamDetailDto>>();
            result.Result = new List<ExamDetailDto>();
            try
            {
                if(SchedulingId == 0)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("معرف دورة امتحانية غير صحيح");
                    return result;
                }
                foreach (var item in Context.Subjects.ToList())
                {
                    var examDetail = new ExamDetailDto();
                    examDetail.SubjectName = item.Name;
                    examDetail.SubjectId = item.Id;
                    examDetail.DepartmentName = Context.Departments.FirstOrDefault(d => d.Id == item.DepartmentId).Name;
                    examDetail.NumberOfParticipants = Context.Exams.Where(e=>e.SchedulingId == SchedulingId && e.SubjectId ==item.Id).Count();
                    examDetail.SuccsessRatio = (double)Context.Exams.Where(e => e.SchedulingId == SchedulingId && e.SubjectId == item.Id && e.Grade>=60).Count()/ examDetail.NumberOfParticipants;
                    examDetail.SuccsessRatio = examDetail.SuccsessRatio * 100;
                    result.Result.Add(examDetail);
                }
                result.EnumResult = HttpStatusCode.OK;
                
            }
            catch (Exception e)
            {
                result.AddError("حصل خطأ ما ");
                result.EnumResult = HttpStatusCode.InternalServerError;
            }
            return result;
        }
        #endregion

        #region Remove

        #endregion

    }
}
