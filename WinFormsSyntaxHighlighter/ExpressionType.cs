namespace WinFormsSyntaxHighlighter
{
    /// <summary>
    /// Enumerates the type of the parsed content
    /// </summary>
    public enum ExpressionType
    {
        Identifier, // i.e. a word which is neither keyword nor inside any word-group
        Operator,
        Number,
        Whitespace,
        Newline,
        Keyword,
        Comment,
        CommentLine,
        String,
        DelimitedGroup,       // needs extra argument
        WordGroup             // needs extra argument
    }
}
