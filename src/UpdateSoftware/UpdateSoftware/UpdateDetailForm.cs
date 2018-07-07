using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Command;
using System.Diagnostics;
using System.IO;

namespace UpdateSoftware
{
    public partial class UpdateDetailForm : Form
    {
        public string updateDetail = "";
        public string updateSoft = "";

        public UpdateDetailForm()
        {
            InitializeComponent();
        }

        private void UpdateDetailForm_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = updateDetail;
        }

        private new void Update()
        {
            //下载更新软件
            var url = XmlHelp.GetInnerTextByPath("Config:WebUrl") + updateSoft;
            var fileName = Path.GetFileName(updateSoft);
            var b = HtmlHelp.DownFile(url, fileName);
            if (!b)
            {
                MessageBox.Show("下载更新文件失败！");
            }
            else
            {
                var aPath = Application.StartupPath + "/";

                var updateFileName = XmlHelp.GetInnerTextByPath("Config:UpdateFileName");
                if (File.Exists(updateFileName))
                    File.Delete(updateFileName);

                ZipHelp.ExtractToDirectory(fileName, aPath);
                File.Delete(fileName);

                //启动更新程序
                Process.Start(updateFileName);
                //关闭主进程
                var proc = Process.GetProcessesByName(XmlHelp.GetInnerTextByPath("Config:MainProcess").Replace(".exe", ""));
                foreach (var p in proc)
                {
                    p.Kill();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (updateSoft != "")
            {
                Update();
            }
            else
            {
                MessageBox.Show("参数错误！");
            }
        }
    }
}
