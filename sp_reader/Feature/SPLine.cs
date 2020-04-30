using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using SPInterface.Core;

namespace SPInterface.Feature
{
    public class SPLine : SPStandardFeature
    {

        public double Length
        {
            get;
            private set;
        }
        public SPLine(Element element)
            : base(element)
        {
            if (element.GeoType != FeatureType.Type.Line)
                throw (new Exception("geoType error"));

            Length = Convert.ToDouble(Parameters["Length"]);
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
