#region references

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HighFive.Server.Web.ViewModels;
using HighFive.Server.Services.Models;

#endregion

namespace HighFive.Server.Web.Controllers
{
    [TestClass]
    public class AuthControllerTest : Controller
    {
        #region TestPost

        [TestMethod]
        public void TestPost_UserAuth()
        {
            //var authCtrl = new AuthController(_signInManager, _repositry, _logger);
            //var usr = new AuthViewModel { Email = "test.user@email.com", Pwd = "password" };
            //authCtrl.Login(usr);
        }

        #endregion

        #region properties

        private ILogger<AuthController> _logger => new Mock<ILogger<AuthController>>().Object;

        private HighFiveRepository _repositry => new Mock<HighFiveRepository>().Object;

        private SignInManager<HighFiveUser> _signInManager => new Mock<SignInManager<HighFiveUser>>().Object;

        #endregion
    }
}

