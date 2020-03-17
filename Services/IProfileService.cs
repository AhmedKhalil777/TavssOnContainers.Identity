using Identity.Api.Contracts.V1.Requests;
using Identity.Api.Contracts.V1.Responses;
using Identity.Api.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Services
{
    public interface IProfileService
    {
        Task<IEnumerable<IViewModel>> getAll(string userType);
        Task<IViewModel> getUserById(string Id, string role);
        Task<bool> updateUser(string Id, UpdateUserViewModel user);
        Task<bool> deleteUser(string Id);
        Task<string> updateImage(string Id , ImageViewModel file);
        Task<bool> updatePassword(string Id, string OldPassword , string NewPassword);
    }
}
