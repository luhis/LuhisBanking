using System;

namespace TrueLayerAccess
{
    public static class Ensure
    {
        public static T NotNull<T>(this T t)
        {
            if (t == null)
            {
                throw new NullReferenceException("Cannot be null");
            }

            return t;
        }

        public static string NotNullOrEmpty(this string t)
        {
            t.NotNull();
            if (string.IsNullOrWhiteSpace(t))
            {
                throw new NullReferenceException("Cannot be empty");
            }

            return t;
        }
    }
}