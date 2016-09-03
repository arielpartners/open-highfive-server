using System.Diagnostics.CodeAnalysis;
using HighFive.Server.Services.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HighFive.Server.Web.Utils
{
    [ExcludeFromCodeCoverage]
    public class WrapSignInManager<T>: IWrapSignInManager<T>
    {
        public WrapSignInManager(SignInManager<HighFiveUser> signInManager)
        {
            WrappedSignInManager = signInManager;
        }

        public WrapSignInManager()
        {
            // use me for testing
        }

        public SignInManager<HighFiveUser> WrappedSignInManager { get; set; }

        public Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return WrappedSignInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }

        public Task SignOutAsync()
        {
            return WrappedSignInManager.SignOutAsync();
        }
    }
}
