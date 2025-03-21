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
                var converDb = _mapper.Map<Conversion>(request);
                converDb.CreatedTime = DateTime.Now;
                converDb.LastUpdatedTime = DateTime.Now;
                converDb.Status = ConversionStatus.Pending.ToString();
                converDb.IsPayment = false;
                await _unitOfWork.GetRepository<Conversion>().InsertAsync(converDb);
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

                var pagingList =await _unitOfWork.GetRepository<Conversion>().GetPagging(listDb, request.PageIndex, request.PageSize);

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
