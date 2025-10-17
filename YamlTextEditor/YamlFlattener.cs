using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace YamlTextEditor
{
    internal class YamlFlattener
    {
        /// <summary>
        /// Flattens a YAML file and separates duplicates into another collection.
        /// </summary>
        public static void FlattenYaml(string yamlFilePath, out List<string> uniqueRecords, out List<string> duplicateRecords)
        {
            using var reader = new StreamReader(yamlFilePath);
            string yamlContent = reader.ReadToEnd();
            FlattenYamlFromString(yamlContent, out uniqueRecords, out duplicateRecords);
        }

        /// <summary>
        /// Flattens a YAML string into colon-separated key paths with values.
        /// Duplicates are separated into a different collection.
        /// </summary>
        public static void FlattenYamlFromString(string yamlContent, out List<string> uniqueRecords, out List<string> duplicateRecords)
        {
            uniqueRecords = new List<string>();
            duplicateRecords = new List<string>();

            if (string.IsNullOrWhiteSpace(yamlContent))
                return;

            var yaml = new YamlStream();
            using var reader = new StringReader(yamlContent);
            yaml.Load(reader);

            if (yaml.Documents.Count == 0)
                return;

            var root = yaml.Documents[0].RootNode as YamlMappingNode;
            if (root == null)
                return;

            var seenKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            FlattenNode(root, "", uniqueRecords, duplicateRecords, seenKeys);
        }

        /// <summary>
        /// Recursive node traversal for flattening YAML and detecting duplicates.
        /// </summary>
        private static void FlattenNode(YamlNode node, string prefix, List<string> uniqueRecords, List<string> duplicateRecords, HashSet<string> seenKeys)
        {
            if (node is YamlMappingNode mappingNode)
            {
                foreach (var entry in mappingNode.Children)
                {
                    var key = ((YamlScalarNode)entry.Key).Value;
                    string newPrefix = string.IsNullOrEmpty(prefix) ? key : $"{prefix}:{key}";
                    FlattenNode(entry.Value, newPrefix, uniqueRecords, duplicateRecords, seenKeys);
                }
            }
            else if (node is YamlSequenceNode sequenceNode)
            {
                int index = 0;
                foreach (var item in sequenceNode.Children)
                {
                    string newPrefix = $"{prefix}:{index}";
                    FlattenNode(item, newPrefix, uniqueRecords, duplicateRecords, seenKeys);
                    index++;
                }
            }
            else if (node is YamlScalarNode scalarNode)
            {
                string value = scalarNode.Value ?? "";
                string formattedValue = NeedsQuoting(value) ? $"\"{value}\"" : value;
                string entry = $"{prefix}={formattedValue}";

                if (!seenKeys.Add(prefix))
                    duplicateRecords.Add(entry);
                else
                    uniqueRecords.Add(entry);
            }
        }

        /// <summary>
        /// Determines whether the value should be enclosed in quotes.
        /// </summary>
        private static bool NeedsQuoting(string value)
        {
            return value.Contains(" ") || value.Contains(":") || value.Contains("/") || value.Contains("@");
        }
    }
}
