using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAIToolsWV
{
    public partial class DebugOutputWindow : Form
    {
        private TabControl tabCtrl;
        private TabPage tabPag;
        public TabPage TabPag
        {
            get { return tabPag; }
            set { tabPag = value; }
        }

        public TabControl TabCtrl
        {
            set { tabCtrl = value; }
        }

        public DebugOutputWindow()
        {
            InitializeComponent();
        }

        private void DebugOutputWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.tabPag.Dispose();
        }
    }
}
