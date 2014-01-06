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
        public string geoType;
        public Dictionary<string, string> xml_paras;
        public string identifier;
        protected Alignment feature_alignment;
  
        protected double form_error { get; set; }
        protected double max_dev { get; set; }
        protected double min_dev { get; set; }
        protected List<MeasPoint> feature_alignment_points;
        protected List<MeasPoint> measPoints;
        protected List<MeasPoint> measMaskedPoints;

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
