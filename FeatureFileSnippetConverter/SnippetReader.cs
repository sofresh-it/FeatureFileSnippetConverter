using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureFileSnippetConverter;

public static class SnippetReader
{
    public static IEnumerable<VsSnippetCode> ReadJson(string snippet)
    {
        var json = JObject.Parse(snippet);
        var result = new List<VsSnippetCode>();
        foreach (var (_, value) in json)
        {
            if (value == null) continue;

            var prefix = (value["prefix"] as JArray)?.Select(x => x.Value<string>()).ToList();
            var body = (value["body"] as JArray)?.Select(x => x.Value<string>()).ToList();
            var description = value["description"]?.Value<string>();

            if (prefix == null) continue;

            foreach (var pr in prefix)
            {
                if (string.IsNullOrEmpty(pr)) continue;

                var vsc = new VsSnippetCode
                {
                    Prefix = pr,
                    Body = body?.Aggregate((a, b) => a + Environment.NewLine + b),
                    Description = description
                };
                result.Add(vsc);
            }
        }
        return result;
    }
}