using Microsoft.AspNetCore.Mvc;
using Univ_Manage.Core.DTOs;
using Univ_Manage.Core.DTOs.Security.User;
using Univ_Manage.Core.DTOs.Univ;
using Univ_Manage.Core.Interfaces.Security;
using Univ_Manage.Core.Interfaces.Univ;
using Univ_Manage.SharedKernal.Enums;
using Univ_Manage.ViewModels;

namespace Univ_Manage.Controllers
{
    public class UserController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IExamRepository _examRepository;

        public UserController(IDepartmentRepository departmentRepository,IUserRepository userRepository
            , ISemesterRepository semesterRepository, IExamRepository examRepository)
        {
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
            _semesterRepository = semesterRepository;
            _examRepository = examRepository;
        }
        public ActionResult LogIn()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int element , int type, int userId)
        {
            double data;
            if(type ==1)
            {
                data = _userRepository.GetAverage(userId);
            }
            else if(type == 2)
            {
                data = _userRepository.GetAnualAverage(userId , element, true);
            }
            else if(type == 3)
            {
                data = _userRepository.GetAnualAverage(userId, element, false);
            }
            else
            {
                data = _examRepository.GetStudentExamBySubject(userId, element).Result.Grade;
            }
            return Json(new {value = data});
        }

        // GET: User/Create
        public ActionResult SignUp()
        {
            var vm = new SignUpViewModel();
            vm.Years = _departmentRepository.GetYears().Result;
            vm.Departments = _departmentRepository.GetDepartments().Result;
            return View(vm);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(UserFormDto dto)
        {
            try
            {
                var user=await _userRepository.SetUser(new UserDto()
                {
                    DepartmentId = dto.Department,
                    Password = dto.Password,
                    PasswordConfirmation = dto.PasswordConfirmation,
                    Role = RoleEnum.Student,
                    Username = dto.Name.Replace(" ",""),
                    FirstName = dto.Name.Split(" ")[0],
                    LastName = dto.Name.Split(" ")[1],
                    YearId = dto.Year,
                    Email = dto.Email,
                });
                return RedirectToAction("DashboardPage","Home",new {userId = user.Result.UserId});
            }
            catch
            {
                return View("SignUp");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateStudent(UserFormDto dto)
        {
            try
            {
                var user = await _userRepository.SetUser(new UserDto()
                {
                    DepartmentId = dto.Department,
                    Password = dto.Password,
                    PasswordConfirmation = dto.PasswordConfirmation,
                    Role = RoleEnum.Student,
                    Username = dto.Name.Replace(" ", ""),
                    FirstName = dto.Name.Split(" ")[0],
                    LastName = dto.Name.Split(" ")[1],
                    YearId = dto.Year,
                    Email = dto.Email,
                });
                return RedirectToAction("StudentsAccounts");
            }
            catch
            {
                return RedirectToAction("StudentsAccounts");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(UserFormDto dto)
        {
            try
            {
                var userDto= new UserDto();
                userDto.Email = dto.Email;
                userDto.Password = dto.Password;
                userDto.Username = dto.Name.Replace(" ","");
                var user = _userRepository.CheckUser(userDto);
                if (user.Result.Item1==0)
                {
                    var vm = new ErrorViewModel();
                    vm.ErrorMessage = user.Result.Item2;
                    return View("Error", vm);
                }
                return RedirectToAction("DashboardPage", "Home" ,new { userId= user.Result.Item1});
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Error(string message)
        {
            var vm = new ErrorViewModel();
            vm.ErrorMessage = message;
            return View(vm);
        }
        public ActionResult MyCourses(int userId)
        {
            var vm = new ReportingViewModel();
            vm.UserId = userId;
            vm.Years = _departmentRepository.GetYears().Result;
            vm.Semesters = _semesterRepository.GetAllSemesters().Result;
            vm.Subjects = _userRepository.GetUserSubjects(userId).Result;
            return View(vm);
        }
        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult StudentsAccounts()
        {
            var vm = new PageViewModel<StudentDto>();
            vm.Records = _userRepository.GetStudents().Result;
            return View(vm);
        }
        public ActionResult StudentDetail(int userId)
        {
            var vm = new PageViewModel<ExamDto>();
            vm.Records = _examRepository.GetExamByStudentId(userId).Result;
            vm.UserId = userId;
            return View(vm);
        }
        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
