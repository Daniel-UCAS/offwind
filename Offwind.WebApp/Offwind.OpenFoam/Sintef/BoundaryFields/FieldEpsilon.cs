using Offwind.OpenFoam.Models.Fields;
using Offwind.Products.OpenFoam.Models.Fields;

namespace Offwind.OpenFoam.Sintef.BoundaryFields
{
    public class EpsilonWallFunction
    {
        public decimal Cmu { get; set; }
        public decimal kappa { get; set; }
        public decimal E { get; set; }
        public PatchValueScalar value { get; set; }
        public EpsilonWallFunction()
        {
            value = new PatchValueScalar();
        }
    }

    public class FieldEpsilon
    {
        public decimal InternalField { get; set; }

        public PatchType BottomType { get; set; }
        public EpsilonWallFunction BottomValue { get; set; }
               
        public PatchType TopType { get; set; }
        public EpsilonWallFunction TopValue { get; set; }

        public PatchType WestType { get; set; }
        public PatchValueScalar WestValue { get; set; }

        public PatchType EastType { get; set; }
        //public PatchValueScalar EastValue { get; set; }

        public PatchType NorthType { get; set; }
        //public PatchValueScalar NorthValue { get; set; }

        public PatchType SouthType { get; set; }
        public PatchValueScalar SouthValue { get; set; }

        public FieldEpsilon()
        {
            BottomValue = new EpsilonWallFunction();
            TopValue = new EpsilonWallFunction();
            WestValue = new PatchValueScalar();
            //EastValue = new PatchValueScalar();
            //NorthValue = new PatchValueScalar();
            SouthValue = new PatchValueScalar();
        }
    }
}
