using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPInterface
{
    class IStandardFeature
    {
        DenseVector Vector
        {
            get;
        }
        DenseVector Postion
        {
            get;
        }
        double MaxDeviation
        {
            get;
        }
        double MinDeviation
        {
            get;
        }
        double Form
        {
            get;
        }
        double Sigma
        {
            get;
        }
        List<MeasPoint> RawPoints
        {
            get;
        }
        List<MeasPoint> AlignmentPoints
        {
            get;
        }
        double X
        {
            get
            {
                return Postion[0];
            }
        }
        double Y
        {
            get
            {
                return Postion[1];
            }
        }
        double Z
        {
            get
            {
                return Postion[2];
            }
        }
    }
}
