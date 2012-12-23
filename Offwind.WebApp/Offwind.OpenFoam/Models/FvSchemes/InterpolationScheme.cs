namespace Offwind.Sowfa.System.FvSchemes
{
    public enum BoundView
    {
        None, Range, Name
    }

    public sealed class InterpolationScheme : SchemeHeader
    {
        public InterpolationType interpolation { set; get; }
        public SchemeBound bound { set; get; }
        public string flux { set; get; }

        public InterpolationScheme()
        {
            interpolation = InterpolationType.linear;
            bound = new SchemeBound();
            flux = null;
        }

        public static bool StrictlyBoundedField(InterpolationType x)
        {
            if ((x == InterpolationType.limitedLinear)
                || (x == InterpolationType.vanLeer)
                || (x == InterpolationType.Gamma)
                || (x == InterpolationType.limitedCubic)
                || (x == InterpolationType.MUSCL)
                ) return true;
            return false;
        }

        public static bool AllowedLimitedPrefix(InterpolationType x)
        {
            if ((x == InterpolationType.limitedCubic) ||
                (x == InterpolationType.limitedLinear))
                return false;
            return true;
        }
    }
}
