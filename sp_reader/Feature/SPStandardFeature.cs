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
    public class SPStandardFeature : ISPStandardFeature
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
        virtual public List<double> Deviations
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
        List<FeatureMeasurePoint> RawPoints
        {
            get;
            set;
        }
        public List<FeatureMeasurePoint> MeasurePoints
        {
            get;
            set;
        }
        public List<FeatureMeasurePoint> FeatureAlignmentPoints
        {
            get;
            set;
        }
        Alignment SPAlignment
        {
            get;
            set;
        }
        public Alignment FeatureAlignment
        {
            get;
            private set;
        }
        #endregion

        public SPStandardFeature(Element element)
        {
            SPAlignment = element.CurrentAlignment;

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
            RawPoints = new List<FeatureMeasurePoint>();
            var measMaskedPoints = new List<FeatureMeasurePoint>();
            int seq_nr = 1;
            while ((line = meas_sr.ReadLine()) != null)
            {
                FeatureMeasurePoint temp_Meas_point = new FeatureMeasurePoint(line);
                temp_Meas_point.seq = seq_nr++;

                if (temp_Meas_point.status == 0)
                {
                    RawPoints.Add(temp_Meas_point);
                }
                else
                {
                    measMaskedPoints.Add(temp_Meas_point);
                }
            }

            MeasurePoints = new List<FeatureMeasurePoint>();
            foreach (FeatureMeasurePoint point in RawPoints)
            {
                MeasurePoints.Add(point * SPAlignment);
            }

            FeatureAlignment = new Alignment(Vector, Position, Identifier + "_alignment");
            
            FeatureAlignmentPoints = new List<FeatureMeasurePoint>();
            foreach (FeatureMeasurePoint point in MeasurePoints)
            {
                FeatureAlignmentPoints.Add(point * FeatureAlignment);
            }

        }
    }
}
