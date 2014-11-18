using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinFormsSyntaxHighlighter
{
    public class SyntaxHighlighter
    {
        /// <summary>
        /// Reference to the RichTextBox instance, for which 
        /// the syntax highlighting is going to occur.
        /// </summary>
        private readonly RichTextBox _richTextBox;

        /// <summary>
        /// Determines whether the program is busy creating rtf for the previous
        /// modification of the text-box. It is necessary to avoid blinks when the 
        /// user is typing fast.
        /// </summary>
        private bool _isDuringHighlight;

        private readonly List<KeyValuePair<PatternDefinition, SyntaxStyle>> _patternStylePairs = new List<KeyValuePair<PatternDefinition, SyntaxStyle>>(); 

        public SyntaxHighlighter(RichTextBox richTextBox)
        {
            if (richTextBox == null)
                throw new ArgumentNullException("richTextBox");

            _richTextBox = richTextBox;

            DisableHighlighting = false;
            ReplaceTabsWithSpaces = true;
            TabSize = 4;

            _richTextBox.TextChanged += RichTextBox_TextChanged;
        }


        /// <summary>
        /// Gets or sets a value indicating whether highlighting should be disabled or not.
        /// If true, the user input will remain intact. If false, the rich content will be
        /// modified to match the syntax of the currently selected language.
        /// </summary>
        public bool DisableHighlighting { get; set; }

        /// <summary>
        /// Number of spaces to put instead of tabs. Used only if <c>ReplaceTabsWithSpaces</c> is <c>true</c>.
        /// </summary>
        public int TabSize { get; set; }

        /// <summary>
        /// When set to <c>true</c> replaces tab characters with a number of spaces specified by <c>TabSize</c>.
        /// </summary>
        public bool ReplaceTabsWithSpaces { get; set; }

        public void AddPattern(PatternDefinition patternDefinition, SyntaxStyle syntaxStyle)
        {
            if (patternDefinition == null)
                throw new ArgumentNullException("patternDefinition");
            if (syntaxStyle == null)
                throw new ArgumentNullException("syntaxStyle");

            _patternStylePairs.Add(new KeyValuePair<PatternDefinition, SyntaxStyle>(patternDefinition, syntaxStyle));
        }

        /// <summary>
        /// Rehighlights the text-box content.
        /// </summary>
        public void ReHighlight()
        {
            if (!DisableHighlighting)
            {
                if (_isDuringHighlight) 
                    return;

                _richTextBox.DisableThenDoThenEnable(HighlighTextBase);
            }
        }

        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            ReHighlight();
        }

        /// <summary>
        /// The base method that highlights the text-box content.
        /// </summary>
        private void HighlighTextBase()
        {
            _isDuringHighlight = true;

            try
            {
                // TODO: do the parsing and RTF generation here
                //// create tab replacement string once forever
                //string tabReplacement = "\t";
                //if (AppSettings.Settings.ReplaceTabs)
                //{
                //    var sbTab = new StringBuilder();
                //    for (int i = 0; i < AppSettings.Settings.TabSize; ++i)
                //    {
                //        sbTab.Append(" ");
                //    }
                //    tabReplacement = sbTab.ToString();
                //}


                //ParserHelper helper = new ParserHelper(selectedLanguage);
                //var sb = new StringBuilder();

                //sb.AppendLine(RTFHeader());
                //sb.AppendLine(RTFColorTable());
                //sb.Append(@"\viewkind4\uc1\pard\f0\fs17 ");

                //foreach (var exp in helper.GetExpressions(TextBox.Text))
                //{
                //    if (exp.Type == ExpressionType.Whitespace)
                //    {
                //        string wsContent = exp.Content;
                //        // TODO: the cursor cannot go to the end of the strings, it remains where it was
                //        //if (AppSettings.Settings.ReplaceTabs && wsContent.IndexOf("\t") >= 0)
                //        //{
                //        //    wsContent = wsContent.Replace("\t", tabReplacement);
                //        //}
                //        sb.Append(wsContent);
                //    }
                //    else if (exp.Type == ExpressionType.Newline)
                //    {
                //        sb.AppendLine(@"\par");
                //    }
                //    else
                //    {
                //        string content = exp.Content.Replace("\\", "\\\\").Replace("{", @"\{").Replace("}", @"\}");

                //        ColorProfile.GroupProperties props;
                //        if (core.ColorProfiles.IsAnyProfileSelected &&
                //            core.ColorProfiles.SelectedProfile.TryGetProperties(
                //                Core.MapExpressionToColorProfileGroupName(exp), out props))
                //        {
                //            string opening = "", cloing = "";

                //            if (props.Bold)
                //            {
                //                opening += @"\b";
                //                cloing += @"\b0";
                //            }

                //            if (props.Italic)
                //            {
                //                opening += @"\i";
                //                cloing += @"\i0";
                //            }

                //            sb.AppendFormat(@"\cf{0}{2} {1}\cf0{3} ", core.GetColorIndex(exp.Type, exp.Group),
                //                content, opening, cloing);
                //        }
                //        else
                //        {
                //            sb.AppendFormat(@"\cf{0} {1}\cf0 ", core.GetColorIndex(exp.Type, exp.Group),
                //                content);
                //        }
                //    }
                //}

                //sb.Append(@"\par }");

                //_richTextBox.Rtf = sb.ToString();
            }
            finally
            {
                _isDuringHighlight = false;
            }
        }
    }
}
