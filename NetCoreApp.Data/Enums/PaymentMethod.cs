using System.ComponentModel;

namespace NetCoreApp.Data.Enums
{
    public enum PaymentMethod
    {
        [Description("Cash on delivery")]
        CashOnDelivery,
        [Description("Online banking")]
        OnlineBanking,
        [Description("Payment Gateway")]
        PaymentGateway,
        [Description("Visa")]
        Visa,
        [Description("Master Card")]
        MasterCard,
        [Description("PayPal")]
        PayPal,
        [Description("Atm")]
        Atm
    }
}
