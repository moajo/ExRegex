using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// |
    /// いずれかがマッチすればマッチ。
    /// </summary>
    public class Or:Regex
    {
        private readonly Regex[] _regexes;

        public Or(params Regex[] regexes)
        {
            _regexes = regexes;
        }

        public override string Name
        {
            get { return "Or"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str)
        {
            foreach (var regex in _regexes)
            {
                foreach (var match in regex.SimpleMatchings(str))
                {
                    yield return new CompositeMatch(this, str, match);
                }
            }
        }
    }
}
