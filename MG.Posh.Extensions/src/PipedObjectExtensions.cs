using System;
using System.Management.Automation;
using System.Reflection;

namespace MG.Posh.Extensions.Pipe
{
    public static class PipedObjectExtensions
    {
        private const BindingFlags NONPUBINST = BindingFlags.Instance | BindingFlags.NonPublic;
        private const string PIPE_PROPERTY = "CurrentPipelineObject";

        public static PSObject GetPipedObject<T>(this T cmdlet) where T : PSCmdlet
        {
            return PrivateGet(cmdlet);
        }
        public static bool TryGetPipedObject<T>(this T cmdlet, out PSObject pso) where T : PSCmdlet
        {
            pso = PrivateGet(cmdlet);
            return pso != null;
        }

        private static PSObject PrivateGet<T>(T cmdlet) where T : PSCmdlet
        {
            PropertyInfo pipePi = typeof(PSCmdlet).GetProperty(PIPE_PROPERTY, NONPUBINST);
            return pipePi?.GetValue(cmdlet) as PSObject;
        }
    }
}
