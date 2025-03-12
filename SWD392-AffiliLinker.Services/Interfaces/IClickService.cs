using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Services.DTO.ClickDTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
    public interface IClickService
    {
        Task CreateClickInfo(ClickInfoRequest request);


    }
}
