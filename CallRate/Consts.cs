using System;
using System.Linq;
namespace CallRate {
    internal class Consts {
        #region Declarations

        public static readonly double[] Cost = { 1, 3, 4, 2 }; // per min cost
        public static readonly double OneDayCost = 6 * 60 * Cost.Sum();

        #endregion
    }
}