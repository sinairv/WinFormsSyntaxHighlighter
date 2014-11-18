using System.Drawing;
using System.Windows.Forms;

namespace WinFormsSyntaxHighlighter.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var syntaxHighlighter = new SyntaxHighlighter(rtbInput);

            syntaxHighlighter.AddPattern(new PatternDefinition(new[] {"for", "foreach", "int"}), new SyntaxStyle(Color.Blue));
        }
    }
}
