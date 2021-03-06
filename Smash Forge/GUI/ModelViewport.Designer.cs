﻿namespace Smash_Forge
{
    partial class ModelViewport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelViewport));
            this.glViewport = new OpenTK.GLControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.totalFrame = new System.Windows.Forms.NumericUpDown();
            this.currentFrame = new System.Windows.Forms.NumericUpDown();
            this.endButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.beginButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.weightToolButton = new System.Windows.Forms.ToolStripButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.totalFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentFrame)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glViewport
            // 
            this.glViewport.BackColor = System.Drawing.Color.Black;
            this.glViewport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glViewport.Location = new System.Drawing.Point(0, 0);
            this.glViewport.Name = "glViewport";
            this.glViewport.Size = new System.Drawing.Size(624, 441);
            this.glViewport.TabIndex = 0;
            this.glViewport.VSync = false;
            this.glViewport.Click += new System.EventHandler(this.glViewport_Click);
            this.glViewport.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.glViewport_MouseDoubleClick);
            this.glViewport.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glViewport_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.totalFrame);
            this.groupBox1.Controls.Add(this.currentFrame);
            this.groupBox1.Controls.Add(this.endButton);
            this.groupBox1.Controls.Add(this.nextButton);
            this.groupBox1.Controls.Add(this.beginButton);
            this.groupBox1.Controls.Add(this.prevButton);
            this.groupBox1.Controls.Add(this.playButton);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 350);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(624, 91);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(441, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Frame:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(542, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "/";
            // 
            // totalFrame
            // 
            this.totalFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.totalFrame.Enabled = false;
            this.totalFrame.Location = new System.Drawing.Point(564, 14);
            this.totalFrame.Name = "totalFrame";
            this.totalFrame.Size = new System.Drawing.Size(54, 20);
            this.totalFrame.TabIndex = 6;
            // 
            // currentFrame
            // 
            this.currentFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.currentFrame.Location = new System.Drawing.Point(482, 14);
            this.currentFrame.Name = "currentFrame";
            this.currentFrame.Size = new System.Drawing.Size(54, 20);
            this.currentFrame.TabIndex = 5;
            // 
            // endButton
            // 
            this.endButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.endButton.Location = new System.Drawing.Point(580, 40);
            this.endButton.Name = "endButton";
            this.endButton.Size = new System.Drawing.Size(38, 45);
            this.endButton.TabIndex = 4;
            this.endButton.Text = ">>";
            this.endButton.UseVisualStyleBackColor = true;
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nextButton.Location = new System.Drawing.Point(542, 40);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(32, 45);
            this.nextButton.TabIndex = 3;
            this.nextButton.Text = ">";
            this.nextButton.UseVisualStyleBackColor = true;
            // 
            // beginButton
            // 
            this.beginButton.Location = new System.Drawing.Point(6, 40);
            this.beginButton.Name = "beginButton";
            this.beginButton.Size = new System.Drawing.Size(38, 44);
            this.beginButton.TabIndex = 2;
            this.beginButton.Text = "<<";
            this.beginButton.UseVisualStyleBackColor = true;
            // 
            // prevButton
            // 
            this.prevButton.Location = new System.Drawing.Point(50, 40);
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size(34, 44);
            this.prevButton.TabIndex = 1;
            this.prevButton.Text = "<";
            this.prevButton.UseVisualStyleBackColor = true;
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playButton.AutoSize = true;
            this.playButton.Location = new System.Drawing.Point(90, 40);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(446, 44);
            this.playButton.TabIndex = 0;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.weightToolButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(624, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabel1.Text = "Camera";
            // 
            // weightToolButton
            // 
            this.weightToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.weightToolButton.Image = ((System.Drawing.Image)(resources.GetObject("weightToolButton.Image")));
            this.weightToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.weightToolButton.Name = "weightToolButton";
            this.weightToolButton.Size = new System.Drawing.Size(23, 22);
            this.weightToolButton.Text = "toolStripButton1";
            this.weightToolButton.Click += new System.EventHandler(this.weightToolButton_Click);
            // 
            // ModelViewport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.glViewport);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ModelViewport";
            this.Text = "ModelViewport";
            this.Load += new System.EventHandler(this.ModelViewport_Load);
            this.Resize += new System.EventHandler(this.ModelViewport_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.totalFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentFrame)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glViewport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button endButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button beginButton;
        private System.Windows.Forms.Button prevButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown totalFrame;
        private System.Windows.Forms.NumericUpDown currentFrame;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton weightToolButton;
    }
}