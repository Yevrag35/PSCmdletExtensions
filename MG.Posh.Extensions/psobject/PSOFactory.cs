﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;

namespace MG.Posh.Extensions
{
    public static class PSOFactory
    {

        public static PSObject CreateFromObject<T>(T obj, params Expression<Func<T, object>>[] memberExpressions)
        {
            var pso = new PSObject();
            pso.AddFromObject(obj, memberExpressions);
            return pso;
        }
        public static PSObject AddProperty(this PSObject pso, string propertyName, object propertyValue)
        {
            pso.Properties.Add(new PSNoteProperty(propertyName, propertyValue));
            return pso;
        }
    }
}
