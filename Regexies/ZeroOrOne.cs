using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// ?
    /// 0回または1回にマッチ
    /// </summary>
    public class ZeroOrOne : Regex
    {
        private readonly Regex _target;

        public ZeroOrOne(Regex target)
        {
            _target = target;
        }

        public override string Name
        {
            get { return "?"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str)
        {
            var match = _target.HeadMatch(str);
            if (match != null)
            {
                yield return new CompositeMatch(this, str, match);
            }
            yield return new PositionMatch(this,str);

        }
    }
}
