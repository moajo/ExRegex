using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;
using ExRegex.Regexies;
using ExRegex.Regexies.Aliases;
using ExRegex.Regexies.Syntax;
using Char = ExRegex.Regexies.Char;

namespace ExRegex.Parse
{
    public static class RegexParser
    {
        //<文字列から探すRegex,[その文字列に対応するRegex]を生成するやつ>
        private static readonly List<Tuple<Regex, Generator, string>> ParseStage = new List<Tuple<Regex, Generator, string>>();
        public static bool debug = true;

        private static readonly Regex _topLebelKeeper = new Or(new OrInvert('('), new Escaped('('), new UnEscapedBraces());
        private static readonly Regex _orSeparator = new Head().To(new Capture(new OneOrMore(_topLebelKeeper)).Literal("|").To(new Capture(new OneOrMore(new Any()))));

        private static readonly Regex independentPatern = new Or(new Literal(@"\"), new UnEscapedBraces(), new OrInvert(new MetaChar()), ".", "^", "$");
        private static readonly Regex patern = new ZeroOrOne(new LookBehindSyntax()).To(independentPatern).To(new ZeroOrOne(new Or(new LookAheadSyntax(), new Repeater().To(new ZeroOrOne("?")))));   
        private static readonly Regex patterns = new OneOrMore(patern);
        public static readonly Regex RegexPattern = new Named("RGP", new Capture("SEPARATED", patterns).To(new ZeroOrOne(new Literal("|").To(new Reference("RGP")))));

        /// <summary>
        /// マッチしたところに対応する正規表現を生成するやつ
        /// </summary>
        /// <param name="match">マッチしたとこ</param>
        /// <param name="context">その時点でのコンテキスト</param>
        /// <returns></returns>
        public delegate Regex Generator(RegexMatch match, ParseContext context);
        static RegexParser()
        {
            //OrSeparator
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(_orSeparator, (match, context) =>
            {
                var regexes = match.GetCaptures().Select((capture) => _parse(capture.MatchStr, context.Next())).ToArray();
                return new Or(regexes);
            },"resolve '|' separation"));

            //NLB
            var patern3 = new NegativeLookBehindSyntax(true).To(new Capture(independentPatern.To(new ZeroOrOne(new Or(new LookAheadSyntax(), new Repeater().To(new ZeroOrOne("?")))))));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(patern3, (match, context) =>
            {
                var captures = match.GetCaptures().ToArray();
                var condition = _parse(captures[0].MatchStr, context.Next());
                var content = _parse(captures[1].MatchStr, context.Next());
                return new NegativeLookbehind(content, condition);
            }, "(?<!)"));
            //PLB
            var patern4 = new PositiveLookBehindSyntax(true).To(new Capture(independentPatern.To(new ZeroOrOne(new Or(new LookAheadSyntax(), new Repeater().To(new ZeroOrOne("?")))))));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(patern4, (match, context) =>
            {
                var captures = match.GetCaptures().ToArray();
                var condition = _parse(captures[0].MatchStr, context.Next());
                var content = _parse(captures[1].MatchStr, context.Next());
                return new PositiveLookbehind(content, condition);
            }, "(?<=)"));

