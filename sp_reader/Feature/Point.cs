using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using SPInterface.Core;

namespace SPInterface.Feature
{
    class Point : StandardFeature
    {
        public Point(Element element):
            base(element)
        {
        }
        public override string ToString()
        {
            return string.Format($"{X}, {Y}, {Z}, {I}, {J}, {K}");
        }
        new List<double> Deviations
        {

        }
    }
}
