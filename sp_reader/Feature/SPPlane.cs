using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using SPInterface.Core;

namespace SPInterface.Feature
{
    public class SPPlane : SPStandardFeature
    {

        public double Length { get; set; }
        public double Width { get; set; }
        public SPPlane(Element element)
            : base(element)
        {
            if (element.GeoType != FeatureType.Type.Plane)
                throw (new Exception("geoType error"));

            Length = Convert.ToDouble(Parameters["Length"]);
            Width = Convert.ToDouble(Parameters["Width"]);

        }

        public double area
        {
            get
            {
                return Length * Width;
            }
        }

        public override List<double> Deviations
        {
            get
            {
                List<double> _devs = new List<double>();
                foreach (var p in FeatureAlignmentPoints)
                {
                    _devs.Add(p.Z);
                }
                return _devs;
            }
        }


    }
}
