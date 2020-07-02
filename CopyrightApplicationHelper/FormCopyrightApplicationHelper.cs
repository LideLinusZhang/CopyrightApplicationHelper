using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyrightApplicationHelper
{
    public partial class FormCopyrightApplicationHelper : Form
    {
        private static readonly string MessageBoxCaption = "提示";
        private static readonly string FinishedMessage = "处理成功。";
        private static readonly string SourceDirNotSelectedWarning = "源代码目录还未选择，不能开始处理。请先选择源代码目录。";
        private static readonly string RuleNotSelectedWarning = "规则文件还未选择，不能开始处理。请先选择规则文件。";
        private static readonly string OutputNotSelectedWarning = "生成文件还未设置，不能开始处理。请先设置生成文件。";
        private static readonly string NameAndVersionNotEnteredWarning = "软件名称和版本还未输入，不能开始处理。请先输入软件名称和版本。";
        private static readonly string FileInUseError = "有正在运行的程序占用了生成文件，不能开始处理。请关闭相关程序并重试。";
        public FormCopyrightApplicationHelper()
        {
            InitializeComponent();

            string programTitle = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
            string programVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 3);
            this.Text = string.Format("{0} V{1}", programTitle, programVersion);

            openFileDialogRule.InitialDirectory = System.Environment.CurrentDirectory;
        }

        private void buttonStartParsing_Click(object sender, EventArgs e)
        {
            if (ReadyForParsing())
            {
                FileStream fileDocx;
                try
                {
                    fileDocx = new FileStream(textBoxOutput.Text, FileMode.Create, FileAccess.Write);
                }
                catch (IOException)
                {
                    MessageBox.Show(FileInUseError, MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Task task = new Task(() =>
                {
                    RuleImporter.ImportRules(textBoxRule.Text);
                    Parser.TraverseSourceForParse(textBoxSourceDir.Text);

                    Output.OutputDocx(fileDocx);
                    fileDocx.Close();

                    if (InvokeRequired)
                        Invoke((Action<bool>)Finished, true);
                    else
                        Finished(true);
                });
                task.Start();
            }
            else
                Finished(false);
        }

        private bool ReadyForParsing()
        {
            if (textBoxSourceDir.Text == string.Empty)
            {
                MessageBox.Show(SourceDirNotSelectedWarning, MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (textBoxRule.Text == string.Empty)
            {
                MessageBox.Show(RuleNotSelectedWarning, MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (textBoxOutput.Text == string.Empty)
            {
                MessageBox.Show(OutputNotSelectedWarning, MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (textBoxNameAndVersion.Text == string.Empty)
            {
                MessageBox.Show(NameAndVersionNotEnteredWarning, MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            buttonOutput.Enabled = false;
            buttonRule.Enabled = false;
            buttonSourceDir.Enabled = false;
            buttonStartParsing.Enabled = false;
            textBoxNameAndVersion.ReadOnly = true;

            ProgressInfo.AppendListBoxProgress = ShowProgress;
            Output.NameAndVersion = textBoxNameAndVersion.Text;

            listBoxProgress.Items.Clear();

            return true;
        }

        private void buttonSourceDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialogSourceDir.ShowDialog();
            textBoxSourceDir.Text = folderBrowserDialogSourceDir.SelectedPath;
        }

        private void buttonRule_Click(object sender, EventArgs e)
        {
            openFileDialogRule.ShowDialog();
            textBoxRule.Text = openFileDialogRule.FileName;
        }

        private void buttonOutput_Click(object sender, EventArgs e)
        {
            saveFileDialogOutput.ShowDialog();
            textBoxOutput.Text = saveFileDialogOutput.FileName;
        }

        private void ShowProgress(string progressInfo)
        {
            if (InvokeRequired)
            {
                Invoke((Action)delegate ()
               {
                   listBoxProgress.Items.Add(progressInfo);
                   listBoxProgress.SelectedIndex = listBoxProgress.Items.Count - 1;
               });
            }
            else
            {
                listBoxProgress.Items.Add(progressInfo);
                listBoxProgress.SelectedIndex = listBoxProgress.Items.Count - 1;
            }
        }

        private void Finished(bool isSuccessful)
        {
            buttonRule.Enabled = true;
            buttonSourceDir.Enabled = true;
            buttonOutput.Enabled = true;
            buttonStartParsing.Enabled = true;
            textBoxNameAndVersion.ReadOnly = false;

            if (isSuccessful)
                MessageBox.Show(FinishedMessage, MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
