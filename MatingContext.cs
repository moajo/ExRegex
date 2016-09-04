using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExRegex
{
    public class MatingContext
    {
        public Dictionary<string, Regex> NamedRegexes = new Dictionary<string, Regex>();
        public List<string> Captures = new List<string>();
        public int ReferenceDepth { get; private set; }

        public MatingContext DepthIncrement()
        {
            var mc = new MatingContext();
            mc.NamedRegexes = NamedRegexes;
            mc.Captures = Captures;
            mc.ReferenceDepth = ReferenceDepth+1;
            return mc;
        }
    }
}
