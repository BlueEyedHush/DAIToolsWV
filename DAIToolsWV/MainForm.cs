using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using DAILibWV;

namespace DAIToolsWV
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        DebugOutputWindow debug;
        bool init = false;

        private void MainForm_Load(object sender, EventArgs e)
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = "DAI Tools WV - build : " + v.ToString();
        }

        private void Init()
        {
            init = true;
            debug = new DebugOutputWindow();
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = debug.Text;
            debug.MdiParent = this;
            debug.Show();
            debug.TabPag = tp;
            debug.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;

            Debug.SetBox(debug.rtb1);
            Debug.LogLn("Hello there! Im starting...");
            Application.DoEvents();
            bool exist = DBAccess.CheckIfDBExists();
            Debug.LogLn("Database file found : " + exist);
            if (!exist)
            {
                DialogResult result = MessageBox.Show("No database found, do you want to create a new one?", "Database", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    DBAccess.CreateDataBase();
                    Debug.LogLn("Database file created");
                }
                else
                    this.Close();
            }
            DBAccess.LoadSettings();
            bool needsScan = DBAccess.CheckIfScanIsNeeded();
            Debug.LogLn("Initial Scan needed : " + needsScan);
            if (needsScan)
            {
                DialogResult result = MessageBox.Show("Database is empty, do you want to start initial scan?", "Database", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    OpenFileDialog d = new OpenFileDialog();
                    d.Filter = "DragonAgeInquisition.exe|DragonAgeInquisition.exe";
                    if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        DBAccess.StartScan(Path.GetDirectoryName(d.FileName) + "\\");
                    Debug.LogLn("Initial Scan Done");
                }
                else
                    this.Close();
            }
            Debug.LogLn("I'm ready!");
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            if(!init)
                Init();
        }

        private void tOCToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileTools.TOCTool child = new FileTools.TOCTool();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void sBToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileTools.SBTool child = new FileTools.SBTool();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void mFTToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileTools.MFTTool child = new FileTools.MFTTool();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void hexToolToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form f = new FileTools.HexTool();
            f.Show();
        }

        private void initFsToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileTools.INITFSTool child = new FileTools.INITFSTool();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void eBXToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContentTools.EBXTool child = new ContentTools.EBXTool();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void cASContainerCreatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileTools.CASContainerCreator child = new FileTools.CASContainerCreator();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void textureToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContentTools.TextureTool child = new ContentTools.TextureTool();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void modEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModTools.ModEditor child = new ModTools.ModEditor();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void modRunnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModTools.ModRunner child = new ModTools.ModRunner();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void bundleBuilderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileTools.BundleBuilder child = new FileTools.BundleBuilder();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void fileBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browser.FileBrowser child = new Browser.FileBrowser();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void bundleBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browser.BundleBrowser child = new Browser.BundleBrowser();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void eBXBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browser.EBXBrowser child = new Browser.EBXBrowser();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void textureBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browser.TextureBrowser child = new Browser.TextureBrowser();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void talktableToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContentTools.TalktableTool child = new ContentTools.TalktableTool();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void sHA1LookupToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ContentTools.SHA1Lookup child = new ContentTools.SHA1Lookup();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void hexToStringToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new GeneralTools.HexToString();
            f.Show();
        }

        private void cATRepairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeneralTools.CATrepair child = new GeneralTools.CATrepair();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void folderCompareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeneralTools.FolderCompare child = new GeneralTools.FolderCompare();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void rESBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browser.RESBrowser child = new Browser.RESBrowser();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void meshBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browser.MeshBrowser child = new Browser.MeshBrowser();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void meshToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContentTools.MeshTool child = new ContentTools.MeshTool();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void supportDAIMODToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileTools.DAIMODSupportTool child = new FileTools.DAIMODSupportTool();
            child.TabCtrl = this.tabControl1;
            TabPage tp = new TabPage();
            tp.Parent = tabControl1;
            tp.Text = child.Text;
            child.MdiParent = this;
            child.Show();
            child.TabPag = tp;
            child.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tp;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex > -1)
                this.MdiChildren[tabControl1.SelectedIndex].Select();
        }
    }
}
