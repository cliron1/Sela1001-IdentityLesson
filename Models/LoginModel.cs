using System.ComponentModel.DataAnnotations;

namespace IdentityLesson.Models {
	public class LoginModel {
		[Display(Name = "שם משתמש:")]
		[Required(ErrorMessage = "שדה חובה")]
		[MaxLength(50, ErrorMessage = "ארוך מידי")]
		public string Username { get; set; }

		[Display(Name = "סיסמה:")]
		[Required(ErrorMessage = "שדה חובה")]
		[DataType(DataType.Password)]
		public string Pwd { get; set; }
	}
}
