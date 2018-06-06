using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace AutoLiveRecorder
{
    public partial class Form1 : Form
    {
        #region Private 字段

        private B_Live aa;
        private bool IsRecording = false;

        /// <summary>
        /// 处理状态
        /// </summary>
        private bool ProcessStatus = false;

        #endregion Private 字段

        #region Public 构造函数

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        #endregion Public 构造函数

        #region Private 方法

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("参数不能为空！");
            }
            else
            {
                aa = new B_Live(int.Parse(textBox2.Text));
                aa.IsAutoTranscode = checkBox2.Checked;
                aa.RecordStatusChanged += RecordStatus_Changed;
                aa.ProcessStatusChanged += ProcessStatus_Changed;
                aa.StartRecord(textBox1.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            aa.StopRecord();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("参数不能为空！");
                }
                else
                {
                    aa = new B_Live(int.Parse(textBox2.Text));
                    aa.IsAutoTranscode = checkBox2.Checked;
                    aa.RecordStatusChanged += RecordStatus_Changed;
                    aa.ProcessStatusChanged += ProcessStatus_Changed;
                    aa.ReadyToRecord(textBox1.Text);
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        { 
            if (checkBox2.Checked)
            {
                if (!System.IO.File.Exists("ffmpeg.exe"))
                {
                    if (MessageBox.Show("需要下载ffmpeg到本地！\r\n点击\"是\"：立即下载ffmpeg，可能会出现短暂的无响应。\r\n点击\"否\"：将不会启用自动转码功能。", "录播姬", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        WebClient c = new WebClient();
                        c.DownloadFile("http://update.zhangbudademao.com/112/ffmpeg.exe", "ffmpeg.exe");
                    }else
                    {
                        checkBox2.Checked = false;
                        return;
                    }
                }
                saveFileDialog1.DefaultExt = "mp4";
                saveFileDialog1.Filter = "MP4媒体文件|*.mp4";
                if(saveFileDialog1.FileName == "*.flv")
                {
                    saveFileDialog1.FileName = "*.mp4";
                }else
                {
                    saveFileDialog1.FileName = saveFileDialog1.FileName.Substring(0,saveFileDialog1.FileName.LastIndexOf(".")) + ".mp4";
                }
            }else
            {
                saveFileDialog1.DefaultExt = "flv";
                saveFileDialog1.Filter = "flv流文件|*.flv";
                if (saveFileDialog1.FileName == "*.mp4")
                {
                    saveFileDialog1.FileName = "*.flv";
                }
                else
                {
                    saveFileDialog1.FileName = saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.LastIndexOf(".")) + ".flv";
                }
            }
            if (aa != null) aa.IsAutoTranscode = checkBox2.Checked;
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        /// <param name="MustGUI">是否必须有界面提示</param>
        private void CheckUpdate(bool MustGUI = false)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://api.zhangbudademao.com/112/update.php");
            HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
            System.IO.StreamReader Reader = new System.IO.StreamReader(rep.GetResponseStream());
            string str = Reader.ReadToEnd();
            string[] infos = str.Split('|');
            if (infos[0] != Application.ProductVersion)
            {
                if (MessageBox.Show("有新版本！\r\n" + Application.ProductVersion + "→" + infos[0] + "\r\n更新内容：" + infos[2] + "\r\n更新时间：" + infos[3], "有新版本！", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(infos[1]);
                    Application.Exit();
                }
            }
            else if (MustGUI) MessageBox.Show("版本已是最新", "录播姬");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsRecording)
            {
                MessageBox.Show("正在录制中，请结束录制后重试");
                e.Cancel = true;
            }
            if (ProcessStatus)
            {
                MessageBox.Show("正在处理录制文件，请稍后重试");
                e.Cancel = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckUpdate();
            label3.Text = "当前版本：" + Application.ProductVersion;
            if (checkBox2.Checked)
            {
                if (!System.IO.File.Exists("ffmpeg.exe"))
                {
                    if (MessageBox.Show("需要下载ffmpeg到本地！\r\n点击\"是\"：立即下载ffmpeg，可能会出现短暂的无响应。\r\n点击\"否\"：将不会启用自动转码功能。", "录播姬", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        WebClient c = new WebClient();
                        c.DownloadFile("http://update.zhangbudademao.com/112/ffmpeg.exe", "ffmpeg.exe");
                    }
                    else
                    {
                        checkBox2.Checked = false;
                        return;
                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CheckUpdate(true);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.ShowDialog();
        }

        /// <summary>
        /// 处理状态改变
        /// </summary>
        /// <param name="ProcessStatus">处理状态</param>
        private void ProcessStatus_Changed(bool ProcessStatus)
        {
            this.ProcessStatus = ProcessStatus;
        }

        private void RecordStatus_Changed(bool RecordStatus)
        {
            IsRecording = RecordStatus;
            if (RecordStatus)
            {
                button1.Enabled = false;
                button2.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
                textBox1.Text = "";
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\*\\.flv");
            if (!reg.Match(saveFileDialog1.FileName).Success)
            {
                textBox1.Text = saveFileDialog1.FileName;
            }
            else
            {
                textBox1.Text = "";
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "请点击此处设置保存路径。") textBox1.Text = ""; textBox1.ForeColor = System.Drawing.Color.Black;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "") textBox1.Text = "请点击此处设置保存路径。"; textBox1.ForeColor = System.Drawing.Color.Gray;
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            saveFileDialog1.ShowDialog();
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\*\\.flv");
            if (!reg.Match(saveFileDialog1.FileName).Success)
            {
                textBox1.Text = saveFileDialog1.FileName;
            }
            else
            {
                textBox1.Text = "";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == "请点击此处设置保存路径。")
            {
                checkBox1.Checked = false;
                checkBox1.Enabled = false;
                button1.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
                button1.Enabled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        #endregion Private 方法
    }
}