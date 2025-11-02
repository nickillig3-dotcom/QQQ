using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShopSizerPro.Core.IO
{
    public static class FileScanner
    {
        private static readonly string[] Allowed = new[] { ".jpg", ".jpeg", ".png" };

        public static IReadOnlyList<string> GetImageFiles(string inputDir)
        {
            if (string.IsNullOrWhiteSpace(inputDir)) throw new ArgumentException(nameof(inputDir));
            if (!Directory.Exists(inputDir)) throw new DirectoryNotFoundException(inputDir);

            return Directory.EnumerateFiles(inputDir, "*.*", SearchOption.TopDirectoryOnly)
                .Where(p => Allowed.Contains(Path.GetExtension(p).ToLowerInvariant()))
                .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
