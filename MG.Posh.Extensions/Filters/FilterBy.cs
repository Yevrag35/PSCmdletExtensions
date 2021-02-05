using MG.Posh.Extensions.Bound;
using MG.Posh.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions.Filters
{
    public static class FilterByExtensions
    {
        /// <summary>
        /// Filters an <see cref="IEnumerable{TOutput}"/> with the given where clause.
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="filterThis">The collection to filter.</param>
        /// <param name="runOnlyIf">Indicates whether to actually perform the 
        /// filter.  Useful in chaining these statements.</param>
        /// <param name="whereClause">The predicate used to filter the collection.</param>
        /// <returns>A filtered <see cref="IEnumerable{TOutput}"/>.</returns>
        public static IEnumerable<TOutput> ThenFilterBy<TOutput>(this IEnumerable<TOutput> filterThis,
            bool runOnlyIf, Func<TOutput, bool> whereClause)
        {
            if (!runOnlyIf || whereClause == null)
                return filterThis;

            return filterThis?.Where(whereClause);
        }

        /// <summary>
        /// Filters an <see cref="IEnumerable{TOutput}"/> with the given where clause.
        /// </summary>
        /// <remarks>Using the specified <see cref="PSCmdlet"/> and expression, 
        /// it is determined first if the parameter was bound prior to filtering the collection.</remarks>
        /// <typeparam name="TOutput"></typeparam>
        /// <typeparam name="TCmdlet"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="filterThis">The collection to filter.</param>
        /// <param name="cmdlet">The <see cref="PSCmdlet"/> who bound parameters are checked.</param>
        /// <param name="cmdletParameter">The bound parameter expression to check.</param>
        /// <param name="runOnlyIf">Indicates whether to actually perform the 
        /// filter.  Useful in chaining these statements.</param>
        /// <param name="whereClause">The predicate used to filter the collection.</param>
        /// <returns>A filtered <see cref="IEnumerable{TOutput}"/>.</returns>
        public static IEnumerable<TOutput> ThenFilterBy<TCmdlet, TParameter, TOutput>(
            this IEnumerable<TOutput> filterThis, TCmdlet cmdlet, Expression<Func<TCmdlet, TParameter>> cmdletParameter,
            bool runOnlyIf, Func<TOutput, bool> whereClause) where TCmdlet : PSCmdlet
        {
            if (!cmdlet.ContainsParameter(cmdletParameter)
                ||
                !runOnlyIf
                ||
                whereClause == null)
            {
                return filterThis;
            }

            return filterThis.Where(whereClause);
        }

        /// <summary>
        /// Filters the given collection of strings and returns all which match any of the specified
        /// wildcard strings.
        /// </summary>
        /// <param name="itemCollection">The collection of strings to filter.</param>
        /// <param name="wildcardStrings">The collection of wildcard strings that will filter
        /// the incoming string collection.</param>
        public static IEnumerable<string> FilterByWildcards(
            this IEnumerable<string> itemCollection, IEnumerable<string> wildcardStrings)
        {
            if (itemCollection == null || wildcardStrings == null)
                return itemCollection;

            IEnumerable<WildcardPattern> patterns = MakePatterns(wildcardStrings);
            return itemCollection
                .Where(i =>
                    patterns
                        .Any(pat =>
                            pat.IsMatch(i)));
        }
        public static IEnumerable<T> FilterByWildcards<T>(
            this IEnumerable<T> itemCollection, IEnumerable<string> wildcardStrings)
            where T : IConvertible
        {
            if (itemCollection == null || wildcardStrings == null)
                return itemCollection;

            IEnumerable<WildcardPattern> patterns = MakePatterns(wildcardStrings);
            return itemCollection
                .Where(i =>
                    patterns
                        .Any(
                            pat =>
                                pat.IsMatch(
                                    Convert.ToString(i))));
        }
        public static IEnumerable<T> FilterByWildcards<T>(
            this IEnumerable<T> itemCollection, IEnumerable<string> wildcardStrings,
            Func<T, IConvertible> valueToFilter)
        {
            if (itemCollection == null || wildcardStrings == null)
                return itemCollection;

            IEnumerable<WildcardPattern> patterns = MakePatterns(wildcardStrings);

            return itemCollection
                .Where(i => 
                    patterns
                        .Any(
                            pat => 
                                pat.IsMatch(
                                    Convert.ToString(
                                        valueToFilter(i)))));
        }

        public static IEnumerable<T> FilterByWildcards<T, TProp>(
            this IEnumerable<T> itemCollection, IEnumerable<string> wildcardStrings,
            Func<T, TProp[]> arrayPropertyToFilter)
            where TProp : IConvertible
        {
            if (itemCollection == null || wildcardStrings == null)
                return itemCollection;

            IEnumerable<WildcardPattern> patterns = MakePatterns(wildcardStrings);

            return itemCollection
                .Where(i =>
                    patterns
                        .Any(
                            pat =>
                                (arrayPropertyToFilter(i)?.Any(
                                    ap =>
                                        pat.IsMatch(Convert.ToString(ap))))
                                            .GetValueOrDefault()));
        }


        public static IEnumerable<T> FilterManyByWildcards<T>(
            this IEnumerable<T> itemCollection, IEnumerable<string> wildcardStrings,
            params Func<T, IConvertible>[] valuesToFilter)
        {
            if (
                itemCollection == null
                || wildcardStrings == null
                || valuesToFilter == null
                || valuesToFilter.Length <= 0
            )
            {
                return itemCollection;
            }

            IEnumerable<WildcardPattern> patterns = MakePatterns(wildcardStrings);

            return itemCollection
                .Where(x =>
                    valuesToFilter
                        .Any(pf =>
                            patterns
                                .Any(pat =>
                                    pat.IsMatch(
                                        Convert.ToString(
                                            pf(x))))));
        }

        

        private static IEnumerable<WildcardPattern> MakePatterns(IEnumerable<string> strings) =>
            strings
                .Select(x => 
                    new WildcardPattern(
                        x, WildcardOptions.IgnoreCase));
    }
}
