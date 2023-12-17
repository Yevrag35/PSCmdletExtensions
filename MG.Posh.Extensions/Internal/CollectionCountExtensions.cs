using System.Collections;
using System.Collections.Generic;

namespace MG.Posh.Internal
{
    internal static class CollectionCountExtensions
    {
        internal static bool TryGetNonEnumeratedCount<T>(this IEnumerable<T> source, out int count)
        {
            Guard.NotNull(source, nameof(source));

            if (source is ICollection<T> tCollection)
            {
                count = tCollection.Count;
                return true;
            }
            else if (source is ICollection nonGenericCollection)
            {
                count = nonGenericCollection.Count;
                return true;
            }

            count = 0;
            return false;
        }
    }
}