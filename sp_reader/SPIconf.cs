using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;


namespace SPInterface
{
    
    public static class SPIconf
    {
        public static readonly string pathFromSP, pathToSP;
        public static readonly int timeout;
      
        /// <summary>
        /// the main interface with Calypso
        /// </summary>
        static SPIconf()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(SP_Path.folder_name);
            XmlNode xmlnode = xmldoc.SelectSingleNode("ConfigFileTest");
            XmlNodeList xmlnodelist = xmlnode.ChildNodes;
            foreach (XmlNode node in xmlnodelist)
            {
                if (node.Name.Equals("PathToSP", StringComparison.CurrentCultureIgnoreCase))
                {
                    pathToSP = new DirectoryInfo(node.InnerText).FullName;
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
