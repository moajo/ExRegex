using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// +
    /// １回以上の反復にマッチ
    /// </summary>
    public class OneOrMore:Regex
    {
        private readonly Regex _target;

        public OneOrMore(Regex target)
        {
            _target = target;
        }

        public override string Name
        {
            get { return "+"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str)
        {
            foreach (var match2 in _target.SimpleMatchings(str))//TODO fix
            {
                var next = str.SubString(match2.Length);
                foreach (var matching in SimpleMatchings(next))
                {
                    var composite = matching as CompositeMatch;
                    var list = new List<RegexMatch>();
                    if (composite == null)
                    {
                        list.Add(matching);
                    }
                    else
                    {
                        list.AddRange(composite.Matches);
                    }
                    list.Insert(0,match2);
                    yield return new CompositeMatch(this,str,list.ToArray());
                }
                yield return new CompositeMatch(this, str, match2);
            }
        }
    }
}
