using System;
using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// リテラル。そのまんまマッチ
    /// </summary>
    public class Literal:Regex
    {
        private readonly string str;
        public Literal(string str)
        {
            this.str = str;
        }

        public override string Name
        {
            get { return "Literal"; }
        }

        public override Regex Clone()
        {
            return new Literal(str);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            if (str.Value().StartsWith(this.str))
            {
                yield return new AtomicMatch(this,str, this.str.Length);
            }
        }

        public override string ToString()
        {
            return String.Format("{0}({1})",Name,str);
        }
    }
}
