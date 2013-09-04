using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;


namespace SPInterface
{
    public class SPIconf
    {
        public readonly string pathFromSP, pathToSP;
        public readonly  int timeout;

        public SPIconf(string filePath)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(filePath);
            XmlNode xmlnode = xmldoc.SelectSingleNode("ConfigFileTest");
            XmlNodeList xmlnodelist = xmlnode.ChildNodes;
            foreach (XmlNode node in xmlnodelist)
            {
                if (node.Name.Equals( "PathToSP",StringComparison.CurrentCultureIgnoreCase))
                {
                    pathToSP = new DirectoryInfo( node.InnerText).FullName;
                }
                if (node.Name.Equals("PathFromSP", StringComparison.CurrentCultureIgnoreCase))
                {
                    pathFromSP = new DirectoryInfo(node.InnerText).FullName;
                }
                if (node.Name.Equals("Timeout", StringComparison.CurrentCultureIgnoreCase))
                {
                    timeout = Convert.ToInt32(node.InnerText);
                }
            }
           
           
        }
    }
}
