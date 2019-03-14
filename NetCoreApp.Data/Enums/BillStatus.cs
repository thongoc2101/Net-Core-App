using System.ComponentModel;

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
