using System;
using System.Management.Automation;
using System.Reflection;

namespace MG.Posh.Extensions.Pipe
{
    /// <summary>
    ///     An extension class offering methods for retrieving piped objects if the extending <see cref="PSCmdlet"/>
    ///     is the recipient in the pipeline.
    /// </summary>
    public static class PipedObjectExtensions
    {
        private const BindingFlags NONPUBINST = BindingFlags.Instance | BindingFlags.NonPublic;
        private const string PIPE_PROPERTY = "CurrentPipelineObject";
        private const string IMMEDIATE_EMPTY = "immediateBaseObjectIsEmpty";

        /// <summary>
        ///     Retrieves the object sent to this <see cref="PSCmdlet"/> from the pipeline.  If no object exists,
        ///     a <see langword="null"/> value is returned.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The extending <see cref="PSCmdlet"/>.</param>
        /// <returns>
        ///     The <see cref="PSObject"/> instance sent through the pipeline to the extended <see cref="PSCmdlet"/>.
        /// </returns>
        public static PSObject GetPipedObject<T>(this T cmdlet) where T : PSCmdlet
        {
            return PrivateGet(cmdlet);
        }

        /// <summary>
        ///     Indicates whether the <see cref="PSCmdlet"/> is receiving an object from the pipeline.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The extending <see cref="PSCmdlet"/>.</param>
        /// <returns>
        ///     Returns <see langword="true"/> if the piped object is exists and <see langword="false"/> if the value is <see langword="null"/>.
        /// </returns>
        public static bool HasPipedObject<T>(this T cmdlet) where T : PSCmdlet
        {
            object obj = GetPipedObjectProperty()?.GetValue(cmdlet);
            FieldInfo empty = GetEmptyField();
            return empty.GetValue(obj) is bool answer && !answer;
        }

        /// <summary>
        ///     Tries to return the object sent to this <see cref="PSCmdlet"/> from the pipeline setting an out variable if
        ///     one exists.
        ///     A <see langword="true"/> value indicates there was an object received.
        ///     A <see langword="false"/> value indicates no object was found or the cmdlet was first in the pipeline.
        /// </summary>
        /// <typeparam name="T">The type of the inheriting <see cref="PSCmdlet"/>.</typeparam>
        /// <param name="cmdlet">The extending <see cref="PSCmdlet"/>.</param>
        /// <param name="pso">The resulting object found from the pipeline.</param>
        /// <returns>
        ///     Returns a <see langword="true" /> value if a piped object was retrieved.
        ///     Returns a <see langword="false"/> value if the resulting piped object was <see langword="null"/>.
        /// </returns>
        public static bool TryGetPipedObject<T>(this T cmdlet, out PSObject pso) where T : PSCmdlet
        {
            pso = PrivateGet(cmdlet);
            FieldInfo empty = GetEmptyField();
            return empty.GetValue(pso) is bool answer && !answer;
        }


        #region BACKEND METHODS
        private static PropertyInfo GetPipedObjectProperty() => typeof(PSCmdlet).GetProperty(PIPE_PROPERTY, NONPUBINST);
        private static FieldInfo GetEmptyField() => typeof(PSObject).GetField(IMMEDIATE_EMPTY, NONPUBINST);
        private static PSObject PrivateGet<T>(T cmdlet) where T : PSCmdlet
        {
            return GetPipedObjectProperty()?.GetValue(cmdlet) as PSObject;
        }

        #endregion
    }
}
