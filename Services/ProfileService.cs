using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Identity.Api.Contracts.V1.Requests;
using Identity.Api.Contracts.V1.Responses;
using Identity.Api.Data;
using Identity.Api.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;

namespace Identity.Api.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ProfileService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
          
        }

        public async Task<bool> deleteUser(string Id)
        {
          var result =  await _userManager.DeleteAsync(await _userManager.FindByIdAsync(Id));
            return result.Succeeded;
        }

        public async Task<IEnumerable<IViewModel>> getAll(string userType)
        {
            var users = await _userManager.GetUsersInRoleAsync(userType);
            var userList = new List<IViewModel>();
            if (userType == "Student")
            {
                foreach (var user in users)
                {
                    var roles = new List<string>();
                    foreach (var role in await _userManager.GetRolesAsync(user))
                    {
                        roles.Add(role);
                    }
                    userList.Add(new UserViewModel
                    {
                        Id = user.Id,
                        Department = user.Department,
                        Email = user.Email,
                        PicPath = user.PicPath,
                        StudyYear = user.StudyYear,
                        Username = user.UserName,
                        Roles = roles
                    });
                }
            }
            else
            {
 
                foreach (var user in users)
                {
                    var roles = new List<string>();
                    foreach (var role in await _userManager.GetRolesAsync(user))
                    {
                        roles.Add(role);
                    }
                    userList.Add(new DoctorViewModel
                    {
                        Id = user.Id,
                        Department = user.Department,
                        Email = user.Email,
                        PicPath = user.PicPath,
                        Username = user.UserName,
                        Roles = roles

                    });

                }
            }
    
            return userList;

        }

        public async Task<IViewModel> getUserById(string Id , string role)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (role =="Student")
            {
                return new UserViewModel
                {
                    Id = user.Id, Department = user.Department ,
                    Email = user.Email 
                    , PicPath = user.PicPath,StudyYear=user.StudyYear, Username= user.UserName
                };
            }
            return new DoctorViewModel
            {
                Id = user.Id,
                Department = user.Department,
                Email = user.Email,
                PicPath = user.PicPath,
                Username = user.UserName
            };

            
        }

        public async Task<string> updateImage(string UId , IFormFile file)
        {
            var user = await _userManager.FindByIdAsync(UId);
            string m = user.Id + user.UserName;
            if (file.Length >0)
            {
                try
                {
                    if (!Directory.Exists(_hostingEnvironment.ContentRootPath + "\\wwwroot\\" + "Images" + "\\" + m + "\\"))
                    {
                        Directory.CreateDirectory(_hostingEnvironment.ContentRootPath + "\\wwwroot\\" + "Images" + "\\" + m + "\\");
                    }
                    string guid = Guid.NewGuid().ToString();
                    using (FileStream fileStream = File.Create(_hostingEnvironment.ContentRootPath + "\\wwwroot\\" + "Images" +
                        "\\" + m + "\\" + guid + file.FileName.Replace("\\", "s").Replace(":", "s")))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        user.PicPath = "https://localhost:6001" + "\\" + "Images" + "\\" + m + "\\" + guid + file.FileName.Replace("\\", "s").Replace(":"
                            , "s");
                        var result = await _userManager.UpdateAsync(user);
                        return result.Succeeded.ToString();
                    }
                }
                catch (Exception ex)
                {

                    return ex.ToString();
                }

            }

            return "Unsuccessful" ;
        }

        public async Task<bool> updatePassword(string Id, string OldPassword, string NewPassword)
        {
            var result = await _userManager.ChangePasswordAsync(await _userManager.FindByIdAsync(Id), OldPassword, NewPassword);
            return result.Succeeded;
        }

        public async Task<bool> updateUser(string Id, UpdateUserViewModel newUser)
        {
            var user = await _userManager.FindByIdAsync(Id);
            var result = false;
            if (user !=null)
            {
                 user.Email = newUser.Email;
                 user.Department= newUser.Department;
                if (newUser.StudyYear !="")
                {
                    user.StudyYear = newUser.StudyYear;
                }
              
                user.UserName=   newUser.Username;

                var userSuccess = await _userManager.UpdateAsync(user);
                result = userSuccess.Succeeded;
            }
            return result;


        }

        public async Task<bool> RegisterUser( UserViewModel registerStudent)
        {
            var applicationUser = new ApplicationUser
            {
                PicPath = registerStudent.PicPath,
                Email = registerStudent.Email,
                UserName = registerStudent.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
                Department = registerStudent.Department,
                StudyYear = registerStudent.StudyYear.ToString()

            };

            var result = await _userManager.CreateAsync(applicationUser, registerStudent.Username+"@123");

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(applicationUser, registerStudent.Roles);

                return true;
            }

            return false;
        }


    }
}
