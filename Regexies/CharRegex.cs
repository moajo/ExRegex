using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    public abstract class CharRegex:Regex
    {
        public abstract bool CheckChar(char c);

        public sealed override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            if (str.Length == 0)
            {
                yield break;
            }
            var sub = str.Value()[0];
            if (CheckChar(sub))
            {
                yield return new AtomicMatch(this,str,1);
            }
        }
    }
}
