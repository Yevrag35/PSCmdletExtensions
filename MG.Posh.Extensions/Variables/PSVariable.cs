using MG.Posh.Internal.Attributes;

namespace MG.Posh
{
    [BuiltInParameterClass]
    public static class PSDefaultParameterNames
    {
        [BuiltInParameter]
        public static readonly string Debug = "Debug";
        [BuiltInParameter]
        public static readonly string ErrorAction = "ErrorAction";
        [BuiltInParameter]
        public static readonly string ErrorVariable = "ErrorVariable";
        [BuiltInParameter]
        public static readonly string InformationAction = "InformationAction";
        [BuiltInParameter]
        public static readonly string InformationVariable = "InformationVariable";
        [BuiltInParameter]
        public static readonly string OutBuffer = "OutBuffer";
        [BuiltInParameter]
        public static readonly string OutVariable = "OutVariable";
        [BuiltInParameter]
        public static readonly string PipelineVariable = "PipelineVariable";
        [BuiltInParameter]
        public static readonly string Verbose = "Verbose";
        [BuiltInParameter]
        public static readonly string WarningAction = "WarningAction";
        [BuiltInParameter]
        public static readonly string WarningVariable = "WarningVariable";
    }
}