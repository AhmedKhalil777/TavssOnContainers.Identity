using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Api.Contracts.V1.Responses;
using Identity.Api.Domain;
using Identity.Api.V1.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Identity.Api.Services;
using Identity.Api.Contracts.V1.Requests;

namespace Identity.Api.Controllers.v1
{


    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        #region Git ALL
        [HttpGet(ApiRoutes.ProfileRoutes.GetUsers)]
        public async Task<IActionResult> GetUsers() => Ok(await _profileService.getAll("User"));

        [HttpGet(ApiRoutes.ProfileRoutes.GetStudents)]
        public async Task<IActionResult> GetStudents() => Ok(await _profileService.getAll("Student"));


        [HttpGet(ApiRoutes.ProfileRoutes.GetDoctors)]
        public async Task<IActionResult> GetDoctors() => Ok(await _profileService.getAll("Doctor"));


        [HttpGet(ApiRoutes.ProfileRoutes.GetTAs)]
        public async Task<IActionResult> GetTAs() => Ok(await _profileService.getAll("TeachingAssistant"));

        #endregion

        #region Git one
        [HttpGet(ApiRoutes.ProfileRoutes.GetUser)]
        public async Task<IActionResult> GetUser([FromRoute] string Id) => Ok(await _profileService.getUserById(Id, "User"));

        [HttpGet(ApiRoutes.ProfileRoutes.GetDoctor)]
        public async Task<IActionResult> GetDoctor([FromRoute] string Id) => Ok(await _profileService.getUserById(Id, "Doctor"));

        [HttpGet(ApiRoutes.ProfileRoutes.GetStudent)]
        public async Task<IActionResult> GetStudent([FromRoute] string Id) => Ok(await _profileService.getUserById(Id, "Student"));

        [HttpGet(ApiRoutes.ProfileRoutes.GetTA)]
        public async Task<IActionResult> GetTA([FromRoute] string Id) => Ok(await _profileService.getUserById(Id, "TeachingAssistant"));

        #endregion

        #region Update 
        [HttpPut(ApiRoutes.ProfileRoutes.UpdateUser)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserViewModel oldUser)
        {
            var result = await _profileService.updateUser(oldUser.Id, oldUser);
            if (result)
            {
                return Ok(new { Id = oldUser.Id, UserName = oldUser.Username, Status = "1", message = "Updated Successfully" });
            }
            return BadRequest();
        }

        [HttpPut(ApiRoutes.ProfileRoutes.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromRoute] string Id , [FromBody] ChangePasswordViewModel model)
        {
            var result = await _profileService.updatePassword(Id,model.OldPassword , model.NewPassword);
            if (result)
            {
                return Ok(new { Id, Status = "1", Message = "Password Changed Successfuly" });

            }
            return BadRequest();

        }

        [HttpPost(ApiRoutes.ProfileRoutes.UpdateImage)]
        public async Task<IActionResult> UpdateImage([FromRoute] string  Id ,[FromForm] ImageViewModel image)
        {
            var result = await _profileService.updateImage(Id, image.Picture);
            if (result == "Unsuccessful")
            {
                return BadRequest(result);
            }
            return Ok(new { Process = result });
        }
        #endregion
        #region Delete
        [HttpDelete(ApiRoutes.ProfileRoutes.DeleteUser)]
        public async Task<IActionResult> DeleteUser([FromRoute] string Id)
        {
            var result = await _profileService.deleteUser(Id);
            if (result)
            {
                return Ok(new { Status = "1", Message = "User Deleted" });
            }
            return BadRequest();
        }
        #endregion
    }
}