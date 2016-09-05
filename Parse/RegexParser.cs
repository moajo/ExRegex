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
        private static readonly List<Tuple<Regex, Func<RegexMatch,ParsingText, Regex>,string>> ParseStage = new List<Tuple<Regex, Func<RegexMatch, ParsingText, Regex>, string>>();

        static RegexParser()
        {
            ParseStage.Add(Tuple.Create<Regex, Func<RegexMatch, ParsingText, Regex>, string>(EscapeLiteral,(match,pt)=> new Literal(@"\"),"ESCAPE_LETERAL"));//エスケープリテラルの解決
            ParseStage.Add(Tuple.Create<Regex, Func<RegexMatch, ParsingText, Regex>, string>(new Literal(@"\d"), (match, pt) => new Digit(),"DIGIT_ALIAS"));
            ParseStage.Add(Tuple.Create<Regex, Func<RegexMatch, ParsingText, Regex>, string>(new Literal(@"\("), (match, pt) => new Literal("("), "(_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Func<RegexMatch, ParsingText, Regex>, string>(new Literal(@"\)"), (match, pt) => new Literal(")"), "(_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Func<RegexMatch, ParsingText, Regex>, string>(new Literal(@"\)"), (match, pt) => new Literal(")"), "(_LITERAL"));
        }

        public static readonly Regex EscapeLiteral = Regex.Make().Literal(@"\\");
        public static readonly Regex Any = Regex.Make().Literal(@"*");

        public static Regex Parse(string regexString)
        {
            return _parse(new ParsingText(regexString), new ParseContext());
        }
        private static Regex _parse(ParsingText regexText,ParseContext context)
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
            var regexString = regexText.ToString();
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
                var l = new List<RegexMatch>();
                foreach (var match in ParseStage[i].Item1.MatchesRegular((StringPointer)regexString))
                {
                    l.Add(match);
                    Console.WriteLine(String.Format("{0}parse stage:{1} FIND.", indent, i));
                    Console.WriteLine(String.Format("{0} {1}", indent, match.ShowMatchText));
                    //regexText.Replace(match, pt => ParseStage[i].Item2(match, pt));
                }
                if (l.Count==0)
                {
                    Console.WriteLine(String.Format("{0}parse stage:{1} is not match.", indent, i));
                }
                regexText.ReplaceMatches(l, ParseStage[i].Item2);
                regexString = regexText.ToString();//マッチ箇所が削除されたように見える
                //var match =  ParseStage[i].Item1.MatchesRegular((StringPointer)regexString).FirstOrDefault();
                //if (match != null)
                //{
                //    Console.WriteLine(String.Format("{0}parse stage:{1} FIND.", indent, i));
                //    regexText.Replace(match,pt=> ParseStage[i].Item2(match,pt));
                //    //var preReg = _parse(match.PreStr, new ParseContext(context.Next()) { SkipStageCount = i+1 });
                //    //var afterReg = _parse(match.AfterStr, context.Next());
                //    //return preReg.To(ParseStage[i].Item2(match)).To(afterReg);
                //}
                //Console.WriteLine(String.Format("{0}parse stage:{1} is not match.", indent, i));
            }
            Console.WriteLine(String.Format("{0}all stage passed. remaining: {1}", indent,regexString));

            ////まず、エスケープリテラルの解決

            ////エスケープの解決

            //括弧系の解決(キャプチャ,グルーピング,名前つき、参照、先読み、Or)
            //^$.の解決
            //+*?の解決
            //


            //throw new NotImplementedException();
            return regexText.ToRegex();
        }

        private class ParseContext
        {
            public int Depth=0;
            public int SkipStageCount=0;

            public ParseContext() { }

            public ParseContext(ParseContext from)
            {
                SkipStageCount = from.SkipStageCount;
                Depth = from.Depth;
            }

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
