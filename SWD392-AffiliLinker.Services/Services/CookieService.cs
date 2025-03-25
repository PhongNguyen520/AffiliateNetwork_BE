using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.Interfaces;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWD392_AffiliLinker.Core.Utils;

namespace SWD392_AffiliLinker.Services.Services
{
    public class CookieService : ICookieService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetCookie(string key)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, Core.Store.StatusCodes.NotFound.Name(), "HttpContext existed!");
                }
                httpContext.Request.Cookies.TryGetValue(key, out string? value);
                return value;
            }
            catch (BaseException.ErrorException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting cookie", ex);
            }
        }

        public void SetCookie(string value)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    throw new BaseException.ErrorException(Core.Store.StatusCodes.NotFound, Core.Store.StatusCodes.NotFound.Name(), "HttpContext existed!");

                var key = ".Affiliate.Tracking.Application";

                var options = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(30)
                };

                httpContext.Response.Cookies.Append(key, value, options);
            }
            catch (BaseException.ErrorException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Error setting cookie", ex);
            }
        }
    }
}
