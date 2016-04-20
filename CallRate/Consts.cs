using System;

namespace CallRate {
    internal class Consts {
        #region Declarations

        public const int Am12To6AmRate = 1;
        public const int Am6To12PmRate = 3;
        public const int Pm12To6PmRate = 4;
        public const int Pm6To12AmRate = 2;

        public const int Am12To6AmTotalCost = Am12To6AmRate * 6 * 60;
        public const int Am6To12PmTotalCost = Am6To12PmRate * 6 * 60;
        public const int Pm12To6PmTotalCost = Pm12To6PmRate * 6 * 60;
        public const int Pm6To12AmTotalCost = Pm6To12AmRate * 6 * 60;

        public const int OneDayCost = Am12To6AmTotalCost + Am6To12PmTotalCost + Pm12To6PmTotalCost + Pm6To12AmTotalCost;

        /// <summary>
        ///     12:00 AM
        /// </summary>
        public static readonly TimeSpan Time12Am = new TimeSpan(0, 0, 0); // 12:00 AM;

        /// <summary>
        ///     05:59 AM
        /// </summary>
        public static readonly TimeSpan Time559Am = new TimeSpan(5, 59, 0); // 05:59 AM;

        /// <summary>
        ///     06:00 AM
        /// </summary>
        public static readonly TimeSpan Time6Am = new TimeSpan(6, 0, 0); // 06:00 AM;

        /// <summary>
        ///     11: 59 AM
        /// </summary>
        public static readonly TimeSpan Time1159Am = new TimeSpan(11, 59, 0); // 11: 59 AM;

        /// <summary>
        ///     12: 00 PM
        /// </summary>
        public static readonly TimeSpan Time12Pm = new TimeSpan(12, 0, 0); // 12: 00 PM;

        /// <summary>
        ///     17:59 or 5:59 PM
        /// </summary>
        public static readonly TimeSpan Time559Pm = new TimeSpan(17, 59, 0); // 17:59 or 5:59 PM

        /// <summary>
        ///     18:00 or 6:00 PM
        /// </summary>
        public static readonly TimeSpan Time6Pm = new TimeSpan(18, 0, 0); // 18:00 or 6:00 PM

        /// <summary>
        ///     23:59 or 11:59:59 PM
        /// </summary>
        public static readonly TimeSpan Time1159Pm = new TimeSpan(23, 59, 59); // 23:59 or 11:59:59 PM

        #endregion
    }
}