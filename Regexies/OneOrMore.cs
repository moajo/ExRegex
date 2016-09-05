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

        public override Regex Clone()
        {
            return new OneOrMore(_target);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            foreach (var selfMatch in _target.HeadMatches(str, context))
            {
                var next = str.SubString(selfMatch.Length);
                foreach (var nextMatch in SimpleMatchings(next, context))
                {
                    var list = new List<RegexMatch>();
                    list.Add(selfMatch);
                    var composite = (CompositeMatch)nextMatch;
                    list.AddRange(composite.Matches);
                    yield return new CompositeMatch(this, str, list.ToArray());
                }
                yield return new CompositeMatch(this, str, selfMatch);
            }
        }
        protected override string StructureString()
        {
            return String.Format("{0}\n [target]\n{1}", ToString(), "  " + _target.ToStructureString().Replace("\n", "\n  "));
        }
    }
}
