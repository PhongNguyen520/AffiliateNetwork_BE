using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
    public interface IHepperUploadImage
    {
        Task<string> UploadImageAsync(Stream fileStream, string fileName);
    }
}
