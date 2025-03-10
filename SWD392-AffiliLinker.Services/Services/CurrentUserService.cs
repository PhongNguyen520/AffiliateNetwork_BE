using Microsoft.AspNetCore.Http;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;

        }

        public async Task<User> GetCurrentAccountAsync()
        {
            _unitOfWork.BeginTransaction();
            try
            {
                string userId = GetUserId();
                var account = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
                _unitOfWork.CommitTransaction();
                return account;
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, Core.Store.StatusCodes.BadRequest.Name(), "Login Before USE!!!!");
            }
            
        }

        public string getUserEmail()
        {
            try
            {
                return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            }
            catch
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, Core.Store.StatusCodes.BadRequest.Name(), "Login Before USE!!!!");
            }
        }

        public string GetUserId()
        {
            try
            {
                return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            catch
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, Core.Store.StatusCodes.BadRequest.Name(), "Login Before USE!!!!");
            }
        }
    }
}
