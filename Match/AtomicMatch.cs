using System;
using System.Collections.Generic;
using ExRegex.Regexies;

namespace ExRegex.Match
{
    /// <summary>
    /// 単一Regexのマッチ
    /// </summary>
    public class AtomicMatch:RegexMatch
    {
        private readonly int _length;
        public AtomicMatch(Regex regex, StringPointer str,int length) : base(regex,str)
        {
            if (str.Length < length)
            {
                throw new Exception("すすめすぎ");
            }
            _length = length;
        }

        public override int Length
        {
            get { return MatchStr.Length; }
        }

        public override string MatchStr
        {
            get { return Str.Value().Substring(0, _length); }
        }

        public override IEnumerable<RegexMatch> GetCaptures()
        {
            yield break;
        }

        public override string ToString()
        {
            return String.Format("{0}{{\n  \"{1}\"\n}}", Regex, MatchStr);
        }
    }
}
