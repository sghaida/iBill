namespace Lync2013Plugin.Implementation
{
    public static class ExtensionMethods
    {
        public static bool In<T>(this T x, params T[] values)
        {
            foreach (var value in values)
            {
                if (x.Equals(value))
                    return true;
            }

            return false;
        }
    }
}