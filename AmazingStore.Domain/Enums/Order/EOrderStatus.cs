using System.ComponentModel;

namespace AmazingStore.Domain.Enums.Order
{
    public enum EOrderStatus
    {
        [Description("Order Received")]
        Received = 1,

        [Description("Order In Process")]
        Processing = 2,

        [Description("Order Sent")]
        Sent = 3
    }
}