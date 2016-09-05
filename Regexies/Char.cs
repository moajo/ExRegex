using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// 一文字にマッチ
    /// </summary>
    public class Char:Regex
    {
        private readonly char _content;

        public Char(char c)
        {
            _content = c;
        }
        public override string Name
        {
            get { return "char"; }
        }

        public override Regex Clone()
        {
            return new Char(_content);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            var s = new string(new char[] {_content});
            if (str.Value().StartsWith(s))
            {
                yield return new AtomicMatch(this, str,1);
            }
        }

        /// <summary>
        /// C#はキャストをオーバーロードできる！！！最高！！！
        /// </summary>
        /// <param name="c"></param>
        public static implicit operator Char(char c)
        {
            return new Char(c);
        }
    }
}
