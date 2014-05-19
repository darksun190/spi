using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;

namespace SPInterface
{
    public class SectionCircle :Circle
    {
       
        List <CircleSection> _sections;
        List<CircleSection> _merge_sections;

        public SectionCircle(Feature fea)
            : base(fea)
        {
           
        }

        public List<CircleSection> merge_sections
        {
            get
            {
                if (_merge_sections == null)
                {
                    _merge_sections = MergeSections(sections);
                }
                return _merge_sections;
            }
        }

        private List<CircleSection> MergeSections(List<CircleSection> ss)
        {
            List<CircleSection> result = new List<CircleSection>();
            bool[] mask = new bool[ss.Count()];
            for (int i = 0; i < ss.Count(); ++i)
            {
                if (ss[i].Count() < 6)
                    mask[i] = true;
                if (mask[i])
                    continue;
                for (int j = i+1; j < ss.Count(); ++j)
                {
                    if (mask[j])
                        continue;

                    if (ss[i].isOverLap(ss[j]))
                    {
                        mask[i] = true;
                        mask[j] = true;
                        result.Add(ss[i].merge(ss[j]));
                    }
                }
                if (!mask[i])
                    result.Add(ss[i]);
            }
            if (result.Count() == ss.Count())
                return result;
            else
                return MergeSections(result);
        }
        public List<CircleSection> sections
        {
            get
            {
                if (_sections == null)
                {
                    ParserSection();
                }
                return _sections;
            }
        }

        private void ParserSection()
        {
            _sections = new List<CircleSection>();
            _sections.Add(new CircleSection());
            for (int i = 0; i < this.point_no(); ++i)
            {
                CircleSection temp = _sections.Last();
                if (!temp.tryParser(transferedPoints[i]))
                {
                    _sections.Add(new CircleSection());
                    _sections.Last().Add(transferedPoints[i]);
                }
            }

        }
        protected override double calcLead()
        {
            List<double> z_value = new List<double>();
            foreach (MeasPoint p in merge_sections[0])
            {
                z_value.Add(p.z - merge_sections[0][0].z);
            }
            double _lead = Math.Round(((z_value.Average() * 2) / (merge_sections[0].rangeAngle / (2 * Math.PI))) * 4, 0) / 4.0;
            return _lead;
        }
    }
}
