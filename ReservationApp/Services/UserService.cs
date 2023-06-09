﻿using Microsoft.AspNetCore.Identity;
using ReservationApp.Data;
using ReservationApp.Models;
using System.Security.Claims;
using System.Web;

namespace ReservationApp.Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public UserService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;

        }        

        // Register to the system
        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "User already exist";
                return status;
            }

            ApplicationUser user = new ApplicationUser()
            {
                PhoneNumber = model.MobilePhone,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Image = model.Image,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User creation failed";
                return status;
            }

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));


            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "You have registered successfully";
            return status;
        }

        // Login to the system
        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid username";
                return status;
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid Password";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Logged in successfully";


            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User is locked out";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on logging in";
            }

            return status;
        }

        //Logout
        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();

        }

        //Change Password
        public async Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username)
        {
            var status = new Status();

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "User does not exist";
                status.StatusCode = 0;
                return status;
            }
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "Password has updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Some error occcured";
                status.StatusCode = 0;
            }
            return status;

        }

        //Get all the users
        public async Task<List<Users_in_Role_ViewModel>> GetAllUsers(string roleSelect)
        {
                        
            var users = userManager.Users.ToList();
            List<Users_in_Role_ViewModel> result = new List<Users_in_Role_ViewModel>();
            foreach(var user in users)
            {
                Users_in_Role_ViewModel model = new Users_in_Role_ViewModel();
                model.UserId = user.Id;
                model.Email = user.Email;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.MobilePhone = user.PhoneNumber;
                model.UserName = user.UserName;
                model.Image = user.Image;
                var roles = userManager.GetRolesAsync(user);
                model.Role = roles.Result[0];
                if(roleSelect == "member")
                {
                    if(model.Role == "member")
                    {
                        result.Add(model);
                    }
                }
                else
                {
                    if(model.Role == "admin" || model.Role == "staff")
                    {
                        result.Add(model);
                    }
                }
                
            }
            return result;
        }

        //get single user by user id
        public async Task<RegistrationModel> GetSingerUser(string userID)
        {
            var user = userManager.Users.First(m => m.Id == userID);
            RegistrationModel model = new RegistrationModel();
            model.RegistrationID = user.Id;
            model.Email = user.Email;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.MobilePhone = user.PhoneNumber;
            model.UserName = user.UserName;
            var roles = userManager.GetRolesAsync(user);
            model.Role = roles.Result[0];
            model.Password =user.PasswordHash;
            model.PasswordConfirm = user.PasswordHash;
            return model;
        }

    }
}
