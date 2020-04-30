using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPInterface.RawPoint;
using System.Security.Cryptography.X509Certificates;

namespace SPInterface.Feature
{
    /// <summary>
    /// interface for standard feature exclude curve
    /// </summary>
    interface ISPStandardFeature
    {
        DenseVector Vector
        {
            get;
        }
        DenseVector Position
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
        double X { get; }
        double Y { get; }
        double Z { get; }
        double I { get; }
        double J { get; }
        double K { get; }
    }
}
