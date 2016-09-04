using System.Collections.Generic;
using System.Linq;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// [^]
    /// 指定した文字のどれにもマッチしなければその一文字に一致
    /// </summary>
    public class OrInvert:Regex
    {
        private Char[] _content;

        public OrInvert(params Char[] chars)
        {
            _content = chars;
        }
        public override string Name
        {
            get { return "[^]"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str)
        {
            if (_content.All(c => c.HeadMatch(str) == null))
            {
                yield return new AtomicMatch(this,str,1);
            }
        }
    }
}
