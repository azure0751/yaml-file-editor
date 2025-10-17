using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YamlTextEditor
{
    internal class RichTextHighlighter
    {
        // <summary>
        /// Highlights multiple exact words inside a RichTextBox.
        /// Each match is fully word-boundary checked (exact word match).
        /// </summary>
        /// <param name="rtb">The RichTextBox control.</param>
        /// <param name="wordsToHighlight">List of words to highlight.</param>
        /// <param name="highlightColor">Color used for highlighting (default = Yellow).</param>
        /// <param name="caseSensitive">If true, match is case-sensitive; otherwise, case-insensitive.</param>
        public static void HighlightWords(RichTextBox rtb, List<string> wordsToHighlight, Color? highlightColor = null, bool caseSensitive = false)
        {
            if (rtb == null || wordsToHighlight == null || wordsToHighlight.Count == 0)
                return;

            Color color = highlightColor ?? Color.Yellow;

            // Preserve user selection
            int selStart = rtb.SelectionStart;
            int selLength = rtb.SelectionLength;

            // Clear old highlights
            rtb.SelectAll();
            rtb.SelectionBackColor = Color.White;
            rtb.DeselectAll();

            string text = rtb.Text;
            StringComparison comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            foreach (string word in wordsToHighlight)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

                int index = 0;
                while ((index = text.IndexOf(word, index, comparison)) != -1)
                {
                    // Exact word check (word boundaries)
                    bool isWordBoundaryStart = index == 0 || !char.IsLetterOrDigit(text[index - 1]);
                    bool isWordBoundaryEnd = index + word.Length == text.Length || !char.IsLetterOrDigit(text[index + word.Length]);

                    if (isWordBoundaryStart && isWordBoundaryEnd)
                    {
                        rtb.Select(index, word.Length);
                        rtb.SelectionBackColor = color;
                    }
                    index += word.Length;
                }
            }

            // Restore selection
            rtb.Select(selStart, selLength);
            rtb.SelectionBackColor = Color.Transparent;
            rtb.ScrollToCaret();
        }
    }
}
