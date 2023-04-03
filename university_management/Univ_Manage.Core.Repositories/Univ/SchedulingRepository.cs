using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Univ_Manage.Core.DTOs.Univ;
using Univ_Manage.Core.Interfaces.Security;
using Univ_Manage.Core.Interfaces.Supervision;
using Univ_Manage.Core.Interfaces.Univ;
using Univ_Manage.Infrastructure.Models.Univ;
using Univ_Manage.Infrastructure.SqlServer;
using Univ_Manage.SharedKernal.OperationResults;

namespace Univ_Manage.Core.Repositories.Univ
{
    public class SchedulingRepository : Univ_ManageRepository , ISchedulingRepository
    {
        #region Properties and constructors

        public SchedulingRepository(Univ_ManageDBContext context, IUserRepository userrepository
            , ITransactionRepository transactionRepository)
            : base(context, userrepository, transactionRepository)
        {


        }
        #endregion

        #region Set
        public OperationResult<HttpStatusCode,SchedulingDto> SetScheduling(SchedulingDto schedulingDto)
        {
            var result = new OperationResult<HttpStatusCode, SchedulingDto>();
            try
            {
                var schedulingEnitiy = Context.Schedulings.FirstOrDefault(scheduling => scheduling.Id == schedulingDto.Id);
                var operation = OperationNameEnum.Update;
                if (schedulingEnitiy == null)
                {
                    schedulingEnitiy = new SchedulingSet();
                    operation = OperationNameEnum.Create;
                }

                schedulingEnitiy.Name = schedulingDto.Name;
                schedulingEnitiy.StartDate = schedulingDto.StartDate;
                schedulingEnitiy.EndDate = schedulingDto.EndDate;
                if (operation == OperationNameEnum.Update)
                {
                    Context.Schedulings.Update(schedulingEnitiy);
                }
                else
                {
                    Context.Schedulings.Add(schedulingEnitiy);
                }
                Context.SaveChanges();

                schedulingDto.Id = schedulingEnitiy.Id;
                result.EnumResult = HttpStatusCode.OK;
                result.Result = schedulingDto;
                return result;
            }
            catch (Exception)
            {
                result.AddError("Error while adding the new Scheduling");
                result.EnumResult = HttpStatusCode.InternalServerError;
                return result;
            }

        }
        #endregion

        #region Get

        public OperationResult<HttpStatusCode, SchedulingDto> GetSchedulingById(int Id)
        {
            var result = new OperationResult<HttpStatusCode, SchedulingDto>();
            try
            {
                if (Id == 0)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("Scheduling doesn't exist or has been deleted");
                }
                else
                {
                    var Scheduling = Context.Schedulings.Where(e => !e.IsDeleted
                    && e.Id == Id).Select(e => new SchedulingDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                    }).FirstOrDefault();

                    if (Scheduling == null)
                    {
                        result.EnumResult = HttpStatusCode.BadRequest;
                        result.AddError("Scheduling doesn't exist or has been deleted");
                    }
                    else
                    {
                        result.Result = Scheduling;
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
        public OperationResult<HttpStatusCode, SchedulingDto> GetLastScheduling()
        {
            var result = new OperationResult<HttpStatusCode, SchedulingDto>();
            try
            {
                result.Result = Context.Schedulings.Select(s => new SchedulingDto
                {
                    EndDate = s.EndDate,
                    StartDate = s.StartDate,
                    Id = s.Id,
                    Name = s.Name,
                }).Last();
                result.EnumResult= HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Something went wrong..! Please contact the developer");
            }
            return result;
        }
        public OperationResult<HttpStatusCode, SchedulingDto> GetSchedulingByDate(DateTime startDate, DateTime endDate)
        {
            var result = new OperationResult<HttpStatusCode, SchedulingDto>();
            try
            {
                if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
                {
                    result.EnumResult = HttpStatusCode.BadRequest;
                    result.AddError("Scheduling doesn't exist or has been deleted");
                }
                else
                {
                    var Scheduling = Context.Schedulings.Where(e => !e.IsDeleted
                    && e.StartDate>=startDate ).Select(e => new SchedulingDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                    }).FirstOrDefault();

                    if (Scheduling == null)
                    {
                        result.EnumResult = HttpStatusCode.BadRequest;
                        result.AddError("Scheduling doesn't exist or has been deleted");
                    }
                    else
                    {
                        result.Result = Scheduling;
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
        
        public OperationResult<HttpStatusCode , List<SchedulingDto>> GetAllSchedulings()
        {
            var result = new OperationResult<HttpStatusCode, List<SchedulingDto>>();
            try
            {
                result.Result = Context.Schedulings.Select(s => new SchedulingDto()
                {
                    Name = s.Name,
                    Id = s.Id,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate
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
        public OperationResult<HttpStatusCode, List<SchedulingDto>> GetLast4Schedulings()
        {
            var result = new OperationResult<HttpStatusCode, List<SchedulingDto>>();
            try
            {
                result.Result = Context.Schedulings.OrderByDescending(s=>s.Id).Take(4).Select(s => new SchedulingDto()
                {
                    Name = s.Name,
                    Id = s.Id,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate
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
        #endregion

        #region Remove

        #endregion


    }
}
