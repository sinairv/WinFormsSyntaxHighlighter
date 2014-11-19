# Syntax Highlighting for Windows Forms Rich Text Box

This repository will enable syntax highlighting based on the patterns the programmer defines.

Example:

```csharp
var syntaxHighlighter = new SyntaxHighlighter(theRichTextBox);

// multi-line comments
syntaxHighlighter.AddPattern(
    new PatternDefinition(new Regex(@"//.*?$", 
        RegexOptions.Multiline | RegexOptions.Compiled)), 
    new SyntaxStyle(Color.Green, bold: false, italic: true));

// singlie-line comments
syntaxHighlighter.AddPattern(
    new PatternDefinition(new Regex(@"/\*(.|[\r\n])*?\*/", 
        RegexOptions.Multiline | RegexOptions.Compiled)), 
    new SyntaxStyle(Color.DarkSeaGreen, bold: false, italic: true));

// numbers
syntaxHighlighter.AddPattern(
    new PatternDefinition(@"\d+|\d+\.\d+"), 
    new SyntaxStyle(Color.Purple));

// double quote strings
syntaxHighlighter.AddPattern(
    new PatternDefinition(@"\""([^""]|\""\"")+\"""), 
    new SyntaxStyle(Color.Red));

// single quote strings
syntaxHighlighter.AddPattern(
    new PatternDefinition(@"\'([^']|\'\')+\'"), 
    new SyntaxStyle(Color.Salmon));
            
// keywords1
syntaxHighlighter.AddPattern(
    new PatternDefinition("for", "foreach", "int", "var"), 
    new SyntaxStyle(Color.Blue));
            
// keywords2
syntaxHighlighter.AddPattern(
    new CaseInsensitivePatternDefinition("public", "partial", "class", "void"), 
    new SyntaxStyle(Color.Navy, true, false));
            
// operators
syntaxHighlighter.AddPattern(
    new PatternDefinition("+", "-", ">", "<", "&", "|"), 
    new SyntaxStyle(Color.Brown));
``` 

## License: MIT 

Copyright (c) 2014 Sina Iravanian (sina.iravanian@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy of 
this software and associated documentation files (the "Software"), to deal in the 
Software without restriction, including without limitation the rights to use, copy, 
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, subject to the 
following conditions:

The above copyright notice and this permission notice shall be included in all 
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 