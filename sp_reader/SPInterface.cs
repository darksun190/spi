using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using SPInterface.Core;

namespace SPInterface
{
    /// <summary>
    /// 调用的主Class，通过定义配置文件来初始化类
    /// </summary>
    public class SPInterface : IDisposable
    {
        #region Properties
        /// <summary>
        /// features which exported by CALYPSO
        /// </summary>
        public List<Element> Elements
        {
            get
            {
                return elements;
            }
        }

        private List<Element> elements = new List<Element>();
        public string CharacterName
        {
            get; set;
        }
        /// <summary>
        /// system parameters given by CALYPSO
        /// </summary>
        public Dictionary<string, string> SystemParameter
        {
            get
            {
                return sys_dict;
            }
        }
        private Dictionary<string, string> sys_dict = new Dictionary<string, string>();
        private XmlDocument xml_result;
        static string XMLOutputPath
        {
            get; set;
        }

        public Alignment CurrentAlignment
        {
            get; set;
        }
        SPInterfaceParameters sp_paras;
        public string PathFromSpecialProgram
        {
            get
            {
                return sp_paras.PathFromSP;
            }
        }
        public string PathToSpecialProgram
        {
            get
            {
                return sp_paras.PathToSP;
            }
        }
        #endregion
        /// <summary>
        /// initialize the class by offering the configuration file path
        /// </summary>
        /// <param name="conf_path"> the path was defined by file "~\conf\configCFSpecialProgram.txt".
        /// one line for each special program "Name, Exe path, conf path".
        /// put the 3rd para here</param>
        /// <param name="working_directory"> home directory for this special program</param>
        public SPInterface(string conf_path, string working_directory)
        {
            Environment.CurrentDirectory = working_directory;

            //get the main 3 parameters from the conf file
            //these 3 paras was defined by special program developer
            sp_paras = new SPInterfaceParameters(conf_path);

            //the entry point file, which calypso give some information to SP, the path defined by sp developer
            //filename was fixed by CALYPSO
            string xmlpath = sp_paras.PathToSP + @"\ElementsToSpecialProgram.xml";

            #region Load Name, Features & Alignment
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlpath);
            XmlNode xmlnode = xmldoc.SelectSingleNode("GeometryData");
            XmlNodeList xmlnodelist = xmlnode.ChildNodes;
            CurrentAlignment = new Alignment();
            foreach (XmlNode node in xmlnodelist)
            {
                string identify = node.Name;
                if (identify.Equals("Name"))
                {
                    CharacterName = node.InnerText;
                }
                if (identify.Equals("CoordinateSystem"))
                {
                    CurrentAlignment = new Alignment(node);
                }
                if (identify.Equals("Element"))
                {
                    try
                    {
                        Element element = new Element(node, CurrentAlignment);
                        elements.Add(element);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                }
            }
            #endregion

            #region Load CALYPSO parameters
            string syspath = sp_paras.PathToSP + @"\SysParaToSpecialProgram.xml";
            XmlDocument sysxmldoc = new XmlDocument();
            sysxmldoc.Load(syspath);
            XmlNode sysxmlnode = sysxmldoc.SelectSingleNode("SystemParameters");
            XmlNodeList sysxmlnodelist = sysxmlnode.ChildNodes;
            foreach (XmlNode node in sysxmlnodelist)
            {
                if (node.Name.Equals("ProtHeadData"))
                {
                    XmlAttributeCollection Xacollect = node.Attributes;
                    foreach (XmlAttribute att in Xacollect)
                    {
                        sys_dict.Add(att.Name, att.Value);
                    }
                }
                else
                {
                    sys_dict.Add(node.Name, node.InnerText);
                }

            }
            #endregion

            //write a file which CALYPSO check
            StreamWriter sw = new StreamWriter(sp_paras.PathFromSP + @"\DataFromSpecialProgram.txt");
            //the name is fixed everytime, CALYPSO read this file to get data from Special program
            sw.WriteLine("ResultsFromSpecialProgram.xml");
            sw.Close();

            //writing result
            XMLOutputPath = sp_paras.PathFromSP + @"\ResultsFromSpecialProgram.xml";


            //create a empty xml result, if nothing happen.
            System.IO.File.Delete(XMLOutputPath);
            xml_result = new XmlDocument();
            XmlElement rootElement = xml_result.CreateElement("GeometryData");
            xml_result.AppendChild(rootElement);
            XmlElement spname = xml_result.CreateElement("Name");
            spname.InnerText = CharacterName;
            rootElement.AppendChild(spname);

        }
        /// <summary>
        /// add one line to the result
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="typesymbol"></param>
        /// <param name="identifier"></param>
        /// <param name="act"></param>
        /// <param name="comment"></param>
        /// <param name="nom"></param>
        /// <param name="ut"></param>
        /// <param name="lt"></param>
        public void addresult(string groupid, string typesymbol, string identifier, double act, string comment = "", double nom = 0, double ut = 0, double lt = 0)
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
            item.SetAttribute("TypeName", typesymbol);
            item.SetAttribute("Identifier", identifier);
            item.SetAttribute("Actual", act.ToString("G4"));
            item.SetAttribute("Nominal", nom.ToString("G4"));
            item.SetAttribute("UpperTol", ut.ToString("G4"));
            item.SetAttribute("LowerTol", lt.ToString("G4"));
            item.SetAttribute("Comment", comment);
            current_node.AppendChild(item);
        }
        public void Dispose()
        {
            xml_result.Save(XMLOutputPath);
        }
        /// <summary>
        /// 子类，保存SP的三个参数
        /// </summary>
        public class SPInterfaceParameters
        {

            public string PathFromSP
            {
                get;
                private set;
            }
            public string PathToSP
            {
                get;
                private set;
            }
            public int Timeout
            {
                get;
                private set;
            }
            /// <summary>
            /// the main interface with Calypso
            /// </summary>
            public SPInterfaceParameters(string para_path)
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(para_path);
                XmlNode xmlnode = xmldoc.SelectSingleNode("ConfigFileTest");
                XmlNodeList xmlnodelist = xmlnode.ChildNodes;
                foreach (XmlNode node in xmlnodelist)
                {
                    if (node.Name.Equals("PathToSP", StringComparison.CurrentCultureIgnoreCase))
                    {
                        PathToSP = new DirectoryInfo(node.InnerText).FullName;
                    }
                    if (node.Name.Equals("PathFromSP", StringComparison.CurrentCultureIgnoreCase))
                    {
                        PathFromSP = new DirectoryInfo(node.InnerText).FullName;
                    }
                    if (node.Name.Equals("Timeout", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Timeout = Convert.ToInt32(node.InnerText);
                    }
                }
            }
        }
    }

}
