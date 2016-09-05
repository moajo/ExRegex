using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// 指定したRegexに名前つけて再利用可能に
    /// </summary>
    public class Named : Regex
    {
        private readonly Regex _content;
        private readonly string _label;

        public Named(string label, Regex content)
        {
            _label = label;
            _content = content;
        }
        public override string Name
        {
            get { return "Named"; }
        }

        public override Regex Clone()
        {
            return new Named(_label,_content);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            //NamedはMatchTreeに影響しない
            if (!context.NamedRegexes.ContainsKey(_label))
            {
                context.NamedRegexes.Add(_label, this);
            }
            return _content.HeadMatches(str, context);
        }

        protected override string StructureString()
        {
            return String.Format("Named({0})\n{1}", _label, "  " + _content.ToStructureString().Replace("\n", "\n  "));
        }
    }
}
