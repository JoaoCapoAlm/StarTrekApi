using System.ComponentModel;

namespace CrossCutting.Enums
{
    public enum QuadrantEnum : byte
    {
        [Description("QuadrantAlphaResource")]
        Alpha = 1,
        [Description("QuadrantBetaResource")]
        Beta = 2,
        [Description("QuadrantGammaResource")]
        Gamma = 3,
        [Description("QuadrantDeltaResource")]
        Delta = 4
    }
}
