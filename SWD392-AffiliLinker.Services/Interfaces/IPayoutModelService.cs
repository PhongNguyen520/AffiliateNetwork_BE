using SWD392_AffiliLinker.Services.DTO.PayoutModelDTO.Request;
using SWD392_AffiliLinker.Services.DTO.PayoutModelDTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
	public interface IPayoutModelService
	{
		Task<IEnumerable<GetPayoutModelsResponse>> GetPayoutModels();
		Task CreatePayoutModel(CreatePayoutModelRequest request);
	}
}
