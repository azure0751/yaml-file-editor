using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace YamlTextEditor
{
    public partial class Form1 : Form
    {
        private RichTextBox yamlTextBox;
        private Panel configPanel;
        private SplitContainer splitContainer;
        private Button loadYamlButton, loadConfigButton, saveButton;

        private string yamlFilePath = "";
        private Dictionary<string, string> configMap = new();

        public Form1()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            this.Text = "YAML Config Editor (Nested Keys, Text-Based)";
            this.Width = 1200;
            this.Height = 700;

            splitContainer = new SplitContainer()
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 700
            };
            this.Controls.Add(splitContainer);

            // Left: YAML RichTextBox
            yamlTextBox = new RichTextBox()
            {
                Font = new Font("Consolas", 11),
                ReadOnly = true,
                Dock = DockStyle.Fill,
                WordWrap = false,
                BackColor = Color.FromArgb(25, 25, 25),
                ForeColor = Color.White
            };
            splitContainer.Panel1.Controls.Add(yamlTextBox);

            // Right: Config panel
            configPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            splitContainer.Panel2.Controls.Add(configPanel);

            // Top toolbar
            var toolbar = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                Height = 40
            };
            loadYamlButton = new Button() { Text = "Load YAML" };
            loadConfigButton = new Button() { Text = "Load Config" };
            saveButton = new Button() { Text = "Save YAML" };
            toolbar.Controls.AddRange(new Control[] { loadYamlButton, loadConfigButton, saveButton });
            this.Controls.Add(toolbar);

            // Event handlers
            loadYamlButton.Click += (s, e) => LoadYamlFile();
            loadConfigButton.Click += (s, e) => LoadConfigFile();
            saveButton.Click += (s, e) => SaveYaml();
        }

        // ---------------- Load YAML ----------------
        private void LoadYamlFile()
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "YAML Files|*.yaml;*.yml" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                yamlFilePath = ofd.FileName;
                yamlTextBox.Text = File.ReadAllText(yamlFilePath);
            }
        }

        // ---------------- Load Config ----------------
        private void LoadConfigFile()
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "YAML Files|*.yaml;*.yml" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var lines = File.ReadAllLines(ofd.FileName);
                configMap.Clear();
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                        continue;
                    var idx = line.IndexOf('=');
                    if (idx > 0)
                    {
                        string key = line.Substring(0, idx).Trim();
                        string value = line.Substring(idx + 1).Trim();
                        configMap[key] = value;
                    }
                }
                BuildConfigUI();
            }
        }

        // ---------------- Build Right-Side Editable UI ----------------
        private void BuildConfigUI()
        {
            configPanel.Controls.Clear();
            int y = 10;
            foreach (var kv in configMap)
            {
                var lbl = new Label() { Text = kv.Key, Left = 10, Top = y + 5, Width = 300 };
                var txt = new TextBox() { Left = 320, Top = y, Width = 250, Text = kv.Value };
                txt.TextChanged += (s, e) =>
                {
                    configMap[kv.Key] = txt.Text;
                    ApplyConfigToYaml();
                };
                configPanel.Controls.Add(lbl);
                configPanel.Controls.Add(txt);
                y += 35;
            }
        }

        // ---------------- Apply Config Updates ----------------
        private void ApplyConfigToYaml()
        {
            var lines = yamlTextBox.Text.Split('\n').ToList();

            foreach (var kv in configMap)
            {
                var pathParts = kv.Key.Split('=');
                lines = UpdateYamlText(lines, pathParts, kv.Value);
            }

            yamlTextBox.Text = string.Join("\n", lines);
        }

        // ---------------- Text-Based YAML Update ----------------
        private List<string> UpdateYamlText(List<string> lines, string[] pathParts, string newValue)
        {
            int targetIndent = 0;
            int level = 0;
            Stack<int> indentStack = new Stack<int>();

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string trimmed = line.TrimStart();
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                    continue;

                int currentIndent = line.Length - line.TrimStart().Length;

                // Adjust indentation stack
                while (indentStack.Count > 0 && currentIndent <= indentStack.Peek())
                    indentStack.Pop();

                string keyPart = trimmed.Split(':')[0];

                if (keyPart == pathParts[level])
                {
                    // Matching current level
                    if (level == pathParts.Length - 1)
                    {
                        // Update value while preserving inline comment
                        var commentIdx = line.IndexOf('#');
                        string newLine = line.Substring(0, line.IndexOf(':') + 1) + " " + newValue;
                        if (commentIdx >= 0)
                            newLine += " " + line.Substring(commentIdx);
                        lines[i] = newLine;
                        break;
                    }
                    else
                    {
                        // Go deeper
                        indentStack.Push(currentIndent);
                        level++;
                    }
                }
                else
                {
                    // If indentation decreased, step back level
                    while (indentStack.Count > 0 && currentIndent <= indentStack.Peek())
                    {
                        indentStack.Pop();
                        if (level > 0) level--;
                    }
                }
            }
            return lines;
        }

        // ---------------- Save YAML ----------------
        private void SaveYaml()
        {
            if (string.IsNullOrEmpty(yamlFilePath))
            {
                MessageBox.Show("Load a YAML file first.");
                return;
            }
            File.WriteAllText(yamlFilePath, yamlTextBox.Text);
            MessageBox.Show("YAML saved successfully!");
        }
    }
}
