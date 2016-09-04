using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// ^
    /// 行頭にマッチ
    /// </summary>
    public class Head:Regex
    {
        public override string Name
        {
            get { return "Head"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            if (str.Pointer == 0)
            {
                yield return new PositionMatch(this,str);
            }
        }
    }
}
