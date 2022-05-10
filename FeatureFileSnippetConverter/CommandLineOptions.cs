using CommandLine;

namespace FeatureFileSnippetConverter;

public class CommandLineOptions
{
    [Option('s', "snippet", Required = true, HelpText = "VS Code snippet.")]
    public string Snippet { get; set; }

    [Option('o', "output", Required = true, HelpText = "Output directory.")]
    public string Output { get; set; }

    [Option('t', "type", Required = false, HelpText = "WebStorm snippet type.",Default = "CUCUMBER_FEATURE_FILE")]
    public string SnippetType { get; set; }
    [Option('g', "group", Required = false, HelpText = "WebStorm snippet group name.", Default = "Ringana")]
    public string GroupName { get; set; }
}