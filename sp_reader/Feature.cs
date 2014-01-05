using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPInterface
{
    internal class Feature
    {
        string geoType;
        Dictionary<string, string> xml_paras;
        string identifier;
        
        protected double form_error { get; set; }
        protected double max_dev { get; set; }
        protected double min_dev { get; set; }

        internal Feature(XmlNode node)
        {
            geoType = node.Attributes["GeoType"].Value;
            identifier = node.Attributes["Identifier"].Value;
            xml_paras = new Dictionary<string, string>();
            foreach (XmlAttribute xmlatt in node.Attributes)
            {
                xml_paras.Add(xmlatt.Name, xmlatt.Value);
            }

        }
        public double formError()
        {
            return form_error;
        }
        public double maxDev()
        {
            return max_dev;
        }
        public double minDev()
        {
            return min_dev;
        }

    }
}
