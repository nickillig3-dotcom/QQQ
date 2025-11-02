using System;
using System.IO;
using ShopSizerPro.Core.IO;
using Xunit;

namespace ShopSizerPro.Core.Tests;

public class FileScannerTests
{
    [Fact]
    public void Scans_only_images()
    {
        var dir = Path.Combine(Path.GetTempPath(), "ssp-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            File.WriteAllBytes(Path.Combine(dir, "a.jpg"), new byte[] { 1, 2, 3 });
            File.WriteAllBytes(Path.Combine(dir, "b.png"), new byte[] { 1, 2, 3 });
            File.WriteAllText(Path.Combine(dir, "c.txt"), "x");

            var list = FileScanner.GetImageFiles(dir);
            Assert.Equal(2, list.Count);
            Assert.Contains(list, p => p.EndsWith("a.jpg", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(list, p => p.EndsWith("b.png", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            Directory.Delete(dir, recursive: true);
        }
    }
}
