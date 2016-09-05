using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;
using ExRegex.Regexies;
using ExRegex.Regexies.Aliases;

namespace ExRegex.Parse
{
    public static class RegexParser
    {
        //<文字列から探すRegex,[その文字列に対応するRegex]を生成するやつ>
        private static readonly List<Tuple<Regex, Generator, string>> ParseStage = new List<Tuple<Regex, Generator, string>>();

        /// <summary>
        /// マッチしたところに対応する正規表現を生成するやつ
        /// </summary>
        /// <param name="match">マッチしたとこ</param>
        /// <param name="context">その時点でのコンテキスト</param>
        /// <returns></returns>
        public delegate Regex Generator(RegexMatch match,ParseContext context);

        static RegexParser()
        {
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new UnEscapedBraces(), (match,c)=> _parse(match.GetCaptures().First().MatchStr, c.Next()), "RECURSIVE"));//エスケープリテラルの解決
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(EscapeLiteral,(match,c)=> new Literal(@"\"),"ESCAPE_LETERAL"));//エスケープリテラルの解決
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\d"), (match, c) => new Digit(),"DIGIT_ALIAS"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\("), (match, c) => new Literal("("), "(_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\)"), (match, c) => new Literal(")"), ")_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Brace(), (match,c) =>  _parse(match.GetCaptures().First().MatchStr,c.Next()) , "BRACE_LITERAL"));
        }

        public static readonly Regex EscapeLiteral = Regex.Make().Literal(@"\\");
        public static readonly Regex Any = Regex.Make().Literal(@"*");

        public static Regex Parse(string regexString)
        {
            return _parse(regexString, new ParseContext());
        }
        private static Regex _parse(string regexString,ParseContext context)
        {
            /*
            todo:実装
            +
            *
            ?
            ()
            []
            [^]
            (?:)
            (?=)
            (?!)
            (??)@再帰

            パースじゅんばん
            エスケープリテラル


            */
            var indent = String.Join("", Enumerable.Range(0, context.Depth).Select(i => "  "));
            if (regexString == "")
            {
                Console.WriteLine(String.Format("{0}EMPTY", indent));
                return new Empty();
            }
            Console.WriteLine(String.Format("{0}{1}",indent,regexString));
            indent = indent + ">";

            int skipStage = context.SkipStageCount;

            for (int i = 0; i < ParseStage.Count; i++)
            {
                if (skipStage-- > 0)
                {
                    Console.WriteLine(String.Format("{0}skip stage:{1}", indent, i));
                    continue;
                }

                var match = ParseStage[i].Item1.MatchesRegular((StringPointer) regexString).FirstOrDefault();
                if (match != null)
                {
                    Console.WriteLine(String.Format("{0}parse stage:{1} FIND.", indent, i));
                    Console.WriteLine(String.Format("{0} {1}", indent, match.ShowMatchText));
                    var preReg = _parse(match.PreStr, new ParseContext(context.Next()) { SkipStageCount = i + 1 });
                    var afterReg = _parse(match.AfterStr, context.Next());
                    var mm = ParseStage[i].Item2(match, context);
                    return preReg.To(mm).To(afterReg);
                }
                    Console.WriteLine(String.Format("{0}parse stage:{1} is not match.", indent, i));
            }
            Console.WriteLine(String.Format("{0}all stage passed. remaining: {1}", indent,regexString));

            ////まず、エスケープリテラルの解決

            ////エスケープの解決

            //括弧系の解決(キャプチャ,グルーピング,名前つき、参照、先読み、Or)
            //^$.の解決
            //+*?の解決
            //


            //throw new NotImplementedException();
            return new DumRegex(regexString);
        }

        public class ParseContext
        {
            public int Depth=0;
            public int SkipStageCount=0;

            public ParseContext() { }

            public ParseContext(ParseContext from)
            {
                SkipStageCount = from.SkipStageCount;
                Depth = from.Depth;
            }

            /// <summary>
            /// 深度だけ深くしたコピー
            /// </summary>
            /// <returns></returns>
            public ParseContext Next()
            {
                var next = new ParseContext(this);
                next.Depth++;
                return next;
            }
        }

        public class DumRegex : Regex
        {
            public DumRegex(string text)
            {
                this.Text = text;
            }

            public string Text;

            public override string Name
            {
                get { return "DUM"; }
            }

            public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return String.Format("{0}({1})", Name, Text);
            }
        }
    }
}
