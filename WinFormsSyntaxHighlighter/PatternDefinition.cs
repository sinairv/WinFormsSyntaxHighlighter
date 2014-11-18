using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WinFormsSyntaxHighlighter
{
    public class PatternDefinition
    {
        private Regex _regex;

        public PatternDefinition(Regex regularExpression)
        {
            if (regularExpression == null)
                throw new ArgumentNullException("regularExpression");
            _regex = regularExpression;
        }

        public PatternDefinition(string regexPattern)
        {
            if (String.IsNullOrEmpty(regexPattern))
                throw new ArgumentException("regex pattern must not be null or empty", "regexPattern");

            _regex = new Regex(regexPattern, RegexOptions.Compiled);
        }

        public PatternDefinition(IEnumerable<string> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException("tokens");

            var regexTokens = new List<string>();

            foreach (var token in tokens)
            {
                var escaptedToken = Regex.Escape(token.Trim());

                if (escaptedToken.Length > 0)
                {
                    if (Char.IsLetterOrDigit(escaptedToken[0]))
                        regexTokens.Add(String.Format(@"\b{0}\b", escaptedToken));
                    else
                        regexTokens.Add(escaptedToken);
                }

            }

            string pattern = String.Join("|", regexTokens);
            _regex = new Regex(pattern, RegexOptions.Compiled);
        }
    }
}
