using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YamlConsoleEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"=== YAML Console Editor (Config File Support, Preserves Comments) === {System.Environment.NewLine}{System.Environment.NewLine}By Abhishek Shukla Abhishek.shukla6@ibm.com");

            Console.Write($"{System.Environment.NewLine}");
            // Ask for YAML file path
            Console.Write("Enter path to YAML file: ");
            string yamlFilePath = Console.ReadLine()?.Trim().Trim('"');
            if (!File.Exists(yamlFilePath.Trim().Trim('"')))
            {
                Console.WriteLine("YAML file not found!");
                return;
            }

            // Ask for config file path
            Console.Write("Enter path to config file: ");
            string configFilePath = Console.ReadLine()?.Trim().Trim('"');
            if (!File.Exists(configFilePath.Trim().Trim('"')))
            {
                Console.WriteLine("Config file not found!");
                return;
            }

            // Load YAML lines
            var yamlLines = File.ReadAllLines(yamlFilePath).ToList();

            // Load config map from file
            var configMap = LoadConfigFile(configFilePath);
            Console.WriteLine($"Loaded {configMap.Count} config entries from file.");

            foreach (var config in configMap)
            {
               Console.Write($"{config.Key} = {config.Value}{System.Environment.NewLine}");
            }

            Console.WriteLine(System.Environment.NewLine);
            // Apply updates
            foreach (var kv in configMap)
            {
                string[] pathParts = kv.Key.Split(':');
                yamlLines = UpdateYamlText(yamlLines, pathParts, kv.Value);
                Console.WriteLine($"Following config entry done : {kv}");
            }

            // Save updated YAML
            string outputPath = Path.Combine(
                Path.GetDirectoryName(yamlFilePath)!,
                Path.GetFileNameWithoutExtension(yamlFilePath) + "_updated.yaml"
            );
            File.WriteAllLines(outputPath, yamlLines);

            Console.WriteLine($"\n Updated YAML saved to: {outputPath}");
            Console.WriteLine("Press Any key to Close this Application");
        }

        // ---------------- Load Config File ----------------

        static Dictionary<string, string> LoadConfigFile(string configFilePath)
        {
            var config = new Dictionary<string, string>();

            foreach (var line in File.ReadAllLines(configFilePath))
            {
                var trimmed = line.Trim();

                // Skip comments and blank lines
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                    continue;

                // Split into key=value
                var parts = trimmed.Split('=', 2);
                if (parts.Length != 2)
                    continue;

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                // Remove extra commas at start (in case of accidental ", value")
                if (value.StartsWith(","))
                    value = value.TrimStart(',');

                // Remove quotes only if BOTH start and end have them
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    config[key] = $"\"{value.Trim('"')}\""; // keep quotes in YAML
                }
                else
                {
                    config[key] = value; // unquoted literal (true, false, 123, etc.)
                }
            }

            return config;
        }

        static Dictionary<string, string> LoadConfigFile_old(string filePath)
        {
            var configMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var line in File.ReadAllLines(filePath))
            {
                string trimmed = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                    continue;

                // Expecting: key=value
                int eq = trimmed.IndexOf('=');
                if (eq <= 0)
                    continue;

                string key = trimmed.Substring(0, eq).Trim();
                string value = trimmed.Substring(eq + 1).Trim();

                // Remove surrounding quotes if present
                if (value.StartsWith("\"") && value.EndsWith("\""))
                    value = value.Substring(1, value.Length - 2);

                configMap[key] = value;
            }
            return configMap;
        }

        // ---------------- Update YAML Text ----------------

        static List<string> UpdateYamlText(List<string> lines, string[] pathParts, string newValue)
        {
            int currentLevel = 0;
            int expectedIndent = 0;

            // Decide whether to quote the value
            bool isBoolean = newValue.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                             newValue.Equals("false", StringComparison.OrdinalIgnoreCase);
            bool isNumeric = double.TryParse(newValue, out _);
            bool alreadyQuoted = newValue.StartsWith("\"") && newValue.EndsWith("\"");

            // Quote if not numeric/boolean and not already quoted
            if (!isBoolean && !isNumeric && !alreadyQuoted)
                newValue = $"\"{newValue}\"";

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string trimmed = line.TrimStart();
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                    continue;

                int indent = line.Length - trimmed.Length;

                while (indent < expectedIndent && currentLevel > 0)
                {
                    currentLevel--;
                    expectedIndent -= 2;
                }

                string key = trimmed.Split(':')[0];

                if (key == pathParts[currentLevel])
                {
                    if (currentLevel == pathParts.Length - 1)
                    {
                        int colonIndex = line.IndexOf(':');
                        int commentIndex = line.IndexOf('#');

                        string newLine = line.Substring(0, colonIndex + 1) + " " + newValue;
                        if (commentIndex >= 0)
                            newLine += " " + line.Substring(commentIndex);

                        lines[i] = newLine;
                        return lines;
                    }
                    else
                    {
                        currentLevel++;
                        expectedIndent = indent + 2;
                    }
                }
            }

            
            return lines;
        }



        static List<string> UpdateYamlText_old(List<string> lines, string[] pathParts, string newValue)
        {
            int currentLevel = 0;
            int expectedIndent = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string trimmed = line.TrimStart();
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                    continue;

                int indent = line.Length - trimmed.Length;

                while (indent < expectedIndent && currentLevel > 0)
                {
                    currentLevel--;
                    expectedIndent -= 2;
                }

                string key = trimmed.Split(':')[0];

                if (key == pathParts[currentLevel])
                {
                    if (currentLevel == pathParts.Length - 1)
                    {
                        int colonIndex = line.IndexOf(':');
                        int commentIndex = line.IndexOf('#');

                        string newLine = line.Substring(0, colonIndex + 1) + " " + newValue;
                        if (commentIndex >= 0)
                            newLine += " " + line.Substring(commentIndex);

                        lines[i] = newLine;
                        return lines;
                    }
                    else
                    {
                        currentLevel++;
                        expectedIndent = indent + 2;
                    }
                }
            }

           
            return lines;
        }
    }
}
