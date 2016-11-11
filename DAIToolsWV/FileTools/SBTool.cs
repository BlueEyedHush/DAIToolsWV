﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAILibWV;
using DAILibWV.Frostbite;
using Be.Windows.Forms;

namespace DAIToolsWV.FileTools
{
    public partial class SBTool : Form
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

        public TOCFile toc;
        public SBFile sb;
        public CATFile cat_base, cat_patch;
        public CASFile cas;
        public BinaryBundle binBundle;
        public string basepath;
        public string lastsearch = "";
        public string[] types;


        public SBTool()
        {
            InitializeComponent();
        }

        private void opnSingleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "*.sb|*.sb";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                LoadFile(d.FileName);
        }

        public void LoadFile(string path)
        {
            this.Text = "SB Tool - " + path;
            toc = new TOCFile(Helpers.GetFileNameWithOutExtension(path) + ".toc");
            if (toc.iscas)
            {
                toolStrip1.Visible = true;
                splitContainer2.BringToFront();
                sb = new SBFile(path);
                RefreshTree();
            }
            else
            {
                toolStrip1.Visible = false;
                tabControl1.BringToFront();
                RefreshBinary();
            }
        }

        public void RefreshBinary()
        {
            listBox1.Items.Clear();
            if (toc != null)
                foreach (TOCFile.TOCBundleInfoStruct info in toc.bundles)
                    listBox1.Items.Add(info.id + (info.isbase ? " (B)" : "") + (info.isdelta ? " (D)" : ""));
            else
                listBox1.Items.Add("unknown");
        }
        public void RefreshBinaryBundle()
        {
            if (binBundle == null)
                return;
            rtb2.Text = "";
            rtb2.AppendText("Magic             : 0x" + binBundle.Header.magic.ToString("X8") + "\n");
            rtb2.AppendText("Total Count       : " + binBundle.Header.totalCount + "\n");
            rtb2.AppendText("EBX Count         : " + binBundle.Header.ebxCount + "\n");
            rtb2.AppendText("RES Count         : " + binBundle.Header.resCount + "\n");
            rtb2.AppendText("CHUNK Count       : " + binBundle.Header.chunkCount + "\n");
            rtb2.AppendText("String Offset     : " + binBundle.Header.stringOffset + "\n");
            rtb2.AppendText("Chunk Meta Offset : " + binBundle.Header.chunkMetaOffset + "\n");
            rtb2.AppendText("Chunk Meta Size   : " + binBundle.Header.chunkMetaSize + "\n");
            listBox2.Items.Clear();
            foreach (BinaryBundle.EbxEntry ebx in binBundle.EbxList)
                listBox2.Items.Add(ebx._name);
            listBox3.Items.Clear();
            foreach (BinaryBundle.ResEntry res in binBundle.ResList)
                listBox3.Items.Add(res._name);
            listBox4.Items.Clear();
            foreach (BinaryBundle.ChunkEntry chunk in binBundle.ChunkList)
                listBox4.Items.Add(Helpers.ByteArrayToHexString(chunk.id));
            treeView2.Nodes.Clear();
            TreeNode t = new TreeNode("Meta");
            if (binBundle.ChunkMeta != null)
                t = BJSON.MakeField(t, binBundle.ChunkMeta);
            treeView2.Nodes.Add(t);
        }

        public void RefreshTree()
        {
            if (sb == null)
                return;
            treeView1.Nodes.Clear();
            foreach (BJSON.Entry e in sb.lines)
                treeView1.Nodes.Add(BJSON.MakeEntry(new TreeNode(e.type.ToString("X")), e));
            Debug.LockWindowUpdate(treeView1.Handle);
            Helpers.ExpandTreeByLevel(treeView1.Nodes[0], 1);
            Debug.LockWindowUpdate(System.IntPtr.Zero);
            Application.DoEvents();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string text = toolStripTextBox1.Text;
            Helpers.SelectNext(toolStripTextBox1.Text, treeView1);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int n = toolStripComboBox1.SelectedIndex;
            if (n == -1)
                return;
            Helpers.SelectNext(toolStripComboBox1.Items[n].ToString().Split(' ')[0].Trim(), treeView1);
        }

        private void SBTool_Load(object sender, EventArgs e)
        {
            tabCtrl.SelectedTab = tabPag;
            
            types = DBAccess.GetUsedRESTypes();
            List<string> tmp = new List<string>();
            foreach (string type in types)
                if (type != "")
                    tmp.Add(BitConverter.ToInt32(Helpers.HexStringToByteArray(type), 0).ToString("X8"));//reversed it
            toolStripComboBox1.Items.Clear();
            int count=0;
            foreach (string type in types)
                if (type != "")
                toolStripComboBox1.Items.Add("0x" + tmp[count++] + " " + Helpers.GetResType(BitConverter.ToUInt32(Helpers.HexStringToByteArray(type), 0)));
            types = tmp.ToArray();
            toolStripComboBox1.SelectedIndex = 0;
        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            TreeNode t = treeView1.SelectedNode;
            hb1.BringToFront();
            hb1.ByteProvider = new DynamicByteProvider(new byte[0]);
            if (t == null) return;
            if (t.Text.ToLower().Contains("sha1"))
            {
                TreeNode t2 = t;
                if (t2 != null)
                {
                    string sha1 = t2.Nodes[0].Text;
                    byte[] sha1buff = Helpers.HexStringToByteArray(sha1);
                    byte[] data = SHA1Access.GetDataBySha1(sha1buff);
                    hb1.ByteProvider = new DynamicByteProvider(data);
                    hb1.BringToFront();
                    rtb1.Text = Helpers.DecompileLUAC(data);
                    if (rtb1.Text != "")
                        rtb1.BringToFront();
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            if (toc != null)
                d.Filter = "*.sb|*.sb";
            else
                d.Filter = "*.bundle|*.bundle";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                if (toc != null && !toc.iscas)
                {
                    sb.Save(d.FileName);
                }
                else
                {
                    if (toc != null)
                    {
                        //TODO save entire sb back nad update toc
                    }
                    else
                    {
                        binBundle.Save(d.FileName);
                    }
                }
                MessageBox.Show("Done.");
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            TreeNode t = treeView1.SelectedNode;
            if (t == null || (t.Nodes != null && t.Nodes.Count != 0))
                return;
            List<int> Indices = GetIndices(t);
            BJSON.Entry entry = sb.lines[Indices[0]];
            BJSON.Field field = null;
            byte swap = 0;
            for (int i = 1; i < Indices.Count - 1; i++)
            {
                if (i % 2 == swap)
                {
                    if (field.data is List<BJSON.Entry>)
                    {
                        List<BJSON.Entry> list = (List<BJSON.Entry>)field.data;
                        entry = list[Indices[i]];
                    }
                    if (field.data is List<BJSON.Field>)
                    {
                        List<BJSON.Field> list = (List<BJSON.Field>)field.data;
                        field = list[Indices[i]];
                        if (swap == 0)
                            swap = 1;
                        else
                            swap = 0;
                    }
                }
                else
                {
                    field = entry.fields[Indices[i]];
                }
            }
            if (field != null)
            {
                TOCTool_InputForm input = new TOCTool_InputForm();
                switch (field.type)
                {
                    case 1:
                        return;
                    case 7:
                        input.hb1.Enabled = false;
                        input.rtb1.Text = (string)field.data;
                        break;
                    case 6:
                        byte[] tmp = new byte[1];
                        if ((bool)field.data)
                            tmp[0] = 1;
                        input.hb1.ByteProvider = new FixedByteProvider(tmp);
                        input.rtb1.Enabled = false;
                        break;
                    default:
                        if (field.data is byte[])
                        {
                            input.hb1.ByteProvider = new DynamicByteProvider((byte[])field.data);
                            input.rtb1.Enabled = false;
                        }
                        break;
                }
                DialogResult res = input.ShowDialog();
                MemoryStream m;
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    switch (field.type)
                    {
                        case 7:
                            field.data = input.rtb1.Text;
                            break;
                        case 6:
                            m = new MemoryStream();
                            for (long i = 0; i < input.hb1.ByteProvider.Length; i++)
                                m.WriteByte(input.hb1.ByteProvider.ReadByte(i));
                            if (m.Length == 1)
                            {
                                m.Seek(0, 0);
                                if ((byte)m.ReadByte() == 0)
                                    field.data = false;
                                else
                                    field.data = true;
                            }
                            break;
                        default:
                            if (field.data is byte[])
                            {
                                m = new MemoryStream();
                                for (long i = 0; i < input.hb1.ByteProvider.Length; i++)
                                    m.WriteByte(input.hb1.ByteProvider.ReadByte(i));
                                field.data = m.ToArray();
                            }
                            break;
                    }
                    RefreshTree();
                }
            }
        }

        private List<int> GetIndices(TreeNode t)
        {
            List<int> result = new List<int>();
            if (t.Parent != null)
                result.AddRange(GetIndices(t.Parent));
            result.Add(t.Index);
            return result;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toc == null)
                return;
            int n = listBox1.SelectedIndex;
            if (n == -1)
                return;
            TOCFile.TOCBundleInfoStruct info = toc.bundles[n];
            if (info.isbase)
                return;
            byte[] data = toc.ExportBinaryBundle(info);
            binBundle = new BinaryBundle(new MemoryStream(data));
            RefreshBinaryBundle();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = listBox2.SelectedIndex;
            if (n == -1)
                return;
            BinaryBundle.EbxEntry ebx = binBundle.EbxList[n];
            hb2.ByteProvider = new DynamicByteProvider(ebx._data);
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {            
            int n = listBox3.SelectedIndex;
            if (n == -1)
                return;
            BinaryBundle.ResEntry res = binBundle.ResList[n];
            hb3.ByteProvider = new DynamicByteProvider(res._data);
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            int n = listBox4.SelectedIndex;
            if (n == -1)
                return;
            BinaryBundle.ChunkEntry chunk = binBundle.ChunkList[n];
            hb4.ByteProvider = new DynamicByteProvider(chunk._data);
        }

        private void expandAllSubNodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode t = treeView1.SelectedNode;
            if (t == null)
                return;
            Debug.LockWindowUpdate(treeView1.Handle);
            t.ExpandAll();
            Debug.LockWindowUpdate(System.IntPtr.Zero);
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                string text = toolStripTextBox1.Text;
                Helpers.SelectNext(toolStripTextBox1.Text, treeView1);
            }
        }

        private void openRawBundleToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "*.bundle|*.bundle";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                toolStrip1.Visible = true;
                splitContainer2.BringToFront();
                sb = new SBFile(d.FileName);
                RefreshTree();
                return;
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lastsearch = Microsoft.VisualBasic.Interaction.InputBox("Please enter search value", "Edit", lastsearch).ToLower();
            for(int i=0;i<listBox2.Items.Count;i++)
                if (listBox2.Items[i].ToString().ToLower().Contains(lastsearch))
                {
                    listBox2.SelectedIndex = i;
                    return;
                }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            lastsearch = Microsoft.VisualBasic.Interaction.InputBox("Please enter search value", "Edit", lastsearch).ToLower();
            for (int i = 0; i < listBox3.Items.Count; i++)
                if (listBox3.Items[i].ToString().ToLower().Contains(lastsearch))
                {
                    listBox3.SelectedIndex = i;
                    return;
                }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            lastsearch = Microsoft.VisualBasic.Interaction.InputBox("Please enter search value", "Edit", lastsearch).ToLower();
            for (int i = 0; i < listBox4.Items.Count; i++)
                if (listBox4.Items[i].ToString().ToLower().Contains(lastsearch))
                {
                    listBox4.SelectedIndex = i;
                    return;
                }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "*.bundle|*.bundle";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                toc = null;
                toolStrip1.Visible = true;
                splitContainer2.BringToFront();
                MemoryStream m = new MemoryStream(File.ReadAllBytes(d.FileName));
                binBundle = new BinaryBundle(m);
                tabControl1.BringToFront();
                RefreshBinary();
                RefreshBinaryBundle();
                return;
            }
        }

        private void SBTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.tabPag.Dispose();
        }
    }
}
