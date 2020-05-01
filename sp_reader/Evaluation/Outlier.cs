using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.Statistics;

namespace SPInterface.Evaluation
{
    public class Outlier<T>
    {
        /// <summary>
        /// Ouside part, plus deviation
        /// </summary>
        public int PlusFactor { get; set; }
        /// <summary>
        /// Inside workpiece, minus deviation
        /// </summary>
        public int MinusFactor { get; set; }

        private List<T> Source { get; set; }
        private Func<T, double> KeySelector { get; set; }

        public double Mean { get; }
        public double Sigma { get; }
        public double UpperTol { get; }
        public double LowerTol { get; }

        /// <summary>
        /// init by a integer for facotr both plus & minus
        /// </summary>
        /// <param name="factor"></param>
        public Outlier(List<T> source, Func<T, double> keySelector, int factor = 3)
            : this(source, keySelector, factor, factor)
        {

        }
        public Outlier(List<T> source, Func<T, double> keySelector, int plusfactor, int minusfactor)
        {
            PlusFactor = plusfactor;
            MinusFactor = minusfactor;
            Source = source;
            KeySelector = keySelector;

            List<double> devs = source.Select(n => KeySelector(n)).ToList();

            Mean = devs.Mean();
            Sigma = devs.StandardDeviation();
            UpperTol = Mean + PlusFactor * Sigma;
            LowerTol = Mean - MinusFactor * Sigma;
        }

        public List<T> OutlierResult
        {
            get
            {
                var result = from u in Source
                             let w = KeySelector(u)
                             where w > LowerTol && w < UpperTol
                             select u;
                return result.ToList();
            }
        }

    }

}
