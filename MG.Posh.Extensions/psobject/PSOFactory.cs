using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions
{
    public static class PSOFactory
    {

        public static PSObject CreateFromObject<T1, T2>(T1 obj, params Expression<Func<T1, T2>>[] memberExpressions)
        {
            var pso = new PSObject();
            pso.AddFromObject(obj, memberExpressions);
            return pso;
        }
    }
}
