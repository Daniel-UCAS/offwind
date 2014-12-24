﻿using System;
using ILNumerics;

namespace WakeFarmControlR
{
    public sealed class Turb_centr_coord : MatlabCode
    {
        // Wake Code - Matlab
        // Rasmus Christensen
        // Control and Automation, Aalborg University
        public static void Calculate(out ILArray<int> output, int nTurb, int iMax, ILArray<double> x, ILArray<double> xTurb, int gridRes)
        {
            ILArray<int> xxcTurb = zeros<int>(1, nTurb);

            for (var i = 1; i <= nTurb; i++)
            {
                for (var ii = 1; ii <= iMax - 1; ii++)
                {
                    if (abs(x._(ii)) <= abs(xTurb._(i)) && abs(xTurb._(i)) < abs(x._(ii + 1)))
                    {
                        xxcTurb._(i, '=', ii * sign(xTurb._(i)));
                        break;
                    }
                }
            }
            output = xxcTurb;
        }
    }
}
