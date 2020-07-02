namespace CopyrightApplicationHelper
{
    partial class FormCopyrightApplicationHelper
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listBoxProgress = new System.Windows.Forms.ListBox();
            this.textBoxSourceDir = new System.Windows.Forms.TextBox();
            this.textBoxRule = new System.Windows.Forms.TextBox();
            this.buttonSourceDir = new System.Windows.Forms.Button();
            this.buttonRule = new System.Windows.Forms.Button();
            this.labelSourceDir = new System.Windows.Forms.Label();
            this.labelRule = new System.Windows.Forms.Label();
            this.buttonStartParsing = new System.Windows.Forms.Button();
            this.folderBrowserDialogSourceDir = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonOutput = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.openFileDialogRule = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogOutput = new System.Windows.Forms.SaveFileDialog();
            this.textBoxNameAndVersion = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxProgress
            // 
            this.listBoxProgress.FormattingEnabled = true;
            this.listBoxProgress.HorizontalScrollbar = true;
            this.listBoxProgress.ItemHeight = 12;
            this.listBoxProgress.Location = new System.Drawing.Point(15, 135);
            this.listBoxProgress.Name = "listBoxProgress";
            this.listBoxProgress.Size = new System.Drawing.Size(600, 292);
            this.listBoxProgress.TabIndex = 0;
            this.listBoxProgress.TabStop = false;
            // 
            // textBoxSourceDir
            // 
            this.textBoxSourceDir.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBoxSourceDir.Location = new System.Drawing.Point(84, 14);
            this.textBoxSourceDir.Name = "textBoxSourceDir";
            this.textBoxSourceDir.ReadOnly = true;
            this.textBoxSourceDir.Size = new System.Drawing.Size(300, 21);
            this.textBoxSourceDir.TabIndex = 1;
            // 
            // textBoxRule
            // 
            this.textBoxRule.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBoxRule.Location = new System.Drawing.Point(84, 41);
            this.textBoxRule.Name = "textBoxRule";
            this.textBoxRule.ReadOnly = true;
            this.textBoxRule.Size = new System.Drawing.Size(300, 21);
            this.textBoxRule.TabIndex = 2;
            // 
            // buttonSourceDir
            // 
            this.buttonSourceDir.Location = new System.Drawing.Point(390, 14);
            this.buttonSourceDir.Name = "buttonSourceDir";
            this.buttonSourceDir.Size = new System.Drawing.Size(85, 20);
            this.buttonSourceDir.TabIndex = 3;
            this.buttonSourceDir.Text = "选择目录";
            this.buttonSourceDir.UseVisualStyleBackColor = true;
            this.buttonSourceDir.Click += new System.EventHandler(this.buttonSourceDir_Click);
            // 
            // buttonRule
            // 
            this.buttonRule.Location = new System.Drawing.Point(390, 40);
            this.buttonRule.Name = "buttonRule";
            this.buttonRule.Size = new System.Drawing.Size(85, 20);
            this.buttonRule.TabIndex = 4;
            this.buttonRule.Text = "选择规则";
            this.buttonRule.UseVisualStyleBackColor = true;
            this.buttonRule.Click += new System.EventHandler(this.buttonRule_Click);
            // 
            // labelSourceDir
            // 
            this.labelSourceDir.AutoSize = true;
            this.labelSourceDir.Location = new System.Drawing.Point(13, 18);
            this.labelSourceDir.Name = "labelSourceDir";
            this.labelSourceDir.Size = new System.Drawing.Size(65, 12);
            this.labelSourceDir.TabIndex = 5;
            this.labelSourceDir.Text = "源代码目录";
            this.labelSourceDir.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelRule
            // 
            this.labelRule.AutoSize = true;
            this.labelRule.Location = new System.Drawing.Point(25, 44);
            this.labelRule.Name = "labelRule";
            this.labelRule.Size = new System.Drawing.Size(53, 12);
            this.labelRule.TabIndex = 6;
            this.labelRule.Text = "规则文件";
            this.labelRule.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonStartParsing
            // 
            this.buttonStartParsing.Location = new System.Drawing.Point(484, 13);
            this.buttonStartParsing.Name = "buttonStartParsing";
            this.buttonStartParsing.Size = new System.Drawing.Size(131, 106);
            this.buttonStartParsing.TabIndex = 7;
            this.buttonStartParsing.Text = "开始处理";
            this.buttonStartParsing.UseVisualStyleBackColor = true;
            this.buttonStartParsing.Click += new System.EventHandler(this.buttonStartParsing_Click);
            // 
            // buttonOutput
            // 
            this.buttonOutput.Location = new System.Drawing.Point(390, 67);
            this.buttonOutput.Name = "buttonOutput";
            this.buttonOutput.Size = new System.Drawing.Size(85, 20);
            this.buttonOutput.TabIndex = 9;
            this.buttonOutput.Text = "设置生成文件";
            this.buttonOutput.UseVisualStyleBackColor = true;
            this.buttonOutput.Click += new System.EventHandler(this.buttonOutput_Click);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBoxOutput.Location = new System.Drawing.Point(84, 68);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.Size = new System.Drawing.Size(300, 21);
            this.textBoxOutput.TabIndex = 8;
            // 
            // openFileDialogRule
            // 
            this.openFileDialogRule.Filter = "规则文件|rule.txt";
            // 
            // saveFileDialogOutput
            // 
            this.saveFileDialogOutput.Filter = "Word 文件|*.docx";
            // 
            // textBoxNameAndVersion
            // 
            this.textBoxNameAndVersion.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBoxNameAndVersion.Location = new System.Drawing.Point(84, 98);
            this.textBoxNameAndVersion.Name = "textBoxNameAndVersion";
            this.textBoxNameAndVersion.Size = new System.Drawing.Size(391, 21);
            this.textBoxNameAndVersion.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(25, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "生成文件";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(25, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 24);
            this.label2.TabIndex = 14;
            this.label2.Text = "软件名称\r\n与版本号";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FormCopyrightApplicationHelper
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(629, 443);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxNameAndVersion);
            this.Controls.Add(this.buttonOutput);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.buttonStartParsing);
            this.Controls.Add(this.labelRule);
            this.Controls.Add(this.labelSourceDir);
            this.Controls.Add(this.buttonRule);
            this.Controls.Add(this.buttonSourceDir);
            this.Controls.Add(this.textBoxRule);
            this.Controls.Add(this.textBoxSourceDir);
            this.Controls.Add(this.listBoxProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormCopyrightApplicationHelper";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ListBox listBoxProgress;
        private System.Windows.Forms.TextBox textBoxSourceDir;
        private System.Windows.Forms.TextBox textBoxRule;
        private System.Windows.Forms.Button buttonSourceDir;
        private System.Windows.Forms.Button buttonRule;
        private System.Windows.Forms.Label labelSourceDir;
        private System.Windows.Forms.Label labelRule;
        private System.Windows.Forms.Button buttonStartParsing;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogSourceDir;
        private System.Windows.Forms.Button buttonOutput;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.OpenFileDialog openFileDialogRule;
        private System.Windows.Forms.SaveFileDialog saveFileDialogOutput;
        private System.Windows.Forms.TextBox textBoxNameAndVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

