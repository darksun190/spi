using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SPInterface
{
    public class Feature
    {
        public FeatureType geoType;
        public Dictionary<string, string> xml_paras;
        public string identifier;
        protected Alignment feature_alignment;
  
        protected double form_error { get; set; }
        protected double max_dev { get; set; }
        protected double min_dev { get; set; }
        protected List<MeasPoint> feature_alignment_points;
        protected List<MeasPoint> measPoints;
        protected List<MeasPoint> measMaskedPoints;
        protected List<double> _devs;

        public Feature ConvertType(FeatureType geoType)
        {
            switch (geoType)
            {
                case FeatureType.Circle:
                    return new Circle(this);
                case FeatureType.Cylinder:
                    return new Cylinder(this);
                case FeatureType.Plane:
                    return new Plane(this);
                case FeatureType.Point:
                    return new Point(this);
                case FeatureType.Curve:
                    return new Curve(this);
                case FeatureType.Cone:
                    throw new Exception("didn't implement");
                default:
                    throw new Exception("unkown type");
            }
        }
        public Feature ConvertType()
        {
            return ConvertType(this.geoType);
        }
        public int point_no
        {
            get
            {
                if (measPoints == null)
                    return 0;
                else
                    return measPoints.Count;
            }
        }
        
        protected Feature()
        {
        }
        FeatureType getType(string type)
        {
            switch (type.ToLower())
            {
                case  "cylinder":
                    return FeatureType.Cylinder;
                case "circle":
                    return FeatureType.Circle;
                case "point":
                    return FeatureType.Plane;
                case "curve":
                    return FeatureType.Curve;
                case "plane":
                    return FeatureType.Plane;
                case "cone":
                    return FeatureType.Cone;
                default:
                    return FeatureType.Unknown;
            }
        }
        internal Feature(XmlNode node)
        {
            geoType = getType(node.Attributes["GeoType"].Value);
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
        virtual public List<double> Deviations
        {get;private set;}
        
    }
}
