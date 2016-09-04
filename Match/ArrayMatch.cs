using System;
using System.Collections.Generic;
using System.Linq;
using ExRegex.Regexies;

namespace ExRegex.Match
{
    /// <summary>
    /// ただの並列したマッチのリスト
    /// </summary>
    public class ArrayMatch:RegexMatch
    {
        public readonly RegexMatch[] Contents;
        public ArrayMatch(StringPointer str,params RegexMatch[] contents) : base(new Empty(),str)
        {
            //ArrayMatchは展開する
            var list = new List<RegexMatch>();
            foreach (var child in contents)
            {
                var array = child as ArrayMatch;
                if (array == null)
                {
                    list.Add(child);
                }
                else
                {
                    list.AddRange(array.Contents);
                }
            }
            Contents = list.ToArray();
        }

        public override int Length
        {
            get { return Contents.Sum(match => match.Length); }
        }

        public override string MatchStr
        {
            get { return String.Join("", Contents.Select(match => match.MatchStr)); }
        }

        public override IEnumerable<RegexMatch> GetCaptures()
        {
            foreach (var match in Contents)
            {
                foreach (var capture in match.GetCaptures())
                {
                    yield return capture;
                }
            }
        }

        public override string ToString()
        {
            return string.Join("\n", Contents.Select(match => match.ToString()));
        }
    }
}
