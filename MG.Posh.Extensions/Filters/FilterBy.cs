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
        public static IEnumerable<TOutput> ThenFilterBy<TCmdlet, TParameter, TOutput>(
            this IEnumerable<TOutput> filterThis, TCmdlet cmdlet, Expression<Func<TCmdlet, TParameter>> cmdletParameter,
            Func<TCmdlet, bool> condition, Func<TOutput, bool> whereClause) where TCmdlet : PSCmdlet
        {
            if (!cmdlet.ContainsParameter(cmdletParameter)
                ||
                condition == null
                ||
                !condition(cmdlet)
                ||
                whereClause == null)
            {
                return filterThis;
            }

            return filterThis.Where(whereClause);
        }

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
            Func<T, IConvertible> propertyFunc)
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
                                        propertyFunc(i)))));
        }

        public static IEnumerable<T> FilterManyByWildcards<T>(
            this IEnumerable<T> itemCollection, IEnumerable<string> wildcardStrings,
            params Func<T, IConvertible>[] propertyFuncs)
        {
            if (
                itemCollection == null
                || wildcardStrings == null
                || propertyFuncs == null
                || propertyFuncs.Length <= 0
            )
            {
                return itemCollection;
            }

            IEnumerable<WildcardPattern> patterns = MakePatterns(wildcardStrings);

            return itemCollection
                .Where(x =>
                    propertyFuncs
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