            //NLA
            var patern5 = new Capture(independentPatern).To(new NegativeLookAheadSyntax(true));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(patern5, (match, context) =>
            {
                var captures = match.GetCaptures().ToArray();
                var condition = _parse(captures[1].MatchStr, context.Next());
                var content = _parse(captures[0].MatchStr, context.Next());
                return new NegativeLookahead(content, condition);
            }, "(?!)"));
            //PLA
            patern5 = new Capture(independentPatern).To(new PositiveLookAheadSyntax(true));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(patern5, (match, context) =>
            {
                var captures = match.GetCaptures().ToArray();
                var condition = _parse(captures[1].MatchStr, context.Next());
                var content = _parse(captures[0].MatchStr, context.Next());
                return new PositiveLookahead(content, condition);
            }, "(?=)"));
            //REPEATER
            patern5 = new Capture(independentPatern).To(new Capture(new Repeater()).To(new ZeroOrOne(new Capture("?"))));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(patern5, (match, context) =>
            {
                var captures = match.GetCaptures().ToArray();
                if (captures.Length == 3) //最小一致
                {
                    var repeaterStr = captures[1].MatchStr;
                    var content = _parse(captures[0].MatchStr, context.Next());
                    if (repeaterStr == "+")
                    {
                        return new NonGreedy(new OneOrMore(content));
                    }
                    if (repeaterStr == "*")
                    {
                        return new NonGreedy(new ZeroOrMore(content));
                    }
                    if (repeaterStr == "?")
                    {
                        return new NonGreedy(new ZeroOrOne(content));
                    }
                    var countRepeaterMatch = new CountRepeaterSyntax(true).PerfectMatch(repeaterStr);
                    if (countRepeaterMatch != null)
                    {
                        var args = countRepeaterMatch.GetCaptures().ToArray();
                        var minCount = Int32.Parse(args[0].MatchStr);
                        if (args.Length == 3)
                        {
                            return new NonGreedy(new CountRepeater(content, minCount, Int32.Parse(args[2].MatchStr)));
                        }
                        if (args.Length == 2)
                        {
                            return new NonGreedy(new CountRepeater(content, minCount, -1));
                        }
                        return new NonGreedy(new CountRepeater(content, minCount, minCount));
                    }
                }
                else
                {
                    var repeaterStr = captures[1].MatchStr;
                    var content = _parse(captures[0].MatchStr, context.Next());
                    if (repeaterStr == "+")
                    {
                        return new OneOrMore(content);
                    }
                    if (repeaterStr == "*")
                    {
                        return new ZeroOrMore(content);
                    }
                    if (repeaterStr == "?")
                    {
                        return new ZeroOrOne(content);
                    }
                    var countRepeaterMatch = new CountRepeaterSyntax(true).PerfectMatch(repeaterStr);
                    if (countRepeaterMatch != null)
                    {
                        var args = countRepeaterMatch.GetCaptures().ToArray();
                        var minCount = Int32.Parse(args[0].MatchStr);
                        if (args.Length == 3)
                        {
                            return new CountRepeater(content,minCount,Int32.Parse(args[2].MatchStr));
                        }
                        if (args.Length == 2)
                        {
                            return new CountRepeater(content, minCount, -1);
                        }
                        return new CountRepeater(content,minCount,minCount);
                    }
                }
                throw new Exception("aaaaaaa");
            }, "+*?"));

            //括弧の解決
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new UnEscapedBraces(), (match, c) =>
            {
                var content = match.GetCaptures().First().MatchStr;
                if (new Head().To(new Literal("?:")).IsHeadMatch(content))//Non-Capture Brace
                {
                    return _parse(content.Substring(2), c.Next());
                }
                if (new Head().To(new Literal("???")).IsHeadMatch(content))//名前参照
                {
                    return new Reference(content.Substring(3));
                }
                var reg = new Head().To(new Literal("??@").To(new Capture(new OneOrMore(new OrInvert('@'))))).To(new Literal("@")).To(new Capture(new OneOrMore(new Any())));
                var m = reg.HeadMatch(content);
                if (m!=null)//名前定義
                {
                    var caps = m.GetCaptures().ToArray();
                    var name = caps[0].MatchStr;
                    var regexStr = caps[1].MatchStr;

                    var contentResult = _parse(regexStr, c.Next());
                    return new Named(name, contentResult);
                }

                //else captureGroup
                var res = _parse(content, c.Next());
                return new Capture(res);
            }, "(),(???),(??@@)"));

            //[]の解決
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new UnEscapedOrBrace(), (match, c) =>
            {
                var first = match.GetCaptures().FirstOrDefault();
                if (first != null && first.MatchStr.StartsWith("^") )
                {
                    var elems = match.GetCaptures().Skip(1).Select(m => _parse(m.MatchStr, c.Next()));
                    if (!elems.All(elem => elem is CharRegex))
                    {
                        throw new Exception("不明なエスケープ");
                    }
                    var chars = elems.Select(elem => (CharRegex)elem).ToArray();
                    return new OrInvert(chars);
                }
                else
                {
                    var elems = match.GetCaptures().Select(m => _parse(m.MatchStr, c.Next())).ToArray();
                    return new Or(elems);
                }
            }, "[]"));

            //エスケープの解決
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\\"), (match, c) => new Literal(@"\"), "ESCAPE_LETERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\)"), (match, c) => new Char(')'), ")_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\("), (match, c) => new Char('('), ")_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\+"), (match, c) => new Char('+'), "+_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\*"), (match, c) => new Char('*'), "*_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\."), (match, c) => new Char('.'), "._LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\?"), (match, c) => new Char('?'), "?_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\$"), (match, c) => new Char('$'), "$_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\^"), (match, c) => new Char('^'), "^_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\d"), (match, c) => new Digit(), "DIGIT_ALIAS"));

            //特殊文字の解決
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"."), (match, c) =>new Any(), "."));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"^"), (match, c) =>new Head(), "^"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"$"), (match, c) =>new Tail(), "$"));
        }

        public static Regex Parse(string regexString)
        {
            if (!RegexPattern.IsPerfectMatch(regexString))
            {
                Console.WriteLine("this is not regex");
                return new Empty();
            }
            return _parse(regexString, new ParseContext());
        }
        private static Regex _parse(string regexString, ParseContext context)
        {
            // check "" -> EMPTY
            var indent = String.Join("", Enumerable.Range(0, context.Depth).Select(i => "  "));
            if (regexString == "")
            {
                if(debug) Console.WriteLine(String.Format("{0}EMPTY", indent));
                return new Empty();
            }
            if (debug) Console.WriteLine(String.Format("{0}{1}", indent, regexString));
            indent = indent + ">";

            //check stageSkiping
            int skipStage = context.SkipStageCount;
            if (skipStage > 0)
            {
                if (debug) Console.WriteLine(String.Format("{0}skip stage:{1}", indent, skipStage));
            }

            for (int i = skipStage; i < ParseStage.Count; i++)
            {
                var match = ParseStage[i].Item1.MatchesRegular((StringPointer)regexString).FirstOrDefault();
                if (match != null)
                {
                    if (debug) Console.WriteLine(String.Format("{0}parse stage({1}):{2} FIND.", indent, i, ParseStage[i].Item3));
                    if (debug) Console.WriteLine(String.Format("{0} {1}", indent, match.ShowMatchText));
                    var preReg = _parse(match.PreStr, new ParseContext(context.Next()) { SkipStageCount = i + 1 });
                    var afterReg = _parse(match.AfterStr, context.Next());
                    var mm =  ParseStage[i].Item2(match, context.Next());
                    return preReg.To(mm).To(afterReg);
                }
            }

            //remain Literal
            if (debug) Console.WriteLine(String.Format("{0}all stage passed. remaining: {1}", indent, regexString));
            if (regexString.Length == 1)
            {
                return new Char(regexString[0]);
            }
            return new Literal(regexString);
        }
        public class ParseContext
        {
            public int Depth = 0;
            public int SkipStageCount = 0;

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
    }
}
