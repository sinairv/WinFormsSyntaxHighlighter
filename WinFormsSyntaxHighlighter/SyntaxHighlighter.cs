using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

        private readonly List<PatternStyleMap> _patternStyles = new List<PatternStyleMap>(); 

        public SyntaxHighlighter(RichTextBox richTextBox)
        {
            if (richTextBox == null)
                throw new ArgumentNullException("richTextBox");

            _richTextBox = richTextBox;

            DisableHighlighting = false;

            _richTextBox.TextChanged += RichTextBox_TextChanged;
        }


        /// <summary>
        /// Gets or sets a value indicating whether highlighting should be disabled or not.
        /// If true, the user input will remain intact. If false, the rich content will be
        /// modified to match the syntax of the currently selected language.
        /// </summary>
        public bool DisableHighlighting { get; set; }

        public void AddPattern(PatternDefinition patternDefinition, SyntaxStyle syntaxStyle)
        {
            AddPattern((_patternStyles.Count + 1).ToString(CultureInfo.InvariantCulture), patternDefinition, syntaxStyle);
        }

        public void AddPattern(string name, PatternDefinition patternDefinition, SyntaxStyle syntaxStyle)
        {
            if (patternDefinition == null)
                throw new ArgumentNullException("patternDefinition");
            if (syntaxStyle == null)
                throw new ArgumentNullException("syntaxStyle");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name must not be null or empty", "name");

            var existingPatternStyle = FindPatternStyle(name);

            if (existingPatternStyle != null)
                throw new ArgumentException("A pattern style pair with the same name already exists");

            _patternStyles.Add(new PatternStyleMap(name, patternDefinition, syntaxStyle));
        }

        private PatternStyleMap FindPatternStyle(string name)
        {
            var patternStyle = _patternStyles.FirstOrDefault(p => String.Equals(p.Name, name, StringComparison.Ordinal));
            return patternStyle;
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

        // TODO: make abstact
        internal IEnumerable<Expression> Parse(string text)
        {
            return new[] { new Expression(text, ExpressionType.String, String.Empty)};
        }

        // TODO: make abstract
        internal IEnumerable<StyleGroupPair> GetStyles()
        {
            return new[] { new StyleGroupPair(new SyntaxStyle(_richTextBox.ForeColor), String.Empty)};
        }

        // TODO: make virtual
        internal virtual string GetGroupName(Expression expression)
        {
            return expression.Group;
        }

        private List<StyleGroupPair> _styleGroupPairs;

        private List<StyleGroupPair> GetStyleGroupPairs()
        {
            if (_styleGroupPairs == null)
            {
                _styleGroupPairs = GetStyles().ToList();

                for (int i = 0; i < _styleGroupPairs.Count; i++)
                {
                    _styleGroupPairs[i].Index = i + 1;
                }
            }

            return _styleGroupPairs;
        }

        /// <summary>
        /// The base method that highlights the text-box content.
        /// </summary>
        private void HighlighTextBase()
        {
            _isDuringHighlight = true;

            try
            {
                var sb = new StringBuilder();

                sb.AppendLine(RTFHeader());
                sb.AppendLine(RTFColorTable());
                sb.Append(@"\viewkind4\uc1\pard\f0\fs17 ");

                foreach (var exp in Parse(_richTextBox.Text))
                {
                    if (exp.Type == ExpressionType.Whitespace)
                    {
                        string wsContent = exp.Content;
                        sb.Append(wsContent);
                    }
                    else if (exp.Type == ExpressionType.Newline)
                    {
                        sb.AppendLine(@"\par");
                    }
                    else
                    {
                        string content = exp.Content.Replace("\\", "\\\\").Replace("{", @"\{").Replace("}", @"\}");

                        var styleGroups = GetStyleGroupPairs();

                        string groupName = GetGroupName(exp);

                        var styleToApply = styleGroups.FirstOrDefault(s => String.Equals(s.GroupName, groupName, StringComparison.Ordinal));

                        if (styleToApply != null)
                        {
                            string opening = String.Empty, cloing = String.Empty;

                            if (styleToApply.SyntaxStyle.Bold)
                            {
                                opening += @"\b";
                                cloing += @"\b0";
                            }

                            if (styleToApply.SyntaxStyle.Italic)
                            {
                                opening += @"\i";
                                cloing += @"\i0";
                            }

                            sb.AppendFormat(@"\cf{0}{2} {1}\cf0{3} ", styleToApply.Index,
                                content, opening, cloing);
                        }
                        else
                        {
                            sb.AppendFormat(@"\cf{0} {1}\cf0 ", 1,
                                content);
                        }
                    }
                }

                sb.Append(@"\par }");

                _richTextBox.Rtf = sb.ToString();
            }
            finally
            {
                _isDuringHighlight = false;
            }
        }

        private string RTFColorTable()
        {
            var styleGroupPairs = GetStyleGroupPairs();

            if (styleGroupPairs.Count <= 0)
                styleGroupPairs.Add(new StyleGroupPair(new SyntaxStyle(_richTextBox.ForeColor), String.Empty));

            var sbRtfColorTable = new StringBuilder();
            sbRtfColorTable.Append(@"{\colortbl ;");

            foreach (var styleGroup in styleGroupPairs)
            {
                sbRtfColorTable.AppendFormat("{0};", ColorUtils.ColorToRtfTableEntry(styleGroup.SyntaxStyle.Color));
            }

            sbRtfColorTable.Append("}");

            return sbRtfColorTable.ToString();
        }


        #region RTF Stuff
        private string RTFHeader()
        {
            return @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Courier New;}}";
        }

        #endregion

    }
}
