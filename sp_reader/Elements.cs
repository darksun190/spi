using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPInterface
{
    static class Elements
    {
        static List<Feature> features;
        static Elements()
        {
            features = new List<Feature>();
        }
        public static void Add(XmlNode node)
        {
            features.Add(new Feature(node));
        }
    }
}
