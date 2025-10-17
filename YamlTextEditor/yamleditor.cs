using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YamlTextEditor
{
    public partial class yamleditor : Form
    {
        private string yamlFilePath = "";
        private Dictionary<string, string> configMap = new();
        private bool isEnplaceEditingEnabled = false;
        private bool isEnableSingleApplyEnabled = false;
        public yamleditor()
        {
            InitializeComponent();
        }

        private void btnLoadYaml_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "YAML Files|*.yaml;*.yml" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                yamlFilePath = ofd.FileName;
                richTextBox1.Text = File.ReadAllText(yamlFilePath);
                lblinputyamlfile.Text = "";
                lblinputyamlfile.Text = "Selected Input File : " + yamlFilePath;
            }
        }

        private void btnloadparamterfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "YAML Files|*.yaml;*.yml" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                lblParameterFile.Text = "Selected Parameter File : " + ofd.FileName;
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
            pnlsideui.Controls.Clear();
            int y = 10;
            foreach (var kv in configMap)
            {
                var lbl = new Label() { Text = kv.Key, Left = 10, Top = y + 5, Width = 300, Font = new Font("Arial", 10, FontStyle.Regular )};
                var txt = new TextBox() { Left = 320, Top = y, Width = 250, Text = kv.Value };
                var btn = new Button() { Left = 600, Top = y, Height = 30, Width = 150, Text = "Apply" };
                btn.Click += (s, e) =>
                {
                    configMap[kv.Key] = txt.Text;
                    string str = $"{lbl.Text}={txt.Text}";
                    //MessageBox.Show($"{lbl.Text}={txt.Text}");

                    if (isEnableSingleApplyEnabled)
                    {
                        ApplyConfigToYaml(str);
                        SearchAndHighlight($"{txt.Text}");
                    }
                };

                txt.TextChanged += (s, e) =>
                {
                    configMap[kv.Key] = txt.Text;
                    string str = $"{lbl.Text}={txt.Text}";
                    //MessageBox.Show($"{lbl.Text}={txt.Text}");

                    if (isEnplaceEditingEnabled)
                    {
                        ApplyConfigToYaml(str);
                    }


                };
                pnlsideui.Controls.Add(lbl);
                pnlsideui.Controls.Add(txt);
                pnlsideui.Controls.Add(btn);
                y += 35;
            }
        }



        private void SearchAndHighlight(string textToSearch)
        {
            string searchTerm = textToSearch;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                //lblInfo.Text = "Please enter a search term.";
                return;
            }

            // Clear previous highlights
            int selStart = richTextBox1.SelectionStart;
            int selLength = richTextBox1.SelectionLength;
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = Color.White;
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.DeselectAll();

            // Find term
            int index = richTextBox1.Text.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                // Highlight
                richTextBox1.Select(index, searchTerm.Length);
                richTextBox1.SelectionBackColor = Color.Yellow;
                richTextBox1.SelectionColor = Color.Black;

                // Scroll to position
                richTextBox1.ScrollToCaret();

                // lblInfo.Text = $"Found at position {index}";
            }
            else
            {
                //lblInfo.Text = "No matches found.";
            }

            // Restore cursor
            richTextBox1.Select(selStart, selLength);
        }




        // ---------------- Apply Config Updates ----------------
        private void ApplyConfigToYaml(string configvalue)
        {
            var lines = richTextBox1.Text.Split('\n').ToList();


            string[] pathParts = configvalue.Split('=');
            string[] keyParts = pathParts[0].Split(":");

            lines = UpdateYamlText(lines, keyParts, pathParts[1].Trim());


            richTextBox1.Text = string.Join("\n", lines);
        }
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

                string key = trimmed.Split(':')[0].Trim();

                string axs = pathParts[currentLevel].Trim();



                if (key == pathParts[currentLevel].Trim())
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

        private void btnGenerateParamPath_Click(object sender, EventArgs e)
        {
            var flattened = YamlFlattener.FlattenYamlFromString(richTextBox1.Text);
            ShowSelectableMessage("Details", FlattenedListToString(flattened));

            MessageBox.Show(FlattenedListToString(flattened));

        }

        /// <summary>
        /// Converts a flattened YAML List (each item like key=value) into a single formatted string.
        /// </summary>
        public static string FlattenedListToString(List<string> flattenedList, string separator = "\n")
        {
            if (flattenedList == null || flattenedList.Count == 0)
                return string.Empty;

            return string.Join(separator, flattenedList);
        }

        public static void ShowSelectableMessage(string title, string message)
        {
            Form form = new Form()
            {
                Text = title,
                Width = 400,
                Height = 250,
                StartPosition = FormStartPosition.CenterParent
            };

            TextBox textBox = new TextBox()
            {
                Multiline = true,
                ReadOnly = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Text = message
            };

            Button okButton = new Button()
            {
                Text = "OK",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            okButton.Click += (s, e) => form.Close();

            form.Controls.Add(textBox);
            form.Controls.Add(okButton);

            form.ShowDialog();
        }

        private void btnApplyAll_Click(object sender, EventArgs e)
        {
            foreach (var kv in configMap)
            {

                string str = $"{kv.Key}={kv.Value}";

                ApplyConfigToYaml(str);
            }


        }

        private void chkEnplaceEditing_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnplaceEditing.Checked)
            {
                isEnplaceEditingEnabled = true;
            }
            else
            {
                isEnplaceEditingEnabled = false;
            }
        }

        private void chkEnableSingleApply_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableSingleApply.Checked)
            {
                isEnableSingleApplyEnabled = true;
            }
            else
            {
                isEnableSingleApplyEnabled = false;
            }
        }
    }
}
