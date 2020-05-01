using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPInterface.Evaluation
{
    public class Filter
    {
        FilterType FilterType { get; set; }
        FilterMethod FilterMethod { get; set; }
        ProfileType ProfileType { get; set; }
        PaddingMethod PaddingMethod { get; set; }
        protected double[] _window;

        public Filter(int windowSize,
            FilterType filterType = FilterType.LowPass,
            FilterMethod filterMethod = FilterMethod.Gaussian,
            ProfileType profileType = ProfileType.Closed)
        {
            _window = calcFilterWindow(windowSize, FilterMethod, FilterType);
        }

        protected double[] PreProcessSample(double[] sample)
        {
            int paddingSize = _window.Count();
            int sampleSize = sample.Count();

            double[] prePaddingArray = new double[paddingSize];
            double[] postPaddingArray = new double[paddingSize];

            switch (ProfileType)
            {
                case ProfileType.Open:
                    {
                        for (int i = 0; i < paddingSize; i++)
                        {
                            prePaddingArray[i] = sample.First();
                            postPaddingArray[i] = sample.Last();
                        }
                    }
                    break;
                case ProfileType.Closed:
                    {
                        for (int i = 0; i < paddingSize; i++)
                        {
                            prePaddingArray[i] = sample[sampleSize - paddingSize + i];
                            postPaddingArray[i] = sample[i];
                        }
                    }
                    break;
                case ProfileType.Unknown:
                    return sample;
                default:
                    throw new NotImplementedException();
            }
            return prePaddingArray.Concat(sample).Concat(postPaddingArray).ToArray();

        }
        public double[] ProcessSample(double[] sample)
        {
            double[] preProcessSample = PreProcessSample(sample);
            double[] preResult = Convolution(preProcessSample, _window);
            double[] result = preResult.Skip(_window.Count()).Take(sample.Count()).ToArray();
            return result;
        }



        #region static function for window calculation
        static double[] calcFilterWindow(int windowsize, FilterMethod method = FilterMethod.Gaussian, FilterType ptype = FilterType.LowPass)
        {
            if (method == FilterMethod.Gaussian)
            {
                return calcGaussFilterWindow(windowsize, ptype);
            }
            throw new NotImplementedException();
        }
        static double[] calcGaussFilterWindow(int windowsize, FilterType ptype = FilterType.LowPass)
        {
            double lamda = Convert.ToDouble(windowsize);
            double factor = 1 / 0.4697;// / lamda; 0.4697 = sqrt(ln2/PI) defined by ISO/TS 16610-21 

            int len = (windowsize - 1) / 2;

            double[] g = new double[windowsize];

            for (int i = 0; i < windowsize; ++i)
            {
                g[i] = Math.Exp(
                    -Math.PI *
                    Math.Pow((i - len) / Convert.ToDouble(len), 2) *
                    Math.Pow(factor, 2)
                    );
            }
            double ds = g.Sum();
            for (int i = 0; i < windowsize; ++i)
                g[i] /= ds;


            if (ptype == FilterType.HighPass)
            {
                double[] h = new double[windowsize];
                h[len] = 1;
                for (int i = 0; i < windowsize; ++i)
                {
                    g[i] = h[i] - g[i];
                }
            }
            return g;
        }
        #endregion
        #region static function for Convolution
        static double[] Convolution(double[] source, double[] window)
        {

            int source_len = source.Count();
            int window_len = window.Count();

            double[] result = new double[source_len];

            int half_len = window_len >> 1;
            for (int i = half_len; i < source_len - half_len; ++i)
            {

                double ys = 0;
                for (int j = 0; j < window_len; j++)
                {
                    ys += source[i - half_len + j] * window[j];
                }
                result[i] = ys;
            }
            return result;
        }
        #endregion
    }


}
