using Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Update
{
    public partial class UpdateForm : Form
    {
        public UpdateForm()
        {
            InitializeComponent();
        }

        private void UpdateSoft()
        {
            try
            {
                var url = XmlHelp.GetInnerTextByPath("Config:UpdateUrl");
                var json = HtmlHelp.Get(url);

                var result = JsonHelp.DeserializeObject<ResultEntity>(json);

                if (result.Code == "1")
                {
                    var url2 = XmlHelp.GetInnerTextByPath("Config:WebUrl");
                    var zip = Path.GetFileName(result.FileName);
                    var b = HtmlHelp.DownFile(url2 + result.FileName, zip, progressBar1);

                    if (!b)
                        throw new Exception();

                    //解压下载的文件
                    var aPath = Application.StartupPath + "/";
                    ZipHelp.ExtractToDirectory(zip, aPath + "NewPixiv");

                    //删除下载的压缩文件
                    File.Delete(aPath + zip);

                    //替换文件
                    ReplaceFile(aPath, aPath + "NewPixiv");

                    //删除解压后的文件
                    Directory.Delete(aPath + "NewPixiv", true);

                    //重新启动主程序
                    var mainPreocess = XmlHelp.GetInnerTextByPath("Config:MainProcess");
                    Process.Start(mainPreocess);
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("更新文件失败！");
            }
        }

        private void ReplaceFile(string oldPath, string newPath)
        {
            oldPath = oldPath.EndsWith("/") ? oldPath : oldPath + "/";
            newPath = newPath.EndsWith("/") ? newPath : newPath + "/";

            //var fs = Directory.GetFiles(newPath);
            var dr = Directory.CreateDirectory(newPath);
            var fs = dr.GetFiles().Select(p => p.Name);
            foreach (var f in fs)
            {
                File.Copy(newPath + f, oldPath + f, true);
            }

            var ds = Directory.GetDirectories(newPath).Select(p => Path.GetDirectoryName(p));
            foreach (var d in ds)
            {
                ReplaceFile(oldPath + d, newPath + d);
            }
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            //var s = Screen.PrimaryScreen.Bounds;
            var s = SystemInformation.WorkingArea;

            this.Location = new Point(s.Width - this.Width, s.Height - this.Height);

            //更新软件
            UpdateSoft();

            //this.Close();
            var updateFile = XmlHelp.GetInnerTextByPath("Config:UpdateFileName").Replace(".exe", "");
            var ps = Process.GetProcessesByName(updateFile);
            foreach (var p in ps)
            {
                p.Kill();
            }
        }
    }
}
