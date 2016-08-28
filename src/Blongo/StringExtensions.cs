namespace Blongo
{
    public static class StringExtensions
    {
        public static string AsNullIfEmpty(this string extended)
        {
            if (string.IsNullOrEmpty(extended))
            {
                return null;
            }

            return extended;
        }

        public static string AsNullIfEmptyOrWhiteSpace(this string extended)
        {
            if (string.IsNullOrWhiteSpace(extended))
            {
                return null;
            }

            return extended;
        }
    }
}