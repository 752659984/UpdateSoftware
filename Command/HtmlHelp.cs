using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Command
{
    public class HtmlHelp
    {
        public static string Get(string url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader sr = null;

            try
            {

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                response = (HttpWebResponse)request.GetResponse();

                stream = response.GetResponseStream();

                sr = new StreamReader(stream);

                return sr.ReadToEnd();
            }
            catch
            {
                return "";
            }
            finally
            {
                if (request != null)
                    request.Abort();

                if (response != null)
                    response.Dispose();

                if (stream != null)
                    stream.Dispose();

                if (sr != null)
                    sr.Dispose();
            }
        }

        public static bool DownFile(string url, string fileName)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            FileStream fs = null;

            try
            {

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                response = (HttpWebResponse)request.GetResponse();

                stream = response.GetResponseStream();

                fs = new FileStream(fileName, FileMode.Create);

                var buffter = new byte[1024];
                int n = 0;
                while ((n = stream.Read(buffter, 0, buffter.Length)) > 0)
                {
                    fs.Write(buffter, 0, n);
                }

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
            finally
            {
                if (request != null)
                    request.Abort();

                if (response != null)
                    response.Dispose();

                if (stream != null)
                    stream.Dispose();

                if (fs != null)
                    fs.Dispose();
            }
        }

        public static bool DownFile(string url, string fileName, ProgressBar pb)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            FileStream fs = null;

            try
            {
                pb.Value = 0;

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                response = (HttpWebResponse)request.GetResponse();

                stream = response.GetResponseStream();

                fs = new FileStream(fileName, FileMode.Create);

                var buffter = new byte[1024];
                var total = response.ContentLength;
                var count = 0.0;
                var n = 0;
                while ((n = stream.Read(buffter, 0, buffter.Length)) > 0)
                {
                    fs.Write(buffter, 0, n);
                    count += n;
                    pb.Value = (int)(count / total * 100);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                if (request != null)
                    request.Abort();

                if (response != null)
                    response.Dispose();

                if (stream != null)
                    stream.Dispose();

                if (fs != null)
                    fs.Dispose();
            }
        }
    }
}
