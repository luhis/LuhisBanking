﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace LuhisBanking.Persistence
{
    public static class AsyncTools
    {
        public static async Task<IReadOnlyList<T>> ToReadOnlyAsync<T>(this DbSet<T> set, CancellationToken cancellationToken) where T : class
        {
            return await set.ToListAsync(cancellationToken);
        }

        public static async Task<Option<T>> SingleOrNone<T>(this DbSet<T> set, Expression<Func<T, bool>> f, CancellationToken cancellationToken) where T : class
        {
            var r = await set.Where(f).SingleOrDefaultAsync(cancellationToken);
            return r == null ? Option.None<T>() : Option.Some(r);
        }
    }
}