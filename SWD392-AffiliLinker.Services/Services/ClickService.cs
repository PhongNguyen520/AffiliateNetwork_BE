using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.DTO.ClickDTO.Request;
using SWD392_AffiliLinker.Services.DTO.ClickDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Services
{
    public class ClickService : IClickService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClickService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateClickInfo(ClickInfoRequest request)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var clickInfoDb = _mapper.Map<ClickInfo>(request);
                clickInfoDb.CreatedTime = DateTime.Now;
                clickInfoDb.LastUpdatedTime = DateTime.Now;

                await _unitOfWork.GetRepository<ClickInfo>().InsertAsync(clickInfoDb);

                await PlusClick(request.AffiliateLinkId);

                await _unitOfWork.SaveAsync();
                _unitOfWork.CommitTransaction();
            }
            catch
            {
                _unitOfWork.RollBack();
                throw;
            }
        }


        public async Task PlusClick(string AffiliateLinkId)
        {
            try
            {
                var checkClinkDay = await _unitOfWork.GetRepository<ClickCount>()
                                                     .FirstOrDefaultAsync(
                                                      c => c.CreatedTime.Date == DateTime.Now.Date &&
                                                      c.AffiliateLinkId == AffiliateLinkId);

                if(checkClinkDay != null)
                {
                    checkClinkDay.Count++;
                    checkClinkDay.LastUpdatedTime = DateTime.Now;
                    await _unitOfWork.GetRepository<ClickCount>()
                                     .UpdateAsync(checkClinkDay);
                    return;
                }
                ClickCount clickCount = new ClickCount()
                {
                    Count = 1,
                    AffiliateLinkId = AffiliateLinkId,
                    LastUpdatedTime = DateTime.Now,
                    CreatedTime = DateTime.Now
                };
                await _unitOfWork.GetRepository<ClickCount>().InsertAsync(clickCount);
            }
            catch
            {
                throw;
            }
        }

        public async Task<BasePaginatedList<TotalClickInfoResponse>> GetTotalClickInfo(TotalClickInfoRequest request)
        {
            try
            {
                var listDB = _unitOfWork.GetRepository<ClickInfo>()
                                    .Entities
                                    .AsNoTracking()
                                    .Where(_ => _.AffiliateLinkId == request.AffiLinkId);

                if (request.Status.HasValue)
                {
                    listDB = listDB.Where(_ => _.Status == request.Status.ToString());
                }
                if (request.BeginDate.HasValue)
                {
                    listDB = listDB.Where(_ => _.CreatedTime.Date >= request.BeginDate.Value.Date);
                }
                if (request.EndDate.HasValue)
                {
                    if (request.BeginDate >= request.EndDate)
                    {
                        throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "Start date must be less than end date!!!");
                    }
                    listDB = listDB.Where(_ => _.CreatedTime.Date <= request.EndDate.Value.Date);
                }
                var pagingList = await _unitOfWork.GetRepository<ClickInfo>().GetPagging(listDB, request.Index, request.Size);

                var result = _mapper.Map<BasePaginatedList<TotalClickInfoResponse>>(pagingList);

                return result;
                
            }
            catch
            {
                throw;
            }
        }
    }
}
