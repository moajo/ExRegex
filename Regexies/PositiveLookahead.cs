using System;
using System.Collections.Generic;
using System.Linq;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// 肯定先読み
    /// 条件パターンが直後に続けば、対象パターンにマッチ
    /// </summary>
    public class PositiveLookahead:Regex
    {
        private readonly Regex _target;
        private readonly Regex _condition;

        public PositiveLookahead(Regex target, Regex condition)
        {
            _target = target;
            _condition = condition;
        }
        public override string Name
        {
            get { return "(?=)"; }
        }

        protected override Regex SingleClone()
        {
            return new PositiveLookahead(_target,_condition);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            foreach (var targetMatch in _target.SimpleMatchings(str, context))
            {
                var nextPos = str.SubString(targetMatch.Length);
                var conditionMatch = _condition.SimpleMatchings(nextPos,context).FirstOrDefault();
                if (conditionMatch != null)
                {
                    yield return new CompositeMatch(this, str, targetMatch);
                }
            }
        }

        protected override string StructureString()
        {
            return String.Format("{0}\n [condition]\n{1}\n [target]\n{2}", ToString(), "  " + _condition.ToStructureString().Replace("\n", "\n  "), "  " + _target.ToStructureString().Replace("\n", "\n  "));
        }
    }
}
