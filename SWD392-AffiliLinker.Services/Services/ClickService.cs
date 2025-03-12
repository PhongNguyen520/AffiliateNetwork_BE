using AutoMapper;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.DTO.ClickDTO.Request;
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
    }
}
