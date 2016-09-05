using System;
using System.Collections.Generic;
using System.Linq;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// |
    /// いずれかがマッチすればマッチ。
    /// </summary>
    public class Or : Regex
    {
        private readonly Regex[] _regexes;

        public Or(params Regex[] regexes)
        {
            _regexes = regexes.ToArray();
        }

        public override string Name
        {
            get { return "Or"; }
        }

        public override Regex Clone()
        {
            return new Or(_regexes);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            foreach (var regex in _regexes)
            {
                foreach (var match in regex.HeadMatches(str, context))
                {
                    yield return new CompositeMatch(this, str, match);
                }
            }
        }
        protected override string StructureString()
        {
            return String.Format("{0}{{\n{1}\n}}", ToString(), string.Join("\n", _regexes.Select(match => "  " + match.ToStructureString().Replace("\n", "\n  "))));
        }
    }
}
