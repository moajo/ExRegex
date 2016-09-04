using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// 普通の正規表現にはない？
    /// 否定。マッチしなければその位置にマッチ。
    /// </summary>
    public class Not:Regex
    {
        private readonly Regex _arg;

        public Not(Regex arg)
        {
            _arg = arg;
        }

        public override string Name
        {
            get { return "Not"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str)
        {
            foreach (var match in _arg.SimpleMatchings(str))
            {
                yield break;
            }
            yield return new PositionMatch(this,str);
        }
    }
}
