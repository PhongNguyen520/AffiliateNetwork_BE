using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.DTO.UserCurrentDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static SWD392_AffiliLinker.Core.Base.BaseException;

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
                return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new();
            }
            catch
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, Core.Store.StatusCodes.BadRequest.Name(), "Login Before USE!!!!");
            }
        }


            public Task<InfoClientResponse> GetClientInfo()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, Core.Store.StatusCodes.NotFound.Name(), "HttpContext not found!");
            }

            // Lấy IP từ Proxy trước (nếu có)
            string? ipAddress = null;
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out StringValues forwardedValues))
            {
                ipAddress = forwardedValues.FirstOrDefault()?.Split(',').First().Trim();
            }
            else
            {
                ipAddress = context.Connection.RemoteIpAddress?.ToString();
            }

            // Nếu không lấy được IP, throw exception
            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, Core.Store.StatusCodes.NotFound.Name(), "Client IP Address not found!");
            }

            // Lấy User-Agent
            if (!context.Request.Headers.TryGetValue("User-Agent", out StringValues userAgentValues) || string.IsNullOrEmpty(userAgentValues))
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, Core.Store.StatusCodes.NotFound.Name(), "User-Agent not found!");
            }

            string userAgent = userAgentValues.ToString();

            return Task.FromResult( new InfoClientResponse
            {
                IpAddress = ipAddress,
                UserAgent = userAgent,
            });
        }
    }
}
