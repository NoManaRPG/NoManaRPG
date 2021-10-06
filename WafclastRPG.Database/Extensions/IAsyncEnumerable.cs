// This file is part of the WafclastRPG project.

using System.Collections.Generic;
using MongoDB.Driver;

namespace WafclastRPG.Database.Extensions
{
    public static class IAsyncEnumerable
    {
        public static async IAsyncEnumerable<T> ToEnumerableAsync<T>(this IAsyncCursor<T> asyncCursor)
        {
            using (asyncCursor)
                while (await asyncCursor.MoveNextAsync())
                    foreach (var current in asyncCursor.Current)
                        yield return current;
        }
    }
}
