using System;
using System.Security.Cryptography;
using System.Text;

namespace IdentityLesson.Extensions {
	public static class SecurityExtensions {

		/// <summary>
		/// Hash a string using MD5
		/// </summary>
		/// <param name="input">The string to hash</param>
		/// <returns>The hashed string</returns>
		public static string Hash(this string input) {
			using(MD5 md5 = MD5.Create()) {
				byte[] byteHash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
				string hash = BitConverter.ToString(byteHash).Replace("-", "");
				return hash;
			}
		}
	}
}
