using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FeatureFileSnippetConverter
{
    public static class SnippetWriter
    {
        public static string ConvertToWebStormSnippet(IEnumerable<VsSnippetCode> snippets, string groupName,
            string snippetType)
        {
            if (snippets == null) throw new ArgumentNullException(nameof(snippets));

            var sb = new StringBuilder();
            sb.AppendLine($"<templateSet group=\"{groupName}\">");
            foreach (var snippet in snippets)
            {
                var val = GetWebStormTemplateValue(snippet.Body, out var vars);
                sb.AppendLine(
                    $"<template name=\"rin\" value=\"{val}\" description=\"{snippet.Description}\" toReformat=\"false\" toShortenFQNames=\"true\">");

                if (vars != null)
                    foreach (var item in vars)
                        sb.AppendLine($"<variable name=\"{item}\" expression=\"\" defaultValue=\"\" alwaysStopAt=\"true\" />");

                sb.AppendLine($"<context><option name=\"{snippetType}\" value=\"true\"/></context>");
                sb.AppendLine("</template>");
            }
            sb.AppendLine("</templateSet>");

            return sb.ToString().PrettyXml();
        }

        private static string GetWebStormTemplateValue(string template, out string[] vars)
        {
            var matches = new Regex(@"\$(\d)|\$\{(\d).+?\}").Matches(template);
            var variables = new List<string>();
            foreach (Match match in matches)
            {
                var val = !string.IsNullOrEmpty(match.Groups[1].Value)
                    ? match.Groups[1].Value
                    : match.Groups[2].Value;
                variables.Add(val);
                val = Convert.ToInt32(val) == 0 ? "$END$" : $"${val}$";
                template = template.Replace(match.Value, val);
            }
            vars = variables.ToArray();
            template = template.ToXmlAttributeString().Replace("&#xD;&#xA;", "&#10;");
            return template;
        }
    }
}
