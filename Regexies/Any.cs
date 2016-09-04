using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// .
    /// 任意の一文字にマッチ
    /// </summary>
    public class Any:Regex
    {
        public override string Name
        {
            get { return "Any"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            if (str.Value().Length > 0)
            {
                yield return new AtomicMatch(this,str,1);
            }
        }
    }
}
