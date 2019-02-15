using System.Collections.Generic;
using System.Linq;
using LuhisBanking.Services;
using OneOf;
using TrueLayerAccess.Dtos;

namespace LuhisBanking.Server
{
    public static class OneOfFunctions
    {
        public static IReadOnlyList<Error> ExtractErrors<T>(this IEnumerable<(Login, OneOf<T, Error>)> t)
        {
            return t.Where(a =>
            {
                var (_, x) = a;
                return x.IsT1;
            }).Select(a =>
            {
                var (_, x) = a;
                return x.AsT1;
            }).ToList();
        }
    }
}