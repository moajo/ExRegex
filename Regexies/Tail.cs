using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    public class Tail:Regex
    {
        public override string Name
        {
            get { return "Tail"; }
        }

        protected override Regex SingleClone()
        {
            return new Tail();
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            if (str.Pointer == str.RawStr.Length)
            {
                yield return new PositionMatch(this,str);
            }
        }
    }
}
