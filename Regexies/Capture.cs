using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// ()
    /// キャプチャ
    /// </summary>
    public class Capture : Regex
    {
        private readonly Regex _content;

        public Capture(Regex content)
        {
            _content = content;
        }
        public override string Name
        {
            get { return "Capture"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            foreach (var match in _content.SimpleMatchings(str,context))
            {
                yield return new CompositeMatch(this,str,match);
            }
        }

        public override string ToString()
        {
            return String.Format("{0}[content:{1}]", base.ToString(), _content.ToStructureString());
        }
    }
}
