using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SPInterface
{
    public class Feature : IFeature
    {
        string rawText;
        public FeatureType GeoType
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
     
        FeatureType getType(string type)
        {
            switch (type.ToLower())
            {
                case "cylinder":
                    return FeatureType.Cylinder;
                case "circle":
                    return FeatureType.Circle;
                case "spacepoint":
                case "point":
                    return FeatureType.Point;
                case "curve":
                    return FeatureType.Curve;
                case "plane":
                    return FeatureType.Plane;
                case "cone":
                    return FeatureType.Cone;
                case "line":
                    return FeatureType.Line;
                case "line3d":
                    return FeatureType.Line3d;
                default:
                    return FeatureType.Unknown;
            }
        }
        internal Feature(XmlNode node)
        {
            rawText = node.InnerText;
            GeoType = getType(node.Attributes["GeoType"].Value);
            Identifier = node.Attributes["Identifier"].Value;
            Parameters = new Dictionary<string, string>();
            foreach (XmlAttribute xmlatt in node.Attributes)
            {
                Parameters.Add(xmlatt.Name, xmlatt.Value);
            }
        }

    }
}
