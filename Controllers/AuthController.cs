using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Net.Http;

namespace ColorvisionPaintsERP.Controllers
{
    public class AuthController : Controller
    {
        private readonly IFirebaseAuthService _authService;
        private readonly IFirestoreService _firestoreService;

        public AuthController(IFirebaseAuthService authService, IFirestoreService firestoreService)
        {
            _authService = authService;
            _firestoreService = firestoreService;
        }

        [HttpGet]
        public IActionResult Login([FromQuery] string mode = "phone")
        {
            ViewBag.Mode = mode;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailLogin(EmailLoginViewModel model)
        {
            try
            {
                var authResult = await _authService.LoginWithEmailAsync(model.Email, model.Password);
                await HandlePostLoginAsync(authResult.LocalId, authResult.Email, string.Empty, authResult.IdToken);
                return RedirectToDashboard();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Mode = "email";
                return View("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendOtp(PhoneLoginViewModel model)
        {
            try
            {
                var otpResult = await _authService.SendOtpAsync(model.PhoneNumber);
                ViewBag.OtpSent = true;
                ViewBag.SessionInfo = otpResult.SessionInfo;
                ViewBag.PhoneNumber = model.PhoneNumber;
                ViewBag.Mode = "phone";
                return View("Login");
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Mode = "phone";
                return View("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(OtpVerifyViewModel model)
        {
            try
            {
                var authResult = await _authService.VerifyOtpAsync(model.SessionInfo, model.Code);
                await HandlePostLoginAsync(authResult.LocalId, string.Empty, model.PhoneNumber, authResult.IdToken);
                return RedirectToDashboard();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.OtpSent = true; // Stay on OTP step
                ViewBag.SessionInfo = model.SessionInfo;
                ViewBag.PhoneNumber = model.PhoneNumber;
                ViewBag.Mode = "phone";
                return View("Login");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private string _lastLoginRole = string.Empty;

        private async Task HandlePostLoginAsync(string uid, string email, string phone, string idToken)
        {
            // Try to load existing profile from Firestore
            UserProfile? profile = null;
            try
            {
                profile = await _firestoreService.GetUserProfileAsync(uid, idToken);
            }
            catch { /* Firestore may be unavailable — continue with defaults */ }

            if (profile == null)
            {
                // Build profile in memory — this is a new user
                profile = new UserProfile 
                { 
                    Uid = uid, 
                    Name = !string.IsNullOrEmpty(email) ? email.Split('@')[0] : "User",
                    Email = email, 
                    Mobile = phone, 
                    Role = UserRoles.DealerPortalUser
                };

                // Try to check if first user (→ SuperAdmin)
                try
                {
                    var isFirst = await _firestoreService.IsFirstUserAsync(idToken);
                    if (isFirst)
                    {
                        profile.Role = UserRoles.SuperAdmin;
                    }
                }
                catch { /* If Firestore fails, keep default role */ }

                // Try to save — don't crash if it fails
                try
                {
                    await _firestoreService.CreateUserProfileAsync(uid, profile, idToken);
                }
                catch { /* Firestore write may fail due to security rules */ }
            }

            _lastLoginRole = profile.Role;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, profile.Uid),
                new Claim(ClaimTypes.Name, profile.Name),
                new Claim("Email", profile.Email),
                new Claim(ClaimTypes.Role, profile.Role),
                new Claim("Role", profile.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private IActionResult RedirectToDashboard()
        {
            // Use the role we just set during login (cookie isn't readable yet in this request)
            var role = _lastLoginRole;
            if (string.IsNullOrEmpty(role))
            {
                role = User.FindFirst("Role")?.Value ?? string.Empty;
            }
            return role switch
            {
                UserRoles.SuperAdmin or UserRoles.FinanceManager => Redirect("/Dashboard/Admin"),
                UserRoles.SalesHead or UserRoles.AreaManager => Redirect("/Dashboard/Sales"),
                UserRoles.SalesRep => Redirect("/Dashboard/Salesman"),
                UserRoles.WarehouseManager => Redirect("/Dashboard/Warehouse"),
                _ => Redirect("/Dashboard/Dealer")
            };
        }
    }
}
