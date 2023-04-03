using Microsoft.AspNetCore.Mvc;
using Univ_Manage.Core.DTOs.Security.User;
using Univ_Manage.Core.DTOs.Univ;
using Univ_Manage.Core.Interfaces.Security;
using Univ_Manage.Core.Interfaces.Univ;
using Univ_Manage.ViewModels;

namespace Univ_Manage.Controllers
{
    public class ExamController : Controller
    {
        private readonly IExamRepository _examRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISchedulingRepository _schedulingRepository;
        public ExamController(IExamRepository examRepository, ISubjectRepository subjectRepository,IUserRepository userRepository
            ,IDepartmentRepository departmentRepository,ISchedulingRepository schedulingRepository)
        {
            _examRepository = examRepository;
            _subjectRepository = subjectRepository;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _schedulingRepository = schedulingRepository;
        }

        public ActionResult Index()
        {
            var vm = new ExamsViewModel();
            vm.Students = _userRepository.GetStudents().Result;
            vm.Subjects = _subjectRepository.GetAllSubjects().Result;
            vm.Years = _departmentRepository.GetYears().Result;
            vm.Departments = _departmentRepository.GetDepartments().Result;
            return View(vm);
        }
        public IActionResult AddNewScheduling(SchedulingDto dto)
        {
            var result = _schedulingRepository.SetScheduling(dto);
            return Ok(result);
        }
        
        public ActionResult SchedulingsPage()
        {
            var vm = new PageViewModel<SchedulingDto>();
            vm.Records = _schedulingRepository.GetAllSchedulings().Result;
            return View(vm);
        }

        // GET: ExamController/Create
        public ActionResult MyExams(int StudentId)
        {
            var vm = new PageViewModel<ExamDto>();
            vm.Records = _examRepository.GetExamByStudentId(StudentId).Result;
            vm.UserId = StudentId;
            return View(vm);
        }
        public ActionResult GetValidStudents(int SubjectId)
        {
            var vm = new PageViewModel<StudentDto>();
            vm.Records = _examRepository.GetValidStudents(SubjectId).Result;
            return Ok(vm);
        }
        // POST: ExamController/Create
        [HttpPost]
        public ActionResult StudentMarks(StudentMarks dto)
        {          
            var result = _examRepository.RegisterMarks(dto);
            return Ok(result);
        }
        public ActionResult Detail(int SubjectId, int SchedulingId)
        {
            var vm = new PageViewModel<ExamDto>();
            vm.Records = _examRepository.GetSubjectExamResults(SubjectId,SchedulingId).Result;
            vm.Name =  _subjectRepository.GetAllSubjects().Result.FirstOrDefault(s=>s.Id==SubjectId).Name + _examRepository.GetSchedulingName(SchedulingId) ;
            return View(vm);
        }
        public ActionResult SchedulingDetail(int SchedulingId)
        {
            var vm = new SchedulingDetailViewModel();
            vm.Name = _examRepository.GetSchedulingName(SchedulingId);
            vm.Id = SchedulingId;
            vm.Exams = _examRepository.GetSchedulingDetails(SchedulingId).Result;
            return View(vm);
        }
    }
}
