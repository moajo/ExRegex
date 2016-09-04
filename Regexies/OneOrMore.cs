using System;
using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// +
    /// １回以上の反復にマッチ
    /// </summary>
    public class OneOrMore : Regex
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

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            foreach (var selfMatch in _target.SimpleMatchings(str, context))
            {
                var next = str.SubString(selfMatch.Length);
                foreach (var nextMatch in SimpleMatchings(next, context))
                {
                    var composite = nextMatch as CompositeMatch;
                    var list = new List<RegexMatch>();
                    if (composite == null)
                    {
                        throw new Exception();
                        list.Add(nextMatch);//到達不能では？要検証
                    }
                    else
                    {
                        list.AddRange(composite.Matches);
                    }
                    list.Insert(0, selfMatch);
                    yield return new CompositeMatch(this, str, list.ToArray());
                }
                yield return new CompositeMatch(this, str, selfMatch);
            }
        }
    }
}
