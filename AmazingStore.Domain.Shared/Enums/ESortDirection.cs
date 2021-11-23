using System.ComponentModel;

namespace AmazingStore.Domain.Shared.Enums
{
    public enum ESortDirection
    {
        [Description("Ascending")]
        Asc = 1,

        [Description("Descending")]
        Desc = 2
    }
}