using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using SPInterface.RawPoint;
using SPInterface.Core;
using System.Security.AccessControl;
using MathNet.Numerics.Statistics;

namespace SPInterface.Feature
{
    /// <summary>
    /// interface just a definition
    /// using this class for base function to implement all properties & function
    /// </summary>
    class StandardFeature : IStandardFeature
    {
        #region Properties
        public FeatureType.Type GeoType
        {
            get;
        }
        public string Identifier
        {
            get;
        }
        public Dictionary<string, string> Parameters
        {
            get;
        }

        public DenseVector Vector
        {
            get;
            private set;
        }
     
        public DenseVector Position
        {
            get;
            private set;
        }
        public double X
        {
            get
            {
                return Position[0];
            }
        }
        public double Y
        {
            get
            {
                return Position[1];
            }
        }
        public double Z
        {
            get
            {
                return Position[2];
            }
        }
        public double I
        {
            get
            {
                return Vector[0];
            }
        }
        public double J
        {
            get
            {
                return Vector[1];
            }
        }
        public double K
        {
            get
            {
                return Vector[2];
            }
        }
        /// <summary>
        /// Deviation is the most important property
        /// need all sub class implement by itself.
        /// </summary>
        virtual internal List<double> Deviations
        {
            get;
            set;
        }
        public double Form
        {
            get
            {
                return MaxDeviation - MinDeviation;
            }
        }
        public double Sigma
        {
            get
            {
                return Deviations.StandardDeviation();
            }
        }
        public double MaxDeviation
        {
            get
            {
                return Deviations.Max();
            }
        }
        public double MinDeviation
        {
            get
            {
                return Deviations.Min();
            }
        }
        List<MeasPoint> RawPoints
        {
            get;
            set;
        }
        List<MeasPoint> FeatureAlignmentPoints
        {
            get
            {
                List<MeasPoint> result = new List<MeasPoint>();
                foreach (MeasPoint point in RawPoints)
                {
                    feature_alignment_points.Add(new MeasPoint(point, feature_alignment));
                }
            }
        }
        public Alignment FeatureAlignment
        {
            get;
            private set;
        }
        #endregion

        public StandardFeature(Element element)
        {
            if (element.GeoType == FeatureType.Type.Curve)
                throw (new Exception("geoType error"));
            this.Parameters = element.Parameters;
            this.Identifier = element.Identifier;
            this.GeoType = element.GeoType;

            //init Vector
            string vector_string = Parameters["Vector"].Trim('\"');
            Vector = new DenseVector(
                vector_string
                .Split(' ')
                .Select(n => Convert.ToDouble(n))
                .ToArray()
                );

            //init Position
            string position_string = Parameters["Position"].Trim('\"');
            Position = new DenseVector(
                position_string
                .Split(' ')
                .Select(n => Convert.ToDouble(n))
                .ToArray()
                );

            //init get meas Points
            string points_file_name = Parameters["Points"];
            FileInfo meas_fi = new FileInfo(points_file_name);
            StreamReader meas_sr = new StreamReader(meas_fi.FullName);
            string line;
            RawPoints = new List<MeasPoint>();
            var measMaskedPoints = new List<MeasPoint>();
            int seq_nr = 1;
            while ((line = meas_sr.ReadLine()) != null)
            {
                MeasPoint temp_Meas_point = new MeasPoint(line, false);
                if (temp_Meas_point.status == 0)
                {
                    RawPoints.Add(temp_Meas_point);
                    RawPoints.Last().seq = seq_nr++;
                }
                else
                {
                    measMaskedPoints.Add(temp_Meas_point);
                    measMaskedPoints.Last().seq = seq_nr++;
                }
            }

            FeatureAlignment = new Alignment(Vector, Position, Identifier + "_alignment");
        }
    }
}
