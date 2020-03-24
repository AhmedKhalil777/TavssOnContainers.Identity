using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Identity.Api.Contracts.V1.Requests;
using Identity.Api.Contracts.V1.Responses;
using Identity.Api.Data;
using Identity.Api.Domain;
using Identity.Api.Options;
using Identity.Api.Services;
using Identity.Api.V1.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Identity.Api.Controllers.v1
{

    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IProfileService _profileService;
        public AccountController(IProfileService profileService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _profileService = profileService;
        }

        [HttpGet("/")]
        public   IActionResult ReturnUsers() {
            //if (_userManager.Users.Count() == 0)
            //{
            //    using (var reader = new StreamReader("conseeder.json"))
            //    {
            //        var AUsers = new List<ApplicationUser>();
            //        var users = JsonConvert.DeserializeObject<List<UserViewModel>>(reader.ReadToEnd());
            //        foreach (var user in users)
            //        {
            //           await _profileService.RegisterUser(user);

            //        }

            //    }
            //}
           return  Ok( _userManager.Users);
        }

        #region RegisterDoctor
        [HttpPost(ApiRoutes.User.RegisterDoctor)]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorTAViewModel registerDoctor)
        {
            List<string> errorList = new List<string>();
            var applicationUser = new ApplicationUser
            {
                Email = registerDoctor.Email,
                UserName = registerDoctor.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                Department = registerDoctor.Department
            };

            var result = await _userManager.CreateAsync(applicationUser, registerDoctor.Password);
     
            if (result.Succeeded )
            {
                await _userManager.AddToRolesAsync(applicationUser, new List<string> { "Doctor" , "Developer" , "Instructor","Supervisor" , "User"});

                //Return successful
                return Ok(
                    new { username = applicationUser.UserName, email = applicationUser.Email, status = 1, message = "Register Successfully" }
                    );
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    errorList.Add(error.Description);
                }
            }
            return  BadRequest(errorList);
        }
        #endregion


        [HttpPost(ApiRoutes.User.RegisterAdmin)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterStudentViewModel registerAdmin)
        {
            List<string> errorList = new List<string>();
            var applicationUser = new ApplicationUser
            {
                Email = registerAdmin.Email,
                UserName = registerAdmin.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                Department = registerAdmin.Department,
                StudyYear = registerAdmin.StudyYear.ToString()

            };

            var result = await _userManager.CreateAsync(applicationUser, registerAdmin.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(applicationUser, new List<string> { "Admin" });

                //Return successful
                return Ok(
                    new { username = applicationUser.UserName, email = applicationUser.Email, status = 1, message = "Register Successfully" }
                    );
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    errorList.Add(error.Description);
                }
            }
            return BadRequest(errorList);
        }
        #region Register Student
        [HttpPost(ApiRoutes.User.RegisterStudent)]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentViewModel registerStudent)
        {
            List<string> errorList = new List<string>();
            var applicationUser = new ApplicationUser
            {
                Email = registerStudent.Email,
                UserName = registerStudent.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                Department = registerStudent.Department,
                StudyYear = registerStudent.StudyYear.ToString()

            };

            var result = await _userManager.CreateAsync(applicationUser, registerStudent.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(applicationUser, new List<string> { "User" , "Student" , "Developer"});

                //Return successful
                return Ok(
                    new { username = applicationUser.UserName, email = applicationUser.Email, status = 1, message = "Register Successfully" }
                    );
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    errorList.Add(error.Description);
                }
            }
            return BadRequest(errorList);
        }
        #endregion

        #region Register TA
        [HttpPost(ApiRoutes.User.RegisterTA)]
        public async Task<IActionResult> RegisterTA([FromBody] RegisterDoctorTAViewModel  registerTA)
        {
            List<string> errorList = new List<string>();
            var applicationUser = new ApplicationUser
            {
                Email = registerTA.Email,
                UserName = registerTA.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                Department = registerTA.Department
            };

            var result = await _userManager.CreateAsync(applicationUser, registerTA.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(applicationUser, new List<string> { "TeachingAssistant", "Developer", "Instructor", "Supervisor" , "User" });

                //Return successful
                return Ok(
                    new { username = applicationUser.UserName, email = applicationUser.Email, status = 1, message = "Register Successfully" }
                    );
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    errorList.Add(error.Description);
                }
            }
            return BadRequest(errorList);
        }

        #endregion

        #region StudentLogin
        [HttpPost(ApiRoutes.User.StudentLogin)]
        public async Task<IActionResult> StudentLogin([FromBody] LoginViewModel loginStudent)
        {
            var student = await _userManager.FindByEmailAsync(loginStudent.Email);
            var roles = await _userManager.GetRolesAsync(student);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            var tokenExpiryTime = Convert.ToDouble(_jwtSettings.ExpireTime);
            if (student !=null && await _userManager.CheckPasswordAsync(student, loginStudent.Password)&& roles.Contains("Student"))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(JwtRegisteredClaimNames.Sub, loginStudent.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, student.Id),
                        new Claim(ClaimTypes.Role, "Student"),
                        new Claim("LoggedOn", DateTime.UtcNow.ToString())

                    }),
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _jwtSettings.Site,
                    Audience = _jwtSettings.Audience,
                    Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)

                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token =tokenHandler.WriteToken(token) , expiration = token.ValidTo ,studentname = student.UserName , role = "Student"  });
            }

            ModelState.AddModelError("", "Email/Password was not found");
            return Unauthorized(new  {LoginError ="Please Check the login credentials - Invalid Email/Password was Entered" });
        }
        #endregion

        #region DoctorLogin
        [HttpPost(ApiRoutes.User.DoctorLogin)]
        public async Task<IActionResult> DoctorLogin([FromBody] LoginViewModel loginDoctor)
        {
            var doctor = await _userManager.FindByEmailAsync(loginDoctor.Email);
            var roles = await _userManager.GetRolesAsync(doctor);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            var tokenExpiryTime = Convert.ToDouble(_jwtSettings.ExpireTime);
            if (doctor != null && await _userManager.CheckPasswordAsync(doctor, loginDoctor.Password) && roles.Contains("Doctor"))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(JwtRegisteredClaimNames.Sub, loginDoctor.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, doctor.Id),
                        new Claim(ClaimTypes.Role, "Doctor"),
                        new Claim("LoggedOn", DateTime.UtcNow.ToString())

                    }),
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _jwtSettings.Site,
                    Audience = _jwtSettings.Audience,
                    Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)

                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token), expiration = token.ValidTo, studentname = doctor.UserName, role = "Doctor" });
            }

            ModelState.AddModelError("", "Email/Password was not found");
            return Unauthorized(new { LoginError = "Please Check the login credentials - Invalid Email/Password was Entered" });
        }
        #endregion

        #region TALogin
        [HttpPost(ApiRoutes.User.AdminLogin)]
        public async Task<IActionResult> AdminLogin([FromBody] LoginViewModel loginAdmin)
        {
            var admin = await _userManager.FindByEmailAsync(loginAdmin.Email);
            var roles = await _userManager.GetRolesAsync(admin);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            var tokenExpiryTime = Convert.ToDouble(_jwtSettings.ExpireTime);
            if (admin != null && await _userManager.CheckPasswordAsync(admin, loginAdmin.Password) && roles.Contains("Admin"))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(JwtRegisteredClaimNames.Sub, loginAdmin.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, admin.Id),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim("LoggedOn", DateTime.Now.ToString())

                    }),
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _jwtSettings.Site,
                    Audience = _jwtSettings.Audience,
                    Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)

                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token), expiration = token.ValidTo, studentname = admin.UserName, role = "Admin" });
            }
            ModelState.AddModelError("", "Email/Password was not found");
            return Unauthorized(new { LoginError = "Please Check the login credentials - Invalid Email/Password was Entered" });
        }
        [HttpPost(ApiRoutes.User.TALogin)]
        public async Task<IActionResult> TALogin([FromBody] LoginViewModel loginTA)
        {
            var ta = await _userManager.FindByEmailAsync(loginTA.Email);
            var roles = await _userManager.GetRolesAsync(ta);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            var tokenExpiryTime = Convert.ToDouble(_jwtSettings.ExpireTime);
            if (ta != null && await _userManager.CheckPasswordAsync(ta, loginTA.Password) && roles.Contains("TeachingAssistant"))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(JwtRegisteredClaimNames.Sub, loginTA.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, ta.Id),
                        new Claim(ClaimTypes.Role, "TeachingAssistant"),
                        new Claim("LoggedOn", DateTime.UtcNow.ToString())

                    }),
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _jwtSettings.Site,
                    Audience = _jwtSettings.Audience,
                    Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)

                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token), expiration = token.ValidTo, studentname = ta.UserName, role = "TA" });
            }

            ModelState.AddModelError("", "Email/Password was not found");
            return Unauthorized(new { LoginError = "Please Check the login credentials - Invalid Email/Password was Entered" });
        }
        #endregion

    }
}