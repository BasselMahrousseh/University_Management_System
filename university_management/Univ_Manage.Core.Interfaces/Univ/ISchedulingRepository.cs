using Univ_Manage.Core.DTOs.Univ;
using Univ_Manage.SharedKernal.OperationResults;

namespace Univ_Manage.Core.Interfaces.Univ
{
    public interface ISchedulingRepository
    {
        public OperationResult<HttpStatusCode, SchedulingDto> GetLastScheduling();
        public OperationResult<HttpStatusCode, SchedulingDto> GetSchedulingById(int Id);
        public OperationResult<HttpStatusCode, SchedulingDto> SetScheduling(SchedulingDto schedulingDto);
        public OperationResult<HttpStatusCode, SchedulingDto> GetSchedulingByDate(DateTime startDate, DateTime endDate);
        public OperationResult<HttpStatusCode, List<SchedulingDto>> GetAllSchedulings();
        public OperationResult<HttpStatusCode, List<SchedulingDto>> GetLast4Schedulings();
    }
}
