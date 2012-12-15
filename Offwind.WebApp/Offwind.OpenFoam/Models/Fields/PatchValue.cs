﻿namespace Offwind.Products.OpenFoam.Models.Fields
{
    public sealed class PatchValue
    {
        public PatchValueType Type { get; set; }
        public decimal SingleValue { get; set; }
        public string Data { get; set; }
    }
}
