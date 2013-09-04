using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SPInterface
{
    public class SPI
    {
        private SPIconf spiconf;
        public List<Element> elements;
        public readonly string CharacterName;
        public readonly Dictionary<string, string> sys_dict;


        public string getPathFromSpecialProgram()
        {
            return spiconf.pathFromSP;
        }
        public string getPathToSpecialProgram()
        {
            return spiconf.pathToSP;
        }
        
        public SPI(string SPIconfpath)
        {
            spiconf = new SPIconf(SPIconfpath);

            string xmlpath = spiconf.pathToSP + @"\ElementsToSpecialProgram.xml";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlpath);
            XmlNode xmlnode = xmldoc.SelectSingleNode("GeometryData");
            XmlNodeList xmlnodelist = xmlnode.ChildNodes;
            elements = new List<Element>();
            foreach (XmlNode node in xmlnodelist)
            {
                string identify = node.Name;
                if(identify.Equals("Name"))
                {
                    CharacterName = node.InnerText;
                }
                if (identify.Equals("Element"))
                {
                    Element n_ele = new Element(node);
                    elements.Add(n_ele);
                }
            }

            sys_dict = new Dictionary<string, string>();

            string syspath = spiconf.pathToSP + @"\SysParaToSpecialProgram.xml";
            XmlDocument sysxmldoc = new XmlDocument();
            sysxmldoc.Load(syspath);
            XmlNode sysxmlnode = sysxmldoc.SelectSingleNode("SystemParameters");
            XmlNodeList sysxmlnodelist = sysxmlnode.ChildNodes;
            foreach (XmlNode node in sysxmlnodelist)
            {
                if(node.Name.Equals("ProtHeadData"))
                {
                    XmlAttributeCollection Xacollect = node.Attributes;
                    foreach (XmlAttribute att in Xacollect)
                    {
                        sys_dict.Add(att.Name, att.Value);
                    }
                }
                else
                {
                    sys_dict.Add(node.Name,node.InnerText);
                }
                
            }
            
        }
    }
}
