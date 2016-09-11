using System;
using System.Collections.Generic;
using System.Linq;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// [^]
    /// 指定した文字のどれにもマッチしなければその一文字に一致
    /// </summary>
    public class OrInvert : Regex//TODO:CharRegex?
    {
        private readonly CharRegex[] _content;

        public OrInvert(params CharRegex[] chars)
        {
            _content = chars.ToArray();
        }

        public OrInvert(params char[] chars):this(chars.Select(c => new Char(c) as CharRegex).ToArray())
        {
        }
        public override string Name
        {
            get { return "[^]"; }
        }

        protected override Regex SingleClone()
        {
            return new OrInvert(_content);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            if (str.Length > 0 && _content.All(c => c.HeadMatch(str, context) == null))
            {
                yield return new AtomicMatch(this, str, 1);
            }
        }

        protected override string StructureString()
        {
            return String.Format("{0}{{\n{1}\n}}", ToString(), string.Join("\n", _content.Select(match => "  " + match.ToStructureString().Replace("\n", "\n  "))));
        }
    }
}
