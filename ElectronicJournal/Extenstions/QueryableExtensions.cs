using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ElectronicJournal.Extenstions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool select, Expression<Func<T, bool>> predicate)
        {
            if (select)
            {
                query = query.Where(predicate);
            }
            return query;
        }
    }
}
