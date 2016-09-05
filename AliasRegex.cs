using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex
{
    /// <summary>
    /// 簡略化表記
    /// </summary>
    public abstract class AliasRegex:Regex
    {
        public abstract Regex Content { get; }
        public sealed override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            foreach (var match in Content.HeadMatches(str,context))
            {
                yield return new CompositeMatch(this,str,match);
            }
        }

        public override string ToString()
        {
            return "(ALIAS)" + base.ToString();
        }
    }
}
