using System;

namespace Schemata
{
    public static class ObjectExtension
    {
        public static T[] ToArray<T>(this object? @this) where T : notnull
        {
            switch (@this)
            {
                case null:
                    return Array.Empty<T>();

                case object[] objects:
                    return Array.ConvertAll(objects, (t) => (T)t);

                default:
                    return new T[] { (T)@this };
            }
        }

        public static bool TryCast<T>(this object? @this, out T? value)
        {
            if (@this is T result)
            {
                value = result;
                return true;
            }
            value = default;
            return false;
        }
    }
}
