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

        public override Regex Clone()
        {
            return new ZeroOrOne(_target);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str,MatingContext context)
        {
            var match = _target.HeadMatch(str,context);
            if (match != null)
            {
                yield return new CompositeMatch(this, str, match);
            }
            yield return new PositionMatch(this,str);

        }
    }
}
