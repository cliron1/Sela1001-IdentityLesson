using System.ComponentModel.DataAnnotations;

namespace IdentityLesson.Models {
	public class SignupModel {
		[Display(Name = "שם משתמש:")]
		[Required(ErrorMessage = "שדה חובה")]
		[MaxLength(50, ErrorMessage = "ארוך מידי")]
		public string Username { get; set; }

		[Display(Name = "אימייל:")]
		[Required(ErrorMessage = "שדה חובה")]
		[EmailAddress(ErrorMessage = "אימייל לא חוקי")]
		public string Email { get; set; }

		[Display(Name = "סיסמה:")]
		[Required(ErrorMessage = "שדה חובה")]
		[DataType(DataType.Password)]
		public string Pwd { get; set; }

		[Display(Name = "וידוא סיסמה:")]
		[Required(ErrorMessage = "שדה חובה")]
		[DataType(DataType.Password)]
		[Compare("Pwd", ErrorMessage = "סיסמאות לא זהות")]
		public string ConfirmPwd { get; set; }
	}
}
