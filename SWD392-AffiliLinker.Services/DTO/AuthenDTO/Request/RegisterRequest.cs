using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request
{
	public class RegisterRequest
	{
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string? Email { get; set; }
		public string LastName { get; set; }
		public string? Password { get; set; }
		public String? PhoneNumber { get; set; }
		public string? Avatar { get; set; }
		public DateTime DOB { get; set; }
		public string Status { get; set; }
	}
}
