using AutoMapper;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.DTO.PayoutModelDTO.Request;
using SWD392_AffiliLinker.Services.DTO.PayoutModelDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.Services.Services
{
	public class PayoutModelService : IPayoutModelService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public PayoutModelService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<GetPayoutModelsResponse>> GetPayoutModels()
		{
			var result = _unitOfWork.GetRepository<PayoutModel>().Entities.Where(s => s.Status == CommonStatus.Active.ToString());
			return _mapper.Map<IEnumerable<GetPayoutModelsResponse>>(result);
		}

		public async Task CreatePayoutModel(CreatePayoutModelRequest request)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var model = _unitOfWork.GetRepository<PayoutModel>().Entities.FirstOrDefault(s => s.Name == request.Name);
				if (model != null)
				{
					throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "This payout methoed is exist");
				}

				var result = _mapper.Map<PayoutModel>(request);
				result.CreatedTime = DateTime.UtcNow;
				result.LastUpdatedTime = DateTime.UtcNow;
				result.Status = CommonStatus.Active.ToString();
				await _unitOfWork.GetRepository<PayoutModel>().InsertAsync(result);
				await _unitOfWork.SaveAsync();
				_unitOfWork.CommitTransaction();
			}
			catch (Exception ex)
			{
				_unitOfWork.RollBack();
				throw;
			}
		}
	}
}
