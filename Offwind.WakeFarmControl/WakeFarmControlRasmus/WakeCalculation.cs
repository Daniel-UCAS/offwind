﻿using System;
using ILNumerics;

namespace WakeFarmControlR
{
    public partial class FarmControl
    {
        internal static void wakeCalculation(out ILArray<double> v_nac, ILArray<double> Ct, int i, ILArray<double> wind)
        {
            #region "Used variables declaration"
            ILArray<double> scaling;
            #endregion

            //% v_nac = WAKECALCULATION(Ct,i,wind)
            //This function calculates the wake
            //Currently it is a very very simplified wake calculation. It just serves as
            //a placeholder for a correct wake calculation that will come later

            scaling = linspace(0.5, 0.9, length(Ct));
            v_nac = scaling * wind._(i, 2);
        }
    }
}
