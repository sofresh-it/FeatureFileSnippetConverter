using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace FeatureFileSnippetConverter
{
    public static class Extensions
    {
        public static string PrettyXml(this string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineOnAttributes = true
            };

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }

        public static string ToXmlAttributeString(this string text)
        {
            var attr = new XAttribute("x", text).ToString();
            var val = attr.Substring(2).Trim('\"');
            return val;
        }
    }
}
