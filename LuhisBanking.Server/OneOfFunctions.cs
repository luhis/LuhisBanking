using System.Collections.Generic;
using System.Linq;
using OneOf;
using TrueLayerAccess.Dtos;

namespace LuhisBanking.Server
{
    public static class OneOfFunctions
    {
        public static IReadOnlyList<Error> ExtractErrors<T>(this IEnumerable<OneOf<T, Error>> t)
        {
            return t.Where(a => a.IsT1).Select(a => a.AsT1).ToList();
        }
    }
}