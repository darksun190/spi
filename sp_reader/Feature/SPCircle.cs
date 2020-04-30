using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using SPInterface.Feature;
using SPInterface.Core;

namespace SPInterface.Feature
{
    public class SPCircle : SPStandardFeature
    {
        public double Radius
        {
            get;
            private set;
        }
        bool Inside
        {
            get;
            set;
        }

        public SPCircle(Element element)
            : base(element)
        {
            if (element.GeoType != FeatureType.Type.Circle && element.GeoType != FeatureType.Type.Cylinder)
                throw (new Exception("geoType error"));

            Radius = Convert.ToDouble(Parameters["Radius"]);
            Inside = Convert.ToBoolean(Parameters["InverseOrientation"]);
        }

        public override List<double> Deviations
        {
            get
            {
                List<double> _devs = new List<double>();

                foreach (var p in FeatureAlignmentPoints)
                {

                    _devs.Add(Math.Sqrt(p.X * p.X + p.Y * p.Y) - Radius);
                }
                return _devs;
            }
        }

     
    }
}
