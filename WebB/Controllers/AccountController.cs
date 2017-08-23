using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CookieInstance;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebB.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebB.Controllers
{
    public class AccountController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="model">UserViewModel实体信息</param>
        /// <param name="returnUrl">返回Url地址</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);

            try
            {
                if (model.Account.Equals("admin", StringComparison.OrdinalIgnoreCase) && model.Password.Equals("123456", StringComparison.OrdinalIgnoreCase))
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, "1213131", ClaimValueTypes.UInteger32));
                    claims.Add(new Claim(ClaimTypes.Name, "WEBAs", ClaimValueTypes.String));
                    claims.Add(new Claim(ClaimTypes.Role, "Normal", ClaimValueTypes.String));
                    var userIdentity = new ClaimsIdentity(CookieAuthInfo.AuthenticationType);
                    userIdentity.AddClaims(claims);
                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(CookieAuthInfo.CookieInstance, userPrincipal,
                       new AuthenticationProperties
                       {
                           ExpiresUtc = DateTime.UtcNow.AddHours(12),
                           IsPersistent = true,
                           AllowRefresh = false
                       });

                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = "/Home";
                    }

                    return Redirect(returnUrl);

                }

                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        /// <summary>
        /// 拒绝访问
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Denied(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthInfo.CookieInstance);
            return RedirectToAction("Login", "Account");
        }
    }
}
