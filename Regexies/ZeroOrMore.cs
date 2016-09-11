using System;
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

        protected override Regex SingleClone()
        {
            return new ZeroOrMore(_target);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)//TODO:fix
        {
            foreach (var selfMatch in _target.HeadMatches(str, context))
            {
                var next = str.SubString(selfMatch.Length);
                foreach (var targetMatch in SimpleMatchings(next, context))
                {
                    var composite = targetMatch as CompositeMatch;
                    var list = new List<RegexMatch>();
                    list.Add(selfMatch);
                    if (composite != null)
                    {
                        list.AddRange(composite.Matches);
                    }
                    else
                    {
                        break;
                    }
                    yield return new CompositeMatch(this, str, list.ToArray());
                }
                yield return new CompositeMatch(this, str, selfMatch);
            }
            yield return new PositionMatch(this,str);
        }
        protected override string StructureString()
        {
            return String.Format("{0}\n [target]\n{1}", ToString(), "  " + _target.ToStructureString().Replace("\n", "\n  "));
        }
    }
}
