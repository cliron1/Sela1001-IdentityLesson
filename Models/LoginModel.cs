using System.ComponentModel.DataAnnotations;

namespace IdentityLesson.Models {
	public class LoginModel {
		[Display(Name = "אימייל:")]
		[Required(ErrorMessage = "שדה חובה")]
		[EmailAddress(ErrorMessage = "אימייל לא חוקי")]
		public string Email { get; set; }

		[Display(Name = "סיסמה:")]
		[Required(ErrorMessage = "שדה חובה")]
		[DataType(DataType.Password)]
		public string Pwd { get; set; }
	}
}
