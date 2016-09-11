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
        public string CaptureName { get; } = "";

        public Capture(Regex content)
        {
            _content = content;
        }

        public Capture(string name, Regex content) : this(content)
        {
            CaptureName = name;
        }
        public override string Name
        {
            get { return "Capture"; }
        }

        protected override Regex SingleClone()
        {
            return new Capture(_content);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            foreach (var match in _content.HeadMatches(str,context))
            {
                yield return new CompositeMatch(this,str,match);
            }
        }

        protected override string StructureString()
        {
            return String.Format("{0}\n{1}",ToString(),"  "+_content.ToStructureString().Replace("\n","\n  "));
        }
    }
}
