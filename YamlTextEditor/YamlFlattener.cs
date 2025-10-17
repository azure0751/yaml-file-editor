using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using YamlDotNet.RepresentationModel;

namespace YamlTextEditor
{
    internal class YamlFlattener
    {
        /// <summary>
        /// Recursively flattens a YAML file into colon-separated key paths with values.
        /// Example: scheduler:jobs:syncReports:command="python sync_reports.py"
        /// </summary>
        public static List<string> FlattenYaml(string yamlFilePath)
        {
            var result = new List<string>();

            using var reader = new StreamReader(yamlFilePath);
            var yaml = new YamlStream();
            yaml.Load(reader);

            if (yaml.Documents.Count == 0)
                return result;

            var root = (YamlMappingNode)yaml.Documents[0].RootNode;
            FlattenNode(root, "", result);

            return result;
        }

        

        public static List<string> FlattenYamlFromString(string yamlContent)
        {
            var result = new List<string>();

            if (string.IsNullOrWhiteSpace(yamlContent))
                return result;

            var yaml = new YamlStream();
            using var reader = new StringReader(yamlContent);
            yaml.Load(reader);

            if (yaml.Documents.Count == 0)
                return result;

            var root = yaml.Documents[0].RootNode as YamlMappingNode;
            if (root != null)
                FlattenNode(root, "", result);

            return result;
        }

        private static void FlattenNode(YamlNode node, string prefix, List<string> result)
        {
            if (node is YamlMappingNode mappingNode)
            {
                foreach (var entry in mappingNode.Children)
                {
                    var key = ((YamlScalarNode)entry.Key).Value;
                    string newPrefix = string.IsNullOrEmpty(prefix) ? key : $"{prefix}:{key}";
                    FlattenNode(entry.Value, newPrefix, result);
                }
            }
            else if (node is YamlSequenceNode sequenceNode)
            {
                int index = 0;
                foreach (var item in sequenceNode.Children)
                {
                    string newPrefix = $"{prefix}:{index}";
                    FlattenNode(item, newPrefix, result);
                    index++;
                }
            }
            else if (node is YamlScalarNode scalarNode)
            {
                string value = scalarNode.Value ?? "";
                string formattedValue = NeedsQuoting(value) ? $"\"{value}\"" : value;
                result.Add($"{prefix}={formattedValue}");
            }
        }

        private static bool NeedsQuoting(string value)
        {
            // Quote if contains spaces or special characters
            return value.Contains(" ") || value.Contains(":") || value.Contains("/") || value.Contains("@");
        }

    }
}
