﻿using System;
using ILNumerics;
using MatlabInterpreter;

namespace WakeFarmControlR
{
    internal partial class TranslatedCode
    {
        #region "Original function comments"
        // Wake Simulation
        // (C) Rasmus Christensen
        // Control and Automation, Aalborg University 2014

        // Compute the velocity in front of each wind-turbine, with respect to the wind input.
        #endregion
        internal static void Compute_Vell(out double[,] vel_output, ILArray<double> yTurb, ILArray<int> xTurbC, ILArray<int> yTurbC, ILArray<double> x, ILArray<double> wField, double[] Uhub, double kWake, int iMax, int jMax, int nTurb, double dTurb, ILArray<double> Ct, double dy)
        {
            #region "Used variables declaration"
            double[,] vell_i;
            ILArray<double> shadow;
            double r0;
            double nk;
            int k;
            int J;
            double SS;
            double SS0;
            int i;
            double RR_i;
            double Dij;
            double Alpha_i;
            double Alpha_k;
            double Area;
            int ii;
            double rrt;
            double nj;
            int jjMin;
            int jjMax;
            #endregion


            //vell_i = wField.C;
            vell_i = new double[wField.Size[0], wField.Size[1]];
            for (var i0 = 0; i0 < vell_i.GetLength(0); i0++)
            {
                for (var i1 = 0; i1 < vell_i.GetLength(1); i1++)
                {
                    vell_i[i0, i1] = wField.GetValue(i0, i1);
                }
            }

            shadow = zeros(1, nTurb);

            r0 = 0.5 * dTurb;
            nk = 2 * ceil(dTurb / dy);

            for (k = 1; k <= nTurb; k++)
            {
                J = 0;
                SS = 0;
                SS0 = pi * r0 * r0;

                for (i = 1; i <= k - 1; i++)
                {
                    RR_i = r0 + kWake * (x._(xTurbC._(k)) - x._(xTurbC._(i)));
                    Dij = abs(yTurb._(i) - yTurb._(k));

                    if ((RR_i >= (r0 + Dij)) || (Dij <= dy))
                    {
                        SS = SS + ((r0 * r0) / (RR_i * RR_i));
                    }
                    else
                    {
                        if (RR_i >= (r0 + Dij) && Dij > dy)
                        {
                            J               = J + 1;
                            Alpha_i         = acos((RR_i * RR_i) + (Dij * Dij) - (r0 * r0) / (2 * RR_i * Dij));
                            Alpha_k         = acos(((r0 * r0) + (Dij * Dij) - (RR_i * RR_i)) / (2 * r0 * Dij));
                            AArea(out Area, RR_i, r0, Dij);
                            shadow._(J,     '=', (Alpha_i * (RR_i * RR_i) + Alpha_k * (r0 * r0)) - 2 * Area);
                            SS              = SS + ((shadow._(J)) / SS0) * ((r0 * r0) / (RR_i * RR_i));
                        }
                    }
                }

                for (ii = xTurbC._(k); ii <= iMax; ii++)
                {
                    rrt     = r0 + kWake * (x._(ii) - x._(xTurbC._(k)));
                    nj      = ceil(rrt / dy);

                    jjMin   = floor_(max(1, yTurbC._(k) - nj));
                    jjMax   = ceil_(min(jMax, yTurbC._(k) + nj));

                    for (var j = jjMin; j <= jjMax; j++)
                    {
                        if (((-vell_i[ii - 1, j - 1] + Uhub[k - 1]) > 0) && (ii > xTurbC._(k) + nk))
                        {
                            vell_i[ii - 1, j - 1] = min(vell_i[ii - 2, j - 1], Uhub[k - 1] + Uhub[k - 1] * (sqrt(1 - Ct._(k)) - 1) * ((r0 * r0) / (rrt * rrt)) * (1 - (1 - sqrt(1 - Ct._(k))) * SS));
                            vell_i[ii - 1, j - 1] = max(0, vell_i[ii - 1, j - 1]);
                        }
                        else
                        {
                            vell_i[ii - 1, j - 1] = (Uhub[k - 1] + Uhub[k - 1] * (sqrt(1 - Ct._(k)) - 1) * (r0 / rrt) * (r0 / rrt)) * (1 - (1 - sqrt(1 - Ct._(k))) * SS);
                            vell_i[ii - 1, j - 1] = max(0, vell_i[ii - 1, j - 1]);
                        }
                    }
                }
            }

            vel_output = vell_i;
        }

        public static void AArea(out double area_out, double x, double y, double z) // Internal function to compute the shadowed area.
        {
            #region "Used variables declaration"
            double PP;
            #endregion

            PP = (x + y + z) * 0.5;
            area_out = sqrt(PP * (PP - x) * (PP - y) * (PP - z));
        }
    }
}
