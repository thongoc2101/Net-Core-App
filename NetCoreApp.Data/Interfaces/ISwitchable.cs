using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Data.Enums;

namespace NetCoreApp.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
