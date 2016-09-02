using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HighFive.Server.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace HighFive.Server.Web.Utils
{
    public interface IWrapSignInManager<T>
    {
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignOutAsync();
    }
    
}
