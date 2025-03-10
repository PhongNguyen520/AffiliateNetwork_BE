using SWD392_AffiliLinker.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
    public interface ICurrentUserService
    {
        string GetUserId();
        string getUserEmail();
        Task<User> GetCurrentAccountAsync();
    }
}
