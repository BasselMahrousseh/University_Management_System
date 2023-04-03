using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Univ_Manage.Core.Interfaces.Security;
using Univ_Manage.Core.Interfaces.Univ;
using Univ_Manage.Infrastructure.Models.Univ;
using Univ_Manage.SharedKernal.Enums;
using Univ_Manage.ViewModels;

namespace Univ_Manage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISchedulingRepository _schedulingRepository;

        public HomeController(ILogger<HomeController> logger ,IUserRepository userRepository,
            ISemesterRepository semesterRepository , IDepartmentRepository departmentRepository,
            ISchedulingRepository schedulingRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _semesterRepository = semesterRepository;
            _departmentRepository = departmentRepository;
            _schedulingRepository = schedulingRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdminDashboard()
        {
            var years = _departmentRepository.GetYears().Result;
            var vm = new AdminDashboardViewModel();
            vm.FirstYearCount = _semesterRepository.GetUserCountInSpecificYear(years.FirstOrDefault(y => y.Name == Enum.GetName(typeof(YearEnum), YearEnum.السنة_الأولى)).Id);
            vm.SecondYearCount = _semesterRepository.GetUserCountInSpecificYear(years.FirstOrDefault(y => y.Name == Enum.GetName(typeof(YearEnum), YearEnum.السنة_الثانية)).Id);
            vm.ThirdYearCount = _semesterRepository.GetUserCountInSpecificYear(years.FirstOrDefault(y => y.Name == Enum.GetName(typeof(YearEnum), YearEnum.السنة_الثالثة)).Id);
            vm.FourthYearCount = _semesterRepository.GetUserCountInSpecificYear(years.FirstOrDefault(y => y.Name == Enum.GetName(typeof(YearEnum), YearEnum.السنة_الرابعة)).Id);
            vm.Last4Exams = _schedulingRepository.GetLast4Schedulings().Result;
            return View(vm);
        }
        public IActionResult DashboardPage(int userId)
        {
            var useroles = _userRepository.GetUserRoleStatus(userId);
            var vm = new AdminDashboardViewModel();
            
            vm.UserId = userId;
            if (useroles.IsAdmin)
            {
                vm.IsAdmin = true;
                return RedirectToAction("AdminDashboard");
            }
            else if (useroles.IsStudent)
            {
                vm.IsStudent = true;
                return View("StudentDashboard",vm);
            }
            else
            {
                vm.IsProfessor = true;
                return RedirectToAction("AdminDashboard");
            }
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}