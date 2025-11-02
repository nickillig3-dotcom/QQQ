using System.IO;
using System.IO.Compression;

namespace ShopSizerPro.Core.IO
{
    public static class Zipper
    {
        public static string CreateZipFromFolder(string folder, string zipPath)
        {
            if (!Directory.Exists(folder))
                throw new DirectoryNotFoundException(folder);
            if (File.Exists(zipPath))
                File.Delete(zipPath);

            ZipFile.CreateFromDirectory(folder, zipPath, CompressionLevel.Optimal, includeBaseDirectory: false);
            return zipPath;
        }
    }
}
