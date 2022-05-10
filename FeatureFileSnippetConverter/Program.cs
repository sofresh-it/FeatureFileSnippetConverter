using System.IO;
using System.IO.Compression;
using CommandLine;

namespace FeatureFileSnippetConverter
{
    public class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(o =>
                {
                    var json = SnippetReader.ReadJson(File.ReadAllText(o.Snippet));
                    var result = SnippetWriter.ConvertToWebStormSnippet(json, o.GroupName, o.SnippetType);
                    using var zipFile = File.Open(Path.Combine(o.Output, "WebStormSnippets.zip"), FileMode.Create);
                    using var archive = new ZipArchive(zipFile, ZipArchiveMode.Create);
                    var zipEntry = archive.CreateEntry(Path.Combine("templates", o.GroupName + ".xml"));
                    using var writer = new StreamWriter(zipEntry.Open());
                    writer.WriteLine(result);
                });
        }
    }
}
