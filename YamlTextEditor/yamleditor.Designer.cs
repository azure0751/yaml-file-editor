namespace YamlTextEditor
{
    partial class yamleditor
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
            panel1 = new Panel();
            splitContainer1 = new SplitContainer();
            richTextBox1 = new RichTextBox();
            panel2 = new Panel();
            btnGenerateParamPath = new Button();
            lblinputyamlfile = new Label();
            btnloadparamterfile = new Button();
            btnLoadYaml = new Button();
            chkEnableSingleApply = new CheckBox();
            chkEnplaceEditing = new CheckBox();
            lblParameterFile = new Label();
            panel3 = new Panel();
            btnApplyAll = new Button();
            pnlsideui = new Panel();
            btnSaveUpdatedYaml = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Location = new Point(0, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(945, 125);
            panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(richTextBox1);
            splitContainer1.Panel1.Controls.Add(panel2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(chkEnableSingleApply);
            splitContainer1.Panel2.Controls.Add(chkEnplaceEditing);
            splitContainer1.Panel2.Controls.Add(lblParameterFile);
            splitContainer1.Panel2.Controls.Add(panel3);
            splitContainer1.Panel2.Controls.Add(pnlsideui);
            splitContainer1.Size = new Size(945, 585);
            splitContainer1.SplitterDistance = 690;
            splitContainer1.TabIndex = 1;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.Location = new Point(0, 91);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(690, 494);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Controls.Add(btnSaveUpdatedYaml);
            panel2.Controls.Add(btnGenerateParamPath);
            panel2.Controls.Add(lblinputyamlfile);
            panel2.Controls.Add(btnloadparamterfile);
            panel2.Controls.Add(btnLoadYaml);
            panel2.Location = new Point(7, 11);
            panel2.Name = "panel2";
            panel2.Size = new Size(680, 74);
            panel2.TabIndex = 0;
            // 
            // btnGenerateParamPath
            // 
            btnGenerateParamPath.Location = new Point(486, 4);
            btnGenerateParamPath.Name = "btnGenerateParamPath";
            btnGenerateParamPath.Size = new Size(161, 29);
            btnGenerateParamPath.TabIndex = 3;
            btnGenerateParamPath.Text = "Generate Param Paths";
            btnGenerateParamPath.UseVisualStyleBackColor = true;
            btnGenerateParamPath.Click += btnGenerateParamPath_Click;
            // 
            // lblinputyamlfile
            // 
            lblinputyamlfile.AutoSize = true;
            lblinputyamlfile.Location = new Point(8, 50);
            lblinputyamlfile.Name = "lblinputyamlfile";
            lblinputyamlfile.Size = new Size(81, 20);
            lblinputyamlfile.TabIndex = 2;
            lblinputyamlfile.Text = "Input File : ";
            // 
            // btnloadparamterfile
            // 
            btnloadparamterfile.Location = new Point(178, 4);
            btnloadparamterfile.Name = "btnloadparamterfile";
            btnloadparamterfile.Size = new Size(156, 29);
            btnloadparamterfile.TabIndex = 1;
            btnloadparamterfile.Text = "Load Parameter File";
            btnloadparamterfile.UseVisualStyleBackColor = true;
            btnloadparamterfile.Click += btnloadparamterfile_Click;
            // 
            // btnLoadYaml
            // 
            btnLoadYaml.Location = new Point(5, 4);
            btnLoadYaml.Name = "btnLoadYaml";
            btnLoadYaml.Size = new Size(167, 29);
            btnLoadYaml.TabIndex = 0;
            btnLoadYaml.Text = "Load Input YAML ";
            btnLoadYaml.UseVisualStyleBackColor = true;
            btnLoadYaml.Click += btnLoadYaml_Click;
            // 
            // chkEnableSingleApply
            // 
            chkEnableSingleApply.AutoSize = true;
            chkEnableSingleApply.Location = new Point(11, 34);
            chkEnableSingleApply.Name = "chkEnableSingleApply";
            chkEnableSingleApply.Size = new Size(164, 24);
            chkEnableSingleApply.TabIndex = 5;
            chkEnableSingleApply.Text = "Enable Single Apply";
            chkEnableSingleApply.UseVisualStyleBackColor = true;
            chkEnableSingleApply.CheckedChanged += chkEnableSingleApply_CheckedChanged;
            // 
            // chkEnplaceEditing
            // 
            chkEnplaceEditing.AutoSize = true;
            chkEnplaceEditing.Location = new Point(11, 8);
            chkEnplaceEditing.Name = "chkEnplaceEditing";
            chkEnplaceEditing.Size = new Size(182, 24);
            chkEnplaceEditing.TabIndex = 4;
            chkEnplaceEditing.Text = "Enable In Place Editing";
            chkEnplaceEditing.UseVisualStyleBackColor = true;
            chkEnplaceEditing.CheckedChanged += chkEnplaceEditing_CheckedChanged;
            // 
            // lblParameterFile
            // 
            lblParameterFile.AutoSize = true;
            lblParameterFile.Location = new Point(3, 68);
            lblParameterFile.Name = "lblParameterFile";
            lblParameterFile.Size = new Size(81, 20);
            lblParameterFile.TabIndex = 3;
            lblParameterFile.Text = "Input File : ";
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlDarkDark;
            panel3.Controls.Add(btnApplyAll);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 534);
            panel3.Name = "panel3";
            panel3.Size = new Size(251, 51);
            panel3.TabIndex = 1;
            // 
            // btnApplyAll
            // 
            btnApplyAll.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnApplyAll.Location = new Point(2, 0);
            btnApplyAll.Name = "btnApplyAll";
            btnApplyAll.Size = new Size(246, 48);
            btnApplyAll.TabIndex = 0;
            btnApplyAll.Text = "Apply All Configuration";
            btnApplyAll.UseVisualStyleBackColor = true;
            btnApplyAll.Click += btnApplyAll_Click;
            // 
            // pnlsideui
            // 
            pnlsideui.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlsideui.AutoScroll = true;
            pnlsideui.BackColor = SystemColors.ActiveCaption;
            pnlsideui.Location = new Point(0, 91);
            pnlsideui.Name = "pnlsideui";
            pnlsideui.Size = new Size(251, 437);
            pnlsideui.TabIndex = 0;
            // 
            // btnSaveUpdatedYaml
            // 
            btnSaveUpdatedYaml.Location = new Point(340, 4);
            btnSaveUpdatedYaml.Name = "btnSaveUpdatedYaml";
            btnSaveUpdatedYaml.Size = new Size(140, 29);
            btnSaveUpdatedYaml.TabIndex = 4;
            btnSaveUpdatedYaml.Text = "Save Updated File";
            btnSaveUpdatedYaml.UseVisualStyleBackColor = true;
            btnSaveUpdatedYaml.Click += btnSaveUpdatedYaml_Click;
            // 
            // yamleditor
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(945, 585);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            Name = "yamleditor";
            Text = "yamleditor";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private SplitContainer splitContainer1;
        private Panel panel2;
        private RichTextBox richTextBox1;
        private Button btnLoadYaml;
        private Button btnloadparamterfile;
        private Panel pnlsideui;
        private Button btnApplyAll;
        private Panel panel3;
        private Label lblinputyamlfile;
        private Label lblParameterFile;
        private Button btnGenerateParamPath;
        private CheckBox chkEnplaceEditing;
        private CheckBox chkEnableSingleApply;
        private Button btnSaveUpdatedYaml;
    }
}