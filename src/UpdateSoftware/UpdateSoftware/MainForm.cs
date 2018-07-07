using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Command;
using System.IO;
using System.Diagnostics;

namespace UpdateSoftware
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void CheckUpdate()
        {
            try
            {
                var updateFile = XmlHelp.GetInnerTextByPath("Config:UpdateFileName");

                var ps = Process.GetProcessesByName(updateFile.Replace(".exe", ""));
                foreach (var p in ps)
                {
                    p.Kill();
                }
                if (File.Exists(updateFile))
                    File.Delete(updateFile);

                var url = XmlHelp.GetInnerTextByPath("Config:CheckVersionUrl");

                var json = HtmlHelp.Get(url);

                var result = JsonHelp.DeserializeObject<ResultEntity>(json);

                if (result.Code == "-1")
                {
                    MessageBox.Show(result.Message);
                }
                else if (result.Code == "1")
                {
                    UpdateDetailForm f = new UpdateDetailForm();
                    f.updateSoft = result.UpdateSoft;
                    f.updateDetail = result.UpdateDetail;

                    f.Show();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("检查更新失败！");
                //MessageBox.Show(e.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckUpdate();
        }
    }
}
