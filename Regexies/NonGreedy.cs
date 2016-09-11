using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// 優先度の反転。（重いかも）//TODO:軽くする
    /// </summary>
    public class NonGreedy:Regex
    {
        private readonly Regex _content;

        public NonGreedy(Regex content)
        {
            _content = content;
        }
        public override string Name
        {
            get { return "non-greedy"; }
        }

        protected override Regex SingleClone()
        {
            return new NonGreedy(_content);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            var matches = _content.HeadMatches(str, context).ToList();
            for (int i = 0; i < matches.Count; i++)
            {
                yield return matches[matches.Count - 1 - i];
            }
        }
    }
}
