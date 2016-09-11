using System;
using System.Collections.Generic;
using System.Linq;
using ExRegex.Match;
using ExRegex.Regexies;

namespace ExRegex
{
    /// <summary>
    /// 正規表現
    /// </summary>
    public abstract class Regex
    {

        public abstract string Name { get; }
        /// <summary>
        /// 親子関係は考慮しない単純なクローン
        /// </summary>
        /// <returns></returns>
        protected abstract Regex SingleClone();


        /// <summary>
        /// 後続を接続
        /// </summary>
        /// <param name="nextRegex"></param>
        public virtual Regex To(Regex nextRegex)
        {
            var clone = SingleClone();
            if (nextRegex is Empty)
            {
                return clone;
            }
            if (_nextRegex != null)
            {
                clone._nextRegex = _nextRegex.To(nextRegex);
            }
            else
            {
                clone._nextRegex = nextRegex;
            }
            return clone;
        }

        /// <summary>
        /// 範囲重複を許容せず最高優先マッチ箇所をすべて列挙
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IEnumerable<RegexMatch> MatchesRegular(StringPointer str)
        {
            for (int i = 0; i < str.Length + 1; i++)
            {
                var subStr = str.SubString(i);
                var match = HeadMatch(subStr, new MatingContext());
                if (match != null)
                {
                    yield return match;
                    i += Math.Max(match.Length - 1, 0);
                }
            }
        }

        /// <summary>
        /// マッチ箇所を全て列挙。重複箇所も。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IEnumerable<RegexMatch> Matches(string str)
        {
            return Matches((StringPointer)str, new MatingContext());
        }


        /// <summary>
        /// 完全マッチ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IEnumerable<RegexMatch> PerfectMatches(string str)
        {
            return PerfectMatches((StringPointer)str,new MatingContext());
        }


        /// <summary>
        /// 先頭マッチ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IEnumerable<RegexMatch> HeadMatches(string str)
        {
            return HeadMatches((StringPointer)str,new MatingContext());
        }

        /// <summary>
        /// 末尾マッチ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IEnumerable<RegexMatch> TailMatches(string str)
        {
            return TailMatches((StringPointer)str,new MatingContext());
        }

        /// <summary>
        /// 最初の完全マッチ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public RegexMatch PerfectMatch(string str)
        {
            return PerfectMatches(str).FirstOrDefault();
        }


        /// <summary>
        /// 最初の先頭マッチ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public RegexMatch HeadMatch(string str)
        {
            return HeadMatches(str).FirstOrDefault();
        }

        /// <summary>
        /// 最初の末尾マッチ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public RegexMatch TailMatch(string str)
        {
            return TailMatches(str).FirstOrDefault();
        }




        /// <summary>
        /// SingleCloneして親子関係も複製
        /// </summary>
        /// <returns></returns>
        public Regex Clone()
        {
            var clone = SingleClone();
            if (_nextRegex != null)
            {
                clone._nextRegex = _nextRegex.Clone();
            }
            return clone;
        }








        protected IEnumerable<RegexMatch> PerfectMatches(StringPointer str, MatingContext context)
        {
            return HeadMatches(str, context).Where(m => m.Length == str.Length);
        }

        private Regex _nextRegex;



        /// <summary>
        /// このRegexChainの再優先マッチを取得。マッチしなければnull
        /// </summary>
        /// <param name="str"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public RegexMatch HeadMatch(StringPointer str, MatingContext context)
        {
            return HeadMatches(str,context).FirstOrDefault();
        }

        public bool IsPerfectMatch(string str)
        {
            return PerfectMatch(str) != null;
        }



        public bool IsHeadMatch(string str)
        {
            return HeadMatch((StringPointer) str, new MatingContext()) != null;
        }

        /// <summary>
        /// 文字列の先頭から、後続を考慮したマッチを優先度順に列挙
        /// </summary>
        /// <param name="str"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<RegexMatch> HeadMatches(StringPointer str,MatingContext context)
        {
            if (_nextRegex == null)//末端は自身のマッチを返せばいい。
            {
                foreach (var matching in SimpleMatchings(str, context))
                {
                    yield return matching;
                }
            }
            else//末端以外は、自分以降にマッチが存在するマッチを返す。
            {
                foreach (var selfMatch in SimpleMatchings(str, context))
                {
                    var nextStr = str.SubString(selfMatch.Length);
                    foreach (var nextMatch in _nextRegex.HeadMatches(nextStr, context))
                    {
                        yield return new ArrayMatch(str, selfMatch, nextMatch);
                    }
                }
            }
        }

        /// <summary>
        /// 文字列の末尾で終わる、後続を考慮したマッチを優先度順に列挙
        /// (後よみ用？)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<RegexMatch> TailMatches(StringPointer str, MatingContext context)
        {
            return Matches(str,context).Where(match => match.AfterStr == "");
        }

        /// <summary>
        /// テキスト全体でマッチ箇所を全て列挙
        /// </summary>
        /// <param name="str"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<RegexMatch> Matches(StringPointer str, MatingContext context)
        {
            for (int i = 0; i < str.Length + 1; i++)
            {
                var subStr = str.SubString(i);
                foreach (var match in HeadMatches(subStr,new MatingContext()))
                {
                    yield return match;
                }
            }
        }


        
        /// <summary>
        /// 与えられた文字列が先頭からこの正規表現にマッチするか判定。後続のことは考慮しない。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="context"></param>
        /// <returns>マッチ優先度高い順のマッチ。マッチしないときは空のIEnumerable</returns>
        public abstract IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context);


        /// <summary>
        /// 空のRegex作るだけ
        /// </summary>
        /// <returns></returns>
        public static Regex Make()
        {
            return new Empty();
        }

        /// <summary>
        /// C#はキャストをオーバーロードできる！！すごい！！
        /// </summary>
        /// <param name="str"></param>
        public static implicit operator Regex(string str)
        {
            return new Literal(str);
        }


        public override string ToString()
        {
            return Name;
        }

        protected virtual string StructureString()
        {
            return ToString();
        }

        public string ToStructureString()
        {
            return String.Format("{0}{1}",StructureString(),_nextRegex!=null?"\n"+_nextRegex.ToStructureString():"");
        }
    }
}
