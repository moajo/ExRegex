using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRegex.Match
{
    /// <summary>
    /// 複数のマッチのシーケンスによるマッチ
    /// </summary>
    public class CompositeMatch:RegexMatch
    {
        public RegexMatch[] Matches;

        public override int Length
        {
            get { return Matches.Sum(match => match.Length); }
        }

        public override string MatchStr
        {
            get { return String.Join("", Matches.Select(match => match.MatchStr)); }
        }

        public CompositeMatch(Regex regex,StringPointer str, params RegexMatch[] children) : base(regex,str)
        {
            //ArrayMatchは展開する
            var list = new List<RegexMatch>();
            foreach (var child in children)
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
            Matches = list.ToArray();
        }


        public override string ToString()
        {
            return String.Format("{0}{{\n{1}\n}}", Regex, string.Join("\n", Matches.Select(match => "  " + match.ToString().Replace("\n", "\n  "))));
        }
    }
}
