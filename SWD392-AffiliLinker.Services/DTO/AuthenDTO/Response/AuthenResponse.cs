using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.AuthenDTO.Response
{
	public class AuthenResponse
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}
}
