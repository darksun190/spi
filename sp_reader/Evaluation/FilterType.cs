using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPInterface.Evaluation
{
    public enum FilterType
    {
        LowPass,
        HighPass,
        BandPass,
        BandStop
    }
    public enum FilterMethod
    {
        Gaussian,
        RC,
        Spline
    }
    public enum ProfileType
    {
        /// <summary>
        /// Line or Curve
        /// </summary>
        Open,
        /// <summary>
        /// Circle or Eclipse
        /// </summary>
        Closed,
        Unknown
    }
    public enum PaddingMethod
    {
        Constant,
        Mean,
        Line
    }
}
