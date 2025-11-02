using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ShopSizerPro.Core.Naming
{
    public static class SeoRenamer
    {
        public static string BuildName(string prefix, int index, string originalExtension)
        {
            if (string.IsNullOrWhiteSpace(prefix)) prefix = "item";
            var ext = originalExtension?.Trim() ?? ".jpg";
            if (!ext.StartsWith(".", StringComparison.Ordinal)) ext = "." + ext;
            ext = ext.ToLowerInvariant();
            var num = index.ToString("D4");
            return $"{Sanitize(prefix)}-{num}{ext}";
        }

        private static string Sanitize(string input)
        {
            if (string.IsNullOrEmpty(input)) return "item";
            var invalid = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(input.Length);
            foreach (var ch in input)
                sb.Append(invalid.Contains(ch) ? '-' : char.ToLowerInvariant(ch));
            var s = sb.ToString().Trim('-');
            return string.IsNullOrEmpty(s) ? "item" : s;
        }
    }
}
