using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SPInterface
{
    public class CurvePara
    {
        public double maxdev, mindev;
        public int maxindex, minindex;
        public bool projected, corrected;

        public CurvePara(XmlNode node)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(node.OuterXml);
            XmlNode temp_node = node.SelectSingleNode("maxDev");
            maxdev = Convert.ToDouble(temp_node.InnerText.Substring(0, 6));
            temp_node = node.SelectSingleNode("minDev");
            mindev = Convert.ToDouble(temp_node.InnerText.Substring(0, 6));
            temp_node = node.SelectSingleNode("indexOfMaxDev");
            maxindex = Convert.ToInt32(temp_node.InnerText);
            temp_node = node.SelectSingleNode("indexOfMinDev");
            minindex = Convert.ToInt32(temp_node.InnerText);

            temp_node = node.SelectSingleNode("isProjected");
            projected = Convert.ToBoolean(temp_node.InnerText);
            temp_node = node.SelectSingleNode("probeCorrection");
            corrected = Convert.ToBoolean(temp_node.InnerText);
       }
    }
}
