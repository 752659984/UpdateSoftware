using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Command
{
    public class XmlHelp
    {
        private static string XmlFileName = "Config.xml";

        public static string GetInnerTextByPath(string path)
        {
            if (!File.Exists(XmlFileName))
                return null;

            var doc = new XmlDocument();
            doc.Load(XmlFileName);
            
            var ps = path.Split(':');
            var pNode = doc.SelectSingleNode(ps[0]);
            for (var i = 1; i < ps.Length; ++i)
            {
                pNode = pNode.SelectSingleNode(ps[i]);
            }

            return pNode.InnerText;
        }
    }
}
