using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using SPInterface.Core;

namespace SPInterface.Feature
{
    class SPPoint : SPStandardFeature
    {
        public SPPoint(Element element):
            base(element)
        {
        }
        public override string ToString()
        {
            return string.Format($"{X}, {Y}, {Z}, {I}, {J}, {K}");
        }
        public override List<double> Deviations
        {
            get
            {
                List<double> _devs = new List<double>();
                foreach(var p in FeatureAlignmentPoints)
                {
                    _devs.Add(p.Z - Z);
                }
                return _devs;
            }
        }
    }
}
