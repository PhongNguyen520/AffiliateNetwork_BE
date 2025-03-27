using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.DTO.ClickDTO.Response;
using SWD392_AffiliLinker.Services.DTO.ConversionDTO.Request;
using SWD392_AffiliLinker.Services.DTO.ConversionDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.Services.Services
{
	public class ConversionService : IConversionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ConversionService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<string> CreateConversionAsync(CreateConversion request)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var affiliateLink = _unitOfWork.GetRepository<AffiliateLink>()
												   .GetById(request.AffiliateLinkId);
				if (affiliateLink is null)
				{
                    throw new BaseException.ErrorException(StatusCodes.NotFound, StatusCodes.NotFound.Name(), "AffiliateLink not exist!!!");
                }

				var converRate = _unitOfWork.GetRepository<Campaign>()
												   .GetById(affiliateLink.CampaignId)
												   .ConversionRate;
                var user = await _unitOfWork.GetRepository<User>()
                        .FindByAndInclude(_ => _.Id == affiliateLink.UserId,
                            _ => _.Publisher);

                var commission = request.Subtotal * converRate;

				var converDb = _mapper.Map<Conversion>(request);
				converDb.CreatedTime = DateTime.Now;
				converDb.LastUpdatedTime = DateTime.Now;
				converDb.Status = ConversionStatus.Pending.ToString();
				converDb.IsPayment = false;
				converDb.Commission = commission.Value;


				await _unitOfWork.GetRepository<Conversion>().InsertAsync(converDb);

				var commissionEntity = await _unitOfWork.GetRepository<Commission>()
														.FirstOrDefaultAsync(_ => _.CreatedTime.Date == DateTime.Now.Date
																			  && _.PublisherId == user.Publisher.Id);
				if (commissionEntity != null)
				{
					commissionEntity.TotalCommission += commission.Value;
					commissionEntity.LastUpdatedTime = DateTime.Now;
					_unitOfWork.GetRepository<Commission>().Update(commissionEntity);
				}
				else
				{
					commissionEntity = new Commission();

					commissionEntity.TotalCommission = commission.Value;
					
					commissionEntity.PublisherId = user.Publisher.Id;
					commissionEntity.CreatedTime = DateTime.Now;
					commissionEntity.LastUpdatedTime = DateTime.Now;

					await _unitOfWork.GetRepository<Commission>().InsertAsync(commissionEntity);
				}

				await _unitOfWork.SaveAsync();
				_unitOfWork.CommitTransaction();
				return converDb.Id;

			}
			catch
			{
				_unitOfWork.RollBack();
				throw;
			}
		}

		public async Task<ConversionResponse> GetConversionById(string converId)
		{
			try
			{
				var conversionDb = await _unitOfWork.GetRepository<Conversion>()
													.FirstOrDefaultAsync(_ => _.Id == converId);
				if (conversionDb == null)
				{
					throw new BaseException.ErrorException(StatusCodes.NotFound, StatusCodes.NotFound.Name(), "Conversion not exist!!!");
				}
				var result = _mapper.Map<ConversionResponse>(conversionDb);

				return result;

			}
			catch
			{
				throw;
			}
		}

		public async Task<BasePaginatedList<ConversionResponse>> ListConverByAffiLinkId(FillterConverByAffiLinkId request)
		{
			try
			{
				var listDb = _unitOfWork.GetRepository<Conversion>()
										.Entities
										.Where(_ => _.AffiliateLinkId == request.AffiLinkId)
										.AsNoTracking();

				if (request.Status.HasValue)
				{
					listDb = listDb.Where(_ => _.Status == request.Status.ToString());
				}
				if (request.IsPayment.HasValue)
				{
					listDb = listDb.Where(_ => _.IsPayment == request.IsPayment);
				}
				if (request.BeginDate.HasValue)
				{
					listDb = listDb.Where(_ => _.CreatedTime.Date >= request.BeginDate.Value.Date);
				}
				if (request.EndDate.HasValue)
				{
					if (request.BeginDate >= request.EndDate)
					{
						throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "Start date must be less than end date!!!");
					}
					listDb = listDb.Where(_ => _.CreatedTime.Date <= request.EndDate.Value.Date);
				}

				var pagingList = await _unitOfWork.GetRepository<Conversion>().GetPagging(listDb, request.PageIndex, request.PageSize);

				var result = _mapper.Map<BasePaginatedList<ConversionResponse>>(pagingList);

				return result;
			}
			catch
			{
				throw;
			}
		}
	}
}
