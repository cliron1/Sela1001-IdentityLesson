using IdentityLesson.Data;
using IdentityLesson.Extensions;
using IdentityLesson.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityLesson.Controllers {
	public class AccountController : Controller {
		private readonly AppDbContext context;

		public AccountController(AppDbContext context) {
			this.context = context;
		}

		public IActionResult Login() {
			var debugData = new LoginModel {
				Email = "liron@flame-ware.com"
			};
			return View(debugData);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginModel model) {
			if(ModelState.IsValid) {
				var user = context.Users.Where(x => x.Email == model.Email && x.HashedPwd == model.Pwd.Hash()).FirstOrDefault();

				if(user != null) {
					var claims = new List<Claim> {
						new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
						new Claim(ClaimTypes.Name, user.Name),
						new Claim(ClaimTypes.Email, user.Email)
					};
					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);

					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					return RedirectToAction("Index", "Home");
				}

				ModelState.AddModelError("", "הפרטים אינם נכונים");
			}
			return View();
		}

		public IActionResult GoogleLogin() {
			var props = new AuthenticationProperties {
				RedirectUri = Url.Action("LoginWithGoogleCallback")
			};
			return Challenge(props, GoogleDefaults.AuthenticationScheme);
		}

		public async Task<IActionResult> LoginWithGoogleCallbackAsync() {
			var result = await HttpContext.AuthenticateAsync("ExternalCookie");
			var exClaims = result.Principal.Claims;

			var googleId = exClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
			var googleName = exClaims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value;
			var googleEmail = exClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

			var user = context.Users.Where(x => x.GoogleId == googleId).FirstOrDefault();

			if(user == null) {
				user = context.Users.Where(x => x.Email == googleEmail).FirstOrDefault();
				
				// Note: Profile exist by email but GoogleId is missing...
				if(user != null) {
					user.GoogleId = googleId;
					context.Users.Update(user);
					await context.SaveChangesAsync();

				} else {
					// Note: Create a new User
					user = new User {
						Name = googleName,
						Email = googleEmail,
						GoogleId = googleId
					};

					context.Users.Add(user);
					await context.SaveChangesAsync();
				}
			}

			var claims = new List<Claim> {
						new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
						new Claim(ClaimTypes.Name, user.Name),
						new Claim(ClaimTypes.Email, user.Email)
					};
			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);

			await HttpContext.SignOutAsync("ExternalCookie");
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

			return RedirectToAction("Index", "Home");
		}

		public IActionResult Signup() {
			var debugData = new SignupModel {
				Name = "לירון כהן",
				Email = "liron@flame-ware.com"
			};
			return View(debugData);
		}

		[HttpPost]
		public async Task<IActionResult> Signup(SignupModel model) {
			if(ModelState.IsValid) {
				var user = context.Users.Where(x => x.Email == model.Email).FirstOrDefault();

				if(user == null) {
					user = new User {
						Name = model.Name,
						Email = model.Email,
						HashedPwd = model.Pwd.Hash()
					};

					var result = context.Users.Add(user);
					try {
						var affected = await context.SaveChangesAsync();
						if(affected > 0)
							return RedirectToAction("SignupSuccess");

					} catch(Exception ex) {
						ModelState.AddModelError("", ex.Message);
						//ModelState.AddModelError("", "לא הצלחנו לרשום אותך למערכת...");
					}
				} else {
					ModelState.AddModelError("", "קיים כבר משתמש עם אימייל זה");
				}
			}
			return View();
		}

		public IActionResult SignupSuccess() {
			return View();
		}

		public async Task<IActionResult> Logout() {
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}
	}
}
