﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace VBN_Editor
{
    public partial class VBNRebuilder : Form
    {
        public VBN vbn;
        public bool vbnSet = false;
        public bool loaded = false;
        public DataTable tbl;

        public VBNRebuilder()
        {
            InitializeComponent();
        }
        
        private TreeNode buildBoneTree(int index)
        {
            List<TreeNode> children = new List<TreeNode>();
            foreach (int i in vbn.bones[index].children)
            {
                children.Add(buildBoneTree(i));
            }
            
            TreeNode temp = new TreeNode(new string(vbn.bones[index].boneName),children.ToArray());

            if (index == 0)
                treeView1.Nodes.Add(temp);

            return temp;

        }

        private void openNUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = "";
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Smash 4 Boneset|*.vbn|All files(*.*)|*.*";
            DialogResult result = save.ShowDialog();

            if(result == DialogResult.OK)
            {
                filename = save.FileName;
                vbn.save(filename);
            }
        }

        private void openVBNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = "";
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Smash 4 Boneset|*.vbn|All files(*.*)|*.*";
            DialogResult result = open.ShowDialog();

            if(result == DialogResult.OK)
            {
                filename = open.FileName;
                vbn = new VBN(filename);
                treeView1.Nodes.Clear();
                buildBoneTree(0);
                vbnSet = true;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newForm = new Form2 ();
            newForm.ShowDialog();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            GL.ClearColor(Color.SkyBlue);
            Application.Idle += AppIdle;
            SetupViewPort();
        }

        private void SetupViewPort()
        {
            int h = glControl1.Height;
            int w = glControl1.Width;
            GL.MatrixMode(MatrixMode.Projection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Viewport(0, 0, w, h);
        }

        private void Render()
        {
            if (!loaded)
                return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (vbnSet)
            {
                foreach (Bone bone in vbn.bones)
                {
                    if (bone.parentIndex != 0x0FFFFFFF)
                    {
                        uint i = bone.parentIndex;
                        GL.Color3(Color.Green);
                        GL.LineWidth(1f);
                        GL.Begin(BeginMode.Lines);
                        GL.Vertex3(vbn.bones[i].position[0], vbn.bones[i].position[1], vbn.bones[i].position[2]);
                        GL.Vertex3(bone.position[0], bone.position[1], bone.position[2]);
                        //GL.Vertex3(0, 0, 0);
                        //GL.Vertex3(100, 100, 0);
                        GL.End();
                    }
                }
            }

            glControl1.SwapBuffers();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            glControl1.SwapBuffers();
        }

        private void AppIdle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                Render();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBox1.Text = treeView1.SelectedNode.Text;
            tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("Name") { ReadOnly = true });
            tbl.Columns.Add("Value");
            dataGridView1.DataSource = tbl;
            tbl.Rows.Clear();

            tbl.Rows.Add("Bone Hash",vbn.bone(treeView1.SelectedNode.Text).boneId.ToString("X"));
            tbl.Rows.Add("Bone Type", vbn.bone(treeView1.SelectedNode.Text).boneType);
            tbl.Rows.Add("X Pos", vbn.bone(treeView1.SelectedNode.Text).position[0]);
            tbl.Rows.Add("Y Pos", vbn.bone(treeView1.SelectedNode.Text).position[1]);
            tbl.Rows.Add("Z Pos", vbn.bone(treeView1.SelectedNode.Text).position[2]);
            tbl.Rows.Add("X Rot", vbn.bone(treeView1.SelectedNode.Text).rotation[0]);
            tbl.Rows.Add("Y Rot", vbn.bone(treeView1.SelectedNode.Text).rotation[1]);
            tbl.Rows.Add("Z Rot", vbn.bone(treeView1.SelectedNode.Text).rotation[2]);
            tbl.Rows.Add("X Scale", vbn.bone(treeView1.SelectedNode.Text).scale[0]);
            tbl.Rows.Add("Y Scale", vbn.bone(treeView1.SelectedNode.Text).scale[1]);
            tbl.Rows.Add("Z Scale", vbn.bone(treeView1.SelectedNode.Text).scale[2]);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            vbn.bones[vbn.boneIndex(treeView1.SelectedNode.Text)].boneId = (uint)int.Parse(tbl.Rows[0][1].ToString(), System.Globalization.NumberStyles.HexNumber);
            vbn.bones[vbn.boneIndex(treeView1.SelectedNode.Text)].boneType = Convert.ToUInt32(tbl.Rows[1][1]);

            vbn.bone(treeView1.SelectedNode.Text).position[0] = Convert.ToSingle(tbl.Rows[2][1]);
            vbn.bone(treeView1.SelectedNode.Text).position[1] = Convert.ToSingle(tbl.Rows[3][1]);
            vbn.bone(treeView1.SelectedNode.Text).position[2] = Convert.ToSingle(tbl.Rows[4][1]);

            vbn.bone(treeView1.SelectedNode.Text).rotation[0] = Convert.ToSingle(tbl.Rows[5][1]);
            vbn.bone(treeView1.SelectedNode.Text).rotation[1] = Convert.ToSingle(tbl.Rows[6][1]);
            vbn.bone(treeView1.SelectedNode.Text).rotation[2] = Convert.ToSingle(tbl.Rows[7][1]);

            vbn.bone(treeView1.SelectedNode.Text).scale[0] = Convert.ToSingle(tbl.Rows[8][1]);
            vbn.bone(treeView1.SelectedNode.Text).scale[1] = Convert.ToSingle(tbl.Rows[9][1]);
            vbn.bone(treeView1.SelectedNode.Text).scale[2] = Convert.ToSingle(tbl.Rows[10][1]);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            vbn.bones[vbn.boneIndex(treeView1.SelectedNode.Text)].boneName = textBox1.Text.ToCharArray();
            treeView1.SelectedNode.Text = textBox1.Text;
        }
    }
}
