using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// *
    /// 0回以上の反復にマッチ
    /// </summary>
    public class ZeroOrMore : Regex
    {
        private readonly Regex _target;

        public ZeroOrMore(Regex target)
        {
            _target = target;
        }

        public override string Name
        {
            get { return "*"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)//TODO:fix
        {
            foreach (var match2 in _target.SimpleMatchings(str, context))
            {
                var next = str.SubString(match2.Length);
                foreach (var matching in SimpleMatchings(next, context))
                {
                    var composite = matching as CompositeMatch;
                    var list = new List<RegexMatch>();
                    if (composite != null)
                    {
                        list.AddRange(composite.Matches);
                    }
                    else
                    {
                        break;
                    }
                    list.Insert(0, match2);
                    yield return new CompositeMatch(this, str, list.ToArray());
                }
                yield return new CompositeMatch(this, str, match2);
            }
            yield return new PositionMatch(this,str);
        }
    }
}
