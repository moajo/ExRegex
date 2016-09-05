using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// (?<=)
    /// 肯定先読み
    /// </summary>
    public class PositiveLookbehind:Regex
    {
        private readonly Regex _target;
        private readonly Regex _condition;

        public PositiveLookbehind(Regex target, Regex condition)
        {
            _target = target;
            _condition = condition;
        }
        public override string Name
        {
            get { return "(?<=)"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            foreach (var targetMatch in _target.SimpleMatchings(str, context))
            {
                var nextPos = str.RawStr.Substring(0, str.Pointer);//ここより前の文字列
                var conditionMatch = _condition.TailMatches((StringPointer) nextPos, context).FirstOrDefault();//条件の直前へのマッチ
                if (conditionMatch != null)
                {
                    yield return new CompositeMatch(this, str, targetMatch);
                }
            }
        }
    }
}
