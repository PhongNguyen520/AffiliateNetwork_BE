using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Core.Store
{
    public enum EnumRolesRegister
    {
        [EnumMember(Value = "Publisher")]
        Publisher,

        [EnumMember(Value = "Advertiser")]
        Advertiser,
    }

    public enum RoleFillterAccount
    {
        Advertiser,
        Publisher,
        Admin
    }
}
