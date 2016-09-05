using System;
using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// 普通の正規表現にはない？
    /// 否定。マッチしなければその位置にマッチ。
    /// </summary>
    public class Not : Regex
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

        public override Regex Clone()
        {
            return new Not(_arg);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            foreach (var match in _arg.SimpleMatchings(str, context))
            {
                yield break;
            }
            yield return new PositionMatch(this, str);
        }
        protected override string StructureString()
        {
            return String.Format("{0}\n [target]\n{1}", ToString(), "  " + _arg.ToStructureString().Replace("\n", "\n  "));
        }
    }
}
