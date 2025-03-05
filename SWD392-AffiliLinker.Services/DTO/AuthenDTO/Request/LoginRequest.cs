using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.AuthenDTO.Request
{
	public class LoginRequest
	{
		[Required(ErrorMessage = "Required")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Required")]
		public string Password { get; set; }
	}
}
