using IdentityLesson.Data;
using IdentityLesson.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityLesson.Controllers {
	public class AccountController : Controller {
		private readonly UserManager<AppUser> userManager;

		public AccountController(UserManager<AppUser> userManager) {
			this.userManager = userManager;
		}


		public IActionResult Login() {
			var debugData = new LoginModel {
				Username = "Liron",
				Pwd = "Qwerty1@"
			};
			return View(debugData);
		}

		[HttpPost]
		public async Task<IActionResult> LoginAsync(LoginModel model) {
			if(ModelState.IsValid) {
				var user = await userManager.FindByNameAsync(model.Username);
				if(user != null && await userManager.CheckPasswordAsync(user, model.Pwd)) {
					var identity = new ClaimsIdentity();
					identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
					var principal = new ClaimsPrincipal(identity);
					await HttpContext.SignInAsync(principal);

					return RedirectToAction("Index", "Home");
				}

				ModelState.AddModelError("", "הפרטים אינם נכונים");
			}
			return View();
		}

		public IActionResult Signup() {
			//var debugData = new SignupModel {
			//	Username = "Liron",
			//	Email = "lironco@sela.co.il",
			//	Pwd = "Qwerty1@",
			//	ConfirmPwd = "Qwerty1@"
			//};
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignupAsync(SignupModel model) {
			if(ModelState.IsValid) {
				var user = await userManager.FindByNameAsync(model.Username);

				if(user == null) {
					user = new AppUser {
						UserName = model.Username,
						Email = model.Email
					};

					var result = await userManager.CreateAsync(user, model.Pwd);
					if(result.Succeeded)
						return RedirectToAction("SignupSuccess");

					if(result.Errors != null && result.Errors.Any())
						foreach(var error in result.Errors)
							ModelState.AddModelError("", error.Description);
					else
						ModelState.AddModelError("", "לא הצלחנו לרשום אותך למערכת...");
				}
			}
			return View();
		}

		public IActionResult SignupSuccess() {
			return View();
		}
	}
}
