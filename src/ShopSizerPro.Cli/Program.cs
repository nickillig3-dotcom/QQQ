using System;
using System.IO;
using System.Linq;
using ShopSizerPro.Core.IO;
using ShopSizerPro.Core.Naming;

internal static class Program
{
    // Beispiel:
    // dotnet run --project .\src\ShopSizerPro.Cli\ShopSizerPro.Cli.csproj -- ^
    //   --input "C:\in" --output "C:\out" --prefix "shopname" --limit 10 --zip
    private static int Main(string[] args)
    {
        try
        {
            var (input, output, prefix, limit, makeZip, dryRun) = ParseArgs(args);
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(output))
            {
                PrintHelp();
                return 2;
            }

            Directory.CreateDirectory(output);
            var files = FileScanner.GetImageFiles(input);
            if (files.Count == 0)
            {
                Console.WriteLine("Keine Bilder im Eingabeordner gefunden (.jpg/.jpeg/.png).");
                return 0;
            }

            var take = limit.HasValue ? Math.Min(limit.Value, files.Count) : files.Count;
            Console.WriteLine($"Verarbeite {take} von {files.Count} Dateien ...");

            for (int i = 0; i < take; i++)
            {
                var src = files[i];
                var newName = SeoRenamer.BuildName(prefix, i + 1, Path.GetExtension(src));
                var dest = Path.Combine(output, newName);
                Console.WriteLine($"{i + 1:D4}: {Path.GetFileName(src)} -> {newName}");
                if (!dryRun)
                {
                    File.Copy(src, dest, overwrite: true);
                }
            }

            if (makeZip)
            {
                var zipPath = Path.Combine(Path.GetDirectoryName(output) ?? output, $"{Path.GetFileName(output)}.zip");
                Console.WriteLine($"ZIP wird erzeugt: {zipPath}");
                if (!dryRun)
                    Zipper.CreateZipFromFolder(output, zipPath);
            }

            Console.WriteLine("Fertig.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("FEHLER: " + ex.Message);
            return 1;
        }
    }

    private static (string input, string output, string prefix, int? limit, bool makeZip, bool dryRun) ParseArgs(string[] args)
    {
        string input = "";
        string output = "";
        string prefix = "shop";
        int? limit = 10;   // Free-Limit (später via Lizenz aufhebbar)
        bool makeZip = false;
        bool dryRun = false;

        for (int i = 0; i < args.Length; i++)
        {
            var a = args[i];
            switch (a)
            {
                case "--input":
                case "-i":
                    input = Next(args, ref i);
                    break;
                case "--output":
                case "-o":
                    output = Next(args, ref i);
                    break;
                case "--prefix":
                case "-p":
                    prefix = Next(args, ref i);
                    break;
                case "--limit":
                case "-l":
                    if (int.TryParse(Next(args, ref i), out var l) && l > 0) limit = l;
                    break;
                case "--zip":
                case "-z":
                    makeZip = true;
                    break;
                case "--dry-run":
                case "-n":
                    dryRun = true;
                    break;
                case "--help":
                case "-h":
                default:
                    break;
            }
        }

        return (input, output, prefix, limit, makeZip, dryRun);

        static string Next(string[] a, ref int i)
        {
            if (i + 1 >= a.Length) throw new ArgumentException($"Fehlender Wert für {a[i]}");
            i++;
            return a[i];
        }
    }

    private static void PrintHelp()
    {
        Console.WriteLine("ShopSizer Pro CLI (Skeleton)");
        Console.WriteLine("Parameter:");
        Console.WriteLine("  --input|-i   <Pfad>   Eingabeordner (Bilder .jpg/.jpeg/.png)");
        Console.WriteLine("  --output|-o  <Pfad>   Ausgabeordner (wird angelegt)");
        Console.WriteLine("  --prefix|-p  <Text>   SEO-Dateinamen-Präfix (Standard: shop)");
        Console.WriteLine("  --limit|-l   <Zahl>   Max. Anzahl Dateien (Standard: 10)");
        Console.WriteLine("  --zip|-z              ZIP am Ende erzeugen");
        Console.WriteLine("  --dry-run|-n          Nur anzeigen, nichts schreiben");
        Console.WriteLine();
        Console.WriteLine("Beispiel:");
        Console.WriteLine("  dotnet run --project .\\src\\ShopSizerPro.Cli\\ShopSizerPro.Cli.csproj -- \\");
        Console.WriteLine("    --input \"C:\\\\bilder\" --output \"C:\\\\out\" --prefix \"meinshop\" --limit 10 --zip");
    }
}
