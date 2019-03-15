using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NetCoreApp.Data.Enums
{
    public enum BillStatus
    {
        [Description("New")]
        New,
        [Description("InProgress")]
        InProgress,
        [Description("Returned")]
        Returned,
        [Description("Cancelled")]
        Cancelled,
        [Description("Completed")]
        Completed
    }
}
