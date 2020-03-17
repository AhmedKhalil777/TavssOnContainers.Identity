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
                    userList.Add(new UserViewModel
                    {
                        Id =user.Id,
                        Department = user.Department,
                        Email = user.Email,
                        PicPath = user.PicPath,
                        StudyYear = user.StudyYear,
                        Username = user.UserName

                    });
                }
            }
            else
            {
                foreach (var user in users)
                {
                    userList.Add(new DoctorViewModel
                    {
                        Id = user.Id,
                        Department = user.Department,
                        Email = user.Email,
                        PicPath = user.PicPath,
                        Username = user.UserName

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

        public async Task<string> updateImage(string Id , ImageViewModel file)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (file.Picture.Length >0)
            {
                try
                {
                    
                    if (!Directory.Exists(_hostingEnvironment.WebRootPath+"\\uploads\\"))
                    {
                        Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "\\uploads\\");
                    }
                    string guid = Guid.NewGuid().ToString();
                    using (FileStream fileStream = System.IO.File.Create(_hostingEnvironment.WebRootPath+"\\uploads\\"+guid+file.Picture.FileName.Replace("\\","s").Replace(":","s")))
                    {
                        file.Picture.CopyTo(fileStream);
                        fileStream.Flush();
                        user.PicPath = _hostingEnvironment.WebRootPath + "\\uploads\\" + guid + file.Picture.FileName;
                        await _userManager.UpdateAsync(user);
                        return "\\uploads\\" +guid+ file.Picture.FileName;
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


    }
}
