﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILNumerics;

namespace WakeFarmControl
{
    public sealed class WindTurbineParameters
    {
        public int N;
        public double rho;
        public ILArray<double> radius;
        public ILArray<double> rated;
        public double ratedSpeed;
        public ILArray<double> Cp;
        public ILArray<double> Ct;
    }
}
