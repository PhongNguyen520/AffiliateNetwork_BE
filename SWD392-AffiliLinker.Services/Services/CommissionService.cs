using AutoMapper;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Repositories.UOW;
using SWD392_AffiliLinker.Services.DTO.CommissionDTO.Request;
using SWD392_AffiliLinker.Services.DTO.CommissionDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Services
{
    public class CommissionService : ICommissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CommissionService(IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<List<CommissionResponse>> GetListCommission(FillterCommission request)
        {
            try
            {
                var userId = _currentUserService.GetUserId();

                var user = await _unitOfWork.GetRepository<User>()
                                      .FindByAndInclude(_ => _.Id.ToString() == userId,
                                                        _ => _.Publisher);
                var publisherId = user.Publisher.Id;

                var listCommissionDb = _unitOfWork.GetRepository<Commission>()
                                                  .GetAll()
                                                  .Where(_ => _.PublisherId == publisherId);
                if(request.BeginDate.HasValue)
                {
                    listCommissionDb = listCommissionDb.Where(_ => _.CreatedTime >= request.BeginDate.Value);
                }
                if(request.EndDate.HasValue)
                {
                    listCommissionDb = listCommissionDb.Where(_ => _.CreatedTime <= request.EndDate.Value);
                }
                var newList = _mapper.Map<List<CommissionResponse>>(listCommissionDb.ToList());
                return newList;
            }
            catch
            {
                throw;
            }
        }
    }
}
