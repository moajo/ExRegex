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
        private Regex _nextRegex;

        /// <summary>
        /// 後続を接続
        /// </summary>
        /// <param name="nextRegex"></param>
        public virtual Regex To(Regex nextRegex)
        {
            if (_nextRegex != null)
            {
                _nextRegex.To(nextRegex);
            }
            else
            {
                _nextRegex = nextRegex;
            }
            return this;
        }


        /// <summary>
        /// このRegexChainの再優先マッチを取得
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public RegexMatch HeadMatch(StringPointer str)
        {
            return HeadMatches(str).FirstOrDefault();
        }
        public RegexMatch HeadMatch(string str)
        {
            return HeadMatch(new StringPointer(str));
        }

        /// <summary>
        /// 文字列の先頭から、後続を考慮したマッチを優先度順に列挙
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IEnumerable<RegexMatch> HeadMatches(StringPointer str)
        {
            if (_nextRegex == null)//末端は自身のマッチを返せばいい。
            {
                foreach (var matching in SimpleMatchings(str))
                {
                    yield return matching;
                }
            }
            else//末端以外は、自分以降にマッチが存在するマッチを返す。
            {
                foreach (var selfMatch in SimpleMatchings(str))
                {
                    var nextStr = str.SubString(selfMatch.Length);
                    foreach (var nextMatch in _nextRegex.HeadMatches(nextStr))
                    {
                        yield return new ArrayMatch(str,selfMatch,nextMatch);
                    }
                }
            }
        }

        public bool IsHeadMatch(string str)
        {
            return HeadMatch(str) != null;
        }

        /// <summary>
        /// テキスト全体でマッチ箇所を全て列挙
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IEnumerable<RegexMatch> Matches(StringPointer str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                var subStr = str.SubString(i);
                foreach (var match in HeadMatches(subStr))
                {
                    yield return match;
                }
            }
        }
        public IEnumerable<RegexMatch> Matches(string str)
        {
            return Matches((StringPointer)str);
        }

        /// <summary>
        /// 与えられた文字列が先頭からこの正規表現にマッチするか判定。後続のことは考慮しない。
        /// </summary>
        /// <param name="str"></param>
        /// <returns>マッチ優先度高い順のマッチ。マッチしないときは空のIEnumerable</returns>
        public abstract IEnumerable<RegexMatch> SimpleMatchings(StringPointer str);


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
    }
}
