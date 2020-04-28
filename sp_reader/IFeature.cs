using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPInterface
{
    public interface IFeature
    {
        FeatureType GeoType
        {
            get;
        }
        string Identifier
        {
            get;
        }
        Dictionary<string, string> Parameters
        {
            get;
        }

    }
}
