using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
namespace SPInterface
{
    public class SPI
    {
        private SPIconf spiconf;
        public List<Element> elements;
        public readonly string CharacterName;
        public readonly Dictionary<string, string> sys_dict;
        public XmlDocument xml_result;
        string xmloutpath;
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
            StreamWriter sw = new StreamWriter(spiconf.pathFromSP + @"\DataFromSpecialProgram.txt");
            sw.WriteLine("ResultsFromSpecialProgram.xml");
            sw.Close();

            xmloutpath = spiconf.pathFromSP + @"\ResultsFromSpecialProgram.xml";

            System.IO.File.Delete(xmloutpath);
            xml_result = new XmlDocument();
            XmlElement rootElement = xml_result.CreateElement("GeometryData");
            xml_result.AppendChild(rootElement);
            XmlElement spname = xml_result.CreateElement("Name");
            spname.InnerText = CharacterName;
            rootElement.AppendChild(spname);
            result_save();
            
        }
        public void result_save()
        {
            xml_result.Save(xmloutpath);
        }
        public void addresult(string groupid, string typesymbol, string identifier, double act)
        {
            XmlNode current_node = null;
            XmlNode rootnode = xml_result.SelectSingleNode("GeometryData");
            foreach (XmlNode xn in rootnode.ChildNodes)
            {
                
                XmlNode gid = xn.SelectSingleNode("GroupId");
                if (gid != null && gid.InnerText == groupid)
                {
                    current_node = xn;
                    break;
                }
            }
            if (current_node == null)
            {
                XmlElement new_ele = xml_result.CreateElement("Element");
                rootnode.AppendChild(new_ele);
                XmlElement name_ele = xml_result.CreateElement("GroupId");
                name_ele.InnerText = groupid;
                new_ele.AppendChild(name_ele);
                current_node = rootnode.LastChild;
            }
            XmlElement item = xml_result.CreateElement("Line");
            item.SetAttribute("TypeSymbol" ,typesymbol);
            item.SetAttribute("Identifier", identifier);
            item.SetAttribute("Actual", act.ToString("G4"));
            current_node.AppendChild(item);

        }
    }
        
}
