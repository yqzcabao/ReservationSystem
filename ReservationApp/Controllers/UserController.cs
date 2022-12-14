using Microsoft.AspNetCore.Mvc;
using ReservationApp.Services;
using ReservationApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ReservationApp.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserService _authService;

        public UserController(IUserService authService)
        {
            _authService = authService;
        }

        public IActionResult MemberRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MemberRegistration(RegistrationModel model)
        {
            if (!ModelState.IsValid) { return View(model); }
            model.Role = "member";
            var result = await _authService.RegisterAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(MemberRegistration));
        }

        //CreateAdmin is for create a sytem admin, admin can create staff account
        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(RegistrationModel model)
        {
            if (!ModelState.IsValid) { return View(model); }
            model.Role = "admin";
            var result = await _authService.RegisterAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(CreateAdmin));
        }

        //CreateStaff is for create a normal staff account which can capture an order or view all the orders.
        public IActionResult CreateStaff()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaff(RegistrationModel model)
        {
            if (!ModelState.IsValid) { return View(model); }
            model.Role = "staff";
            var result = await _authService.RegisterAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(CreateStaff));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authService.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }

        //Logout
        //[Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        //Change Password
        public IActionResult ChangePassword()
        {
            return View();
        }

        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authService.ChangePasswordAsync(model, User.Identity.Name);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(ChangePassword));
        }

        //To get all the member
        public async Task<IActionResult> GetAllMember()
        {
            string roleSelect = "member";
            List<Users_in_Role_ViewModel> result = await _authService.GetAllUsers(roleSelect);
            return View(result);
        }

        //To get all the staff and admin
        public async Task<IActionResult> GetAllStaff()
        {
            string roleSelect = "nonmember";
            List<Users_in_Role_ViewModel> result = await _authService.GetAllUsers(roleSelect);
            return View(result);
        }

        //Edit a user infomation
        //Not finished yet
        public IActionResult Edit(string id)
        {
            var singleUser = _authService.GetSingerUser(id).Result;
            return View(singleUser);
        }


    }
}
