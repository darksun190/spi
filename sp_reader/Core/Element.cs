using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MathNet.Numerics.LinearAlgebra.Double;
using SPInterface.Feature;

namespace SPInterface.Core
{
    /// <summary>
    /// Element is raw information from CALYPSO
    /// raw xml text & attribute
    /// without any position or vector
    /// </summary>
    public class Element 
    {
        string rawText;
        public FeatureType.Type GeoType
        {
            get;
            private set;
        }
        public Dictionary<string, string> Parameters
        {
            get;
            private set;
        }
        public string Identifier
        {
            get;
            private set;
        }
        
        public Alignment CurrentAlignment
        {
            get;
            private set;
        }
 
        internal Element(XmlNode node, Alignment alignment)
        {
            rawText = node.OuterXml;
            GeoType = FeatureType.getType(node.Attributes["GeoType"].Value);
            Identifier = node.Attributes["Identifier"].Value;
            Parameters = new Dictionary<string, string>();
            foreach (XmlAttribute xmlatt in node.Attributes)
            {
                Parameters.Add(xmlatt.Name, xmlatt.Value);
            }
            CurrentAlignment = alignment;
        }

    }
}
