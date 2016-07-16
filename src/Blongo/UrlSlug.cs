using System.Text;
using System.Text.RegularExpressions;

namespace Blongo
{
    public class UrlSlug
    {
        public UrlSlug(string value)
        {
            Value = ConvertToUrlSlug(value);
        }

        public static string ConvertToUrlSlug(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var urlSlug = value;
            urlSlug = RemoveAccents(urlSlug);
            urlSlug = Regex.Replace(urlSlug, @"[\s_~]", "-", RegexOptions.Compiled);
            urlSlug = Regex.Replace(urlSlug, @"[^A-Za-z0-9-'.!()[\]*<>]", "", RegexOptions.Compiled);
            urlSlug = Regex.Replace(urlSlug, @"[-]{2,}", "-", RegexOptions.Compiled);
            urlSlug = urlSlug.Trim('-');
            urlSlug = urlSlug.ToLowerInvariant();

            return urlSlug;
        }

        private static string RemoveAccents(string value)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            return Encoding.ASCII.GetString(bytes);
        }

        public string Value { get; }
    }
}