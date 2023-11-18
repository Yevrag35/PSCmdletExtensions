using MG.Posh.Internal;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace MG.Posh.Extensions.PSPaths
{
    public static class PathResolutionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdlet"></param>
        /// <param name="path"></param>
        /// <param name="providerInfo"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(path))]
        public static string? GetResolvedPath(this PSCmdlet cmdlet, string? path, out ProviderInfo? providerInfo)
        {
            Guard.NotNull(cmdlet, nameof(cmdlet));

            if (string.IsNullOrWhiteSpace(path))
            {
                providerInfo = null;
                return path;
            }

            var collection = cmdlet.GetResolvedProviderPathFromPSPath(path, out providerInfo);
            if (collection.Count > 0)
            {
                return collection[0];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

