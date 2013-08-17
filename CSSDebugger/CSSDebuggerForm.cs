using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Marzersoft.CSS;
using BluEngine.ScreenManager.Screens;
using BluEngine.ScreenManager.Styles.CSS;

namespace CSSDebugger
{
    public partial class CSSDebuggerForm : Form
    {
        private BluCSSParser parser = null;
        public CSSDebuggerForm()
        {
            InitializeComponent();
            parser = new BluCSSParser(true);
        }

        private void tbInput_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            tbOutput.Text = parser.ParseText(tbInput.Text).ToString();
        }
    }
}
