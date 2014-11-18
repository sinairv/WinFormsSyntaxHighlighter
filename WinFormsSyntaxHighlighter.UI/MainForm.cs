using System.Windows.Forms;

namespace WinFormsSyntaxHighlighter.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var syntaxHighlighter = new SyntaxHighlighter(rtbInput);
        }
    }
}
