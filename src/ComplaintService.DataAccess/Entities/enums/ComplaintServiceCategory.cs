using System.ComponentModel;

namespace ComplaintService.DataAccess.Entities.enums
{
    public enum ComplaintServiceCategory
    {
        /// <summary>
        ///     General
        /// </summary>
        [Description("General")] General = 1,

        /// <summary>
        ///     Bank Transfer
        /// </summary>
        [Description("Bank Transfer")] BankTransfer,

        /// <summary>
        ///     Bank Transfer
        /// </summary>
        [Description("Bank Transfer")] InternationalTransfer,

        /// <summary>
        ///     Bank Transfer
        /// </summary>
        [Description("Bank Transfer")] Airtime,

        /// <summary>
        ///     Bank Transfer
        /// </summary>
        [Description("Bank Transfer")] BillPayment
    }
}