using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;
using ExRegex.Regexies;
using ExRegex.Regexies.Aliases;
using Char = ExRegex.Regexies.Char;

namespace ExRegex.Parse
{
    public static class RegexParser
    {
        //<文字列から探すRegex,[その文字列に対応するRegex]を生成するやつ>
        private static readonly List<Tuple<Regex, Generator, string>> ParseStage = new List<Tuple<Regex, Generator, string>>();
        private static bool debug = true;

        /// <summary>
        /// マッチしたところに対応する正規表現を生成するやつ
        /// </summary>
        /// <param name="match">マッチしたとこ</param>
        /// <param name="context">その時点でのコンテキスト</param>
        /// <returns></returns>
        public delegate ParseResult Generator(RegexMatch match, ParseContext context,ParseResult pre,ParseResult after);

        public delegate Regex Generator2(RegexMatch match, ParseContext context);

        static RegexParser()
        {
            //括弧の解決
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new UnEscapedBraces(), (match, c,a,b) =>
            {
                var content = match.GetCaptures().First().MatchStr;
                if (new Head().To(new Literal("?:")).IsHeadMatch(content))//Non-Capture Brace
                {
                    var mm =  _parse(content.Substring(2), c.Next()).AssertNoRequest();
                    return b.ConnectAhead(mm).ConnectAhead(a);
                }
                if (new Head().To(new Literal("?=")).IsHeadMatch(content))//肯定先読み
                {
                    var contentResult = _parse(content.Substring(2), c.Next()).AssertNoRequest();
                    var mm= new ParseResult(null, (ahead => new PositiveLookahead(ahead, contentResult.Result)),null) ;
                    return b.ConnectAhead(mm).ConnectAhead(a);
                }
                if (new Head().To(new Literal("?!")).IsHeadMatch(content))//否定先読み
                {
                    var contentResult = _parse(content.Substring(2), c.Next()).AssertNoRequest();
                    var mm = new ParseResult(null, (ahead => new NegativeLookahead(ahead, contentResult.Result)),null);
                    return b.ConnectAhead(mm).ConnectAhead(a);
                }
                if (new Head().To(new Literal("?<=")).IsHeadMatch(content))//肯定後読み
                {
                    var contentResult = _parse(content.Substring(3), c.Next()).AssertNoRequest();
                    var mm = new ParseResult(null,null, (behind => new PositiveLookbehind(behind, contentResult.Result))) ;
                    return b.ConnectAhead(mm).ConnectAhead(a);
                }
                if (new Head().To(new Literal("?<!")).IsHeadMatch(content))//否定後読み
                {
                    var contentResult = _parse(content.Substring(3), c.Next()).AssertNoRequest();
                    var mm = new ParseResult(null,null, (behind => new NegativeLookbehind(behind, contentResult.Result)));
                    return b.ConnectAhead(mm).ConnectAhead(a);
                }
                if (new Head().To(new Literal("???")).IsHeadMatch(content))//名前参照
                {
                    var mm = new ParseResult(new Reference(content.Substring(3)));
                    return b.ConnectAhead(mm).ConnectAhead(a);
                }
                var reg = new Head().To(new Literal("??@").To(new Capture(new OneOrMore(new OrInvert('@'))))).To(new Literal("@")).To(new Capture(new OneOrMore(new Any())));
                var m = reg.HeadMatch(content,new MatingContext());
                if (m!=null)//名前定義
                {
                    var caps = m.GetCaptures().ToArray();
                    var name = caps[0].MatchStr;
                    var regexStr = caps[1].MatchStr;

                    var contentResult = _parse(regexStr, c.Next()).AssertNoRequest();
                    var mm =  new ParseResult(new Named(name, contentResult.Result));
                    return b.ConnectAhead(mm).ConnectAhead(a);
                }

                //else captureGroup
                var res = _parse(content, c.Next()).AssertNoRequest();
                var mm2 = new ParseResult(new Capture(res.Result));
                return b.ConnectAhead(mm2).ConnectAhead(a);
            }, "RECURSIVE"));

            //[]の解決
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new UnEscapedOrBrace(), (match, c, a, b) =>
            {
                var first = match.GetCaptures().FirstOrDefault();
                if (first != null && first.MatchStr == "^")
                {
                    var elems = match.GetCaptures().Skip(1).Select(m => _parse(m.MatchStr, c.Next()).Result);
                    if (!elems.All(elem => elem is CharRegex))
                    {
                        throw new Exception("不明なエスケープ");
                    }
                    var chars = elems.Select(elem => (CharRegex) elem).ToArray();
                    var mm = new ParseResult(new OrInvert(chars));
                    return b.ConnectAhead(mm).ConnectAhead(a);
                }
                else
                {
                    var elems = match.GetCaptures().Select(m => _parse(m.MatchStr, c.Next()).Result).ToArray();
                    var mm = new ParseResult(new Or(elems));
                    return b.ConnectAhead(mm).ConnectAhead(a);
                }
            }, "OR"));

            //|の解決
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new OrSeparator(), (match, c, a, b) =>
            {
                var caps = match.GetCaptures().Select(m=>m.MatchStr);
                var first = match.MatchStr[0] == '|';
                var last = match.MatchStr[match.MatchStr.Length - 1] == '|';
                if (first && last)
                {
                    Console.WriteLine("両方");
                    var elemList = caps.Where(s => s != "").Select(m => _parse(m, c.Next()).Result).ToList();
                    if (a.Result is Empty || b.Result is Empty)
                    {
                        throw new Exception("未対応ですごめんね");
                    }
                    elemList.Insert(0, a.Result);
                    elemList.Add(b.Result);
                    return new ParseResult(new Or(elemList.ToArray()),a.AheadRequest,b.BehindRequest);
                }
                if (first)
                {
                    Console.WriteLine("左のみ");
                    var elemList = caps.Where(s=>s!="").Select(m => _parse(m, c.Next()).Result).ToList();
                    if (!(a.Result is Empty))
                    {
                        elemList.Insert(0, a.Result);
                        return b.ConnectAhead(new ParseResult(new Or(elemList.ToArray()), a.AheadRequest, null));
                    }
                    if (a.AheadRequest != null)
                    {
                        throw new Exception("不正構文");
                    }
                    return b.ConnectAhead(new ParseResult(null, ahe =>
                    {
                        elemList.Insert(0,ahe);
                        return new Or(elemList.ToArray());
                    },null));
                }
                if (last)
                {
                    Console.WriteLine("右のみ");
                    var elemList = caps.Where(s => s != "").Select(m => _parse(m, c.Next()).Result).ToList();
                    if (!(b.Result is Empty))
                    {
                        elemList.Add(b.Result);
                        return new ParseResult(new Or(elemList.ToArray()), null, b.BehindRequest).ConnectAhead(a);
                    }
                    if (b.BehindRequest != null)
                    {
                        throw new Exception("不正構文");
                    }

                    return new ParseResult(null,null,beh =>
                    {
                        elemList.Add(beh);
                        return new Or(elemList.ToArray());
                    }).ConnectAhead(a);
                }
                Console.WriteLine("どっちでもない");
                var elems = caps.Select(m => _parse(m, c.Next()).Result).ToArray();
                return b.ConnectAhead(new ParseResult(new Or(elems))).ConnectAhead(a);

            }, "OR"));


            //エスケープの解決
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(EscapeLiteral, (match, c, a, b) => b.ConnectAhead(new ParseResult(new Literal(@"\"))).ConnectAhead(a), "ESCAPE_LETERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\d"), (match, c, a, b) => b.ConnectAhead(new ParseResult(new Digit())).ConnectAhead(a), "DIGIT_ALIAS"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\)"), (match, c, a, b) => b.ConnectAhead(new ParseResult(new Char(')'))).ConnectAhead(a), ")_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\("), (match, c, a, b) => b.ConnectAhead(new ParseResult(new Char('('))).ConnectAhead(a), ")_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\+"), (match, c, a, b) => b.ConnectAhead(new ParseResult(new Char('+'))).ConnectAhead(a), "+_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\*"), (match, c, a, b) => b.ConnectAhead(new ParseResult(new Char('*'))).ConnectAhead(a), "*_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\."), (match, c, a, b) => b.ConnectAhead(new ParseResult(new Char('.'))).ConnectAhead(a), "._LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\?"), (match, c, a, b) => b.ConnectAhead(new ParseResult(new Char('?'))).ConnectAhead(a), "?_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\$"), (match, c, a, b) => b.ConnectAhead(new ParseResult(new Char('$'))).ConnectAhead(a), "$_LITERAL"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"\^"), (match, c, a, b) => b.ConnectAhead(new ParseResult(new Char('^'))).ConnectAhead(a), "^_LITERAL"));

            //特殊文字の解決
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"+"), (match, c, a, b) => b.ConnectAhead(new ParseResult(null, (ahead => new OneOrMore(ahead)),null)).ConnectAhead(a), "+"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"*"), (match, c, a, b) => b.ConnectAhead(new ParseResult(null, (ahead => new ZeroOrMore(ahead)),null)).ConnectAhead(a), "*"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"?"), (match, c, a, b) => b.ConnectAhead(new ParseResult(null, (ahead => new ZeroOrOne(ahead)),null)).ConnectAhead(a), "?"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"."), (match, c, a, b) => b.ConnectAhead(new ParseResult(new Any())).ConnectAhead(a), "."));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"^"), (match, c, a, b) => b.ConnectAhead(new ParseResult(new Head())).ConnectAhead(a), "^"));
            ParseStage.Add(Tuple.Create<Regex, Generator, string>(new Literal(@"$"), (match, c, a, b) => b.ConnectAhead(new ParseResult(new Tail())).ConnectAhead(a), "$"));
        }

        public static readonly Regex EscapeLiteral = Regex.Make().Literal(@"\\");
        public static readonly Regex Any = Regex.Make().Literal(@"*");

        public static Regex Parse(string regexString)
        {
            var res = _parse(regexString, new ParseContext()).AssertNoRequest();
            return res.Result;
        }
        private static ParseResult _parse(string regexString, ParseContext context)
        {
            /*//TODO:
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
            
            //括弧系の解決(キャプチャ,グルーピング,名前つき、参照、先読み、Or)
            //エスケープリテラルの解決

            //エスケープの解決

            //^$.の解決
            //+*?の解決
            //



            */
            var indent = String.Join("", Enumerable.Range(0, context.Depth).Select(i => "  "));
            if (regexString == "")
            {
                if(debug) Console.WriteLine(String.Format("{0}EMPTY", indent));
                return new ParseResult(new Empty());
            }
            if (debug) Console.WriteLine(String.Format("{0}{1}", indent, regexString));
            indent = indent + ">";

            int skipStage = context.SkipStageCount;
            if (skipStage > 0)
            {
                if (debug) Console.WriteLine(String.Format("{0}skip stage:{1}", indent, skipStage));
            }

            for (int i = 0; i < ParseStage.Count; i++)
            {
                if (skipStage-- > 0)
                {
                    continue;
                }

                var match = ParseStage[i].Item1.MatchesRegular((StringPointer)regexString).FirstOrDefault();
                if (match != null)
                {
                    if (debug) Console.WriteLine(String.Format("{0}parse stage({1}):{2} FIND.", indent, i, ParseStage[i].Item3));
                    if (debug) Console.WriteLine(String.Format("{0} {1}", indent, match.ShowMatchText));
                    var preReg = _parse(match.PreStr, new ParseContext(context.Next()) { SkipStageCount = i + 1 });//括弧がないのでリクエストはありえない
                    var afterReg = _parse(match.AfterStr, context.Next());
                    return ParseStage[i].Item2(match, context,preReg,afterReg);
                }
                if (debug) Console.WriteLine(String.Format("{0}parse stage:{1} is not match.", indent, i));
            }
            if (debug) Console.WriteLine(String.Format("{0}all stage passed. remaining: {1}", indent, regexString));
            if (regexString.Length == 1)
            {
                return new ParseResult(new Char(regexString[0]));
            }
            return new ParseResult(new Literal(regexString));
        }


        public class ParseResult
        {
            public Regex Result { get; }
            public Func<Regex, Regex> AheadRequest { get; }//先読み
            public Func<Regex, Regex> BehindRequest { get; }

            public ParseResult(Regex result):this(result,null,null)
            {
            }
            public ParseResult(Regex result, Func<Regex, Regex> aheadRequest, Func<Regex, Regex> behindRequest)
            {
                Result = result ?? new Empty();
                AheadRequest = aheadRequest;
                BehindRequest = behindRequest;
            }

            /// <summary>
            /// つなげる
            /// </summary>
            /// <param name="ahead"></param>
            /// <returns></returns>
            public ParseResult ConnectAhead(ParseResult ahead)
            {
                var res = ahead.Result;
                var aheadReq = ahead.AheadRequest;
                var behindReq = BehindRequest;
                //aheadが先頭側。
                if (AheadRequest != null)
                {
                    //res.TAILを置換
                    if (!(res is Empty))
                    {
                        var newTail = AheadRequest(res.TailRegex);
                        if (!res.ReplaceTail(newTail))
                        {
                            res = newTail;
                        }
                    }
                    else
                    {
                        if (aheadReq != null)
                        {
                            throw new Exception("不正な構文");
                        }
                        else
                        {
                            aheadReq = AheadRequest;
                        }
                    }
                    

                }
                Regex newHead = Result;
                if (ahead.BehindRequest != null)
                {

                    //Result.HEADを置換
                    if (!(newHead is Empty))
                    {
                        newHead = ahead.BehindRequest(Result);
                        Result.ReplaceHead(newHead);
                    }
                    else
                    {
                        if (behindReq != null)
                        {
                            throw new Exception("不正な構文");
                        }
                        else
                        {
                            behindReq = ahead.BehindRequest;
                        }
                        
                    }
                    

                }
                res = res.To(newHead);



                return new ParseResult(res, aheadReq, behindReq);
            }

            public ParseResult AssertNoRequest()
            {
                if (AheadRequest != null | BehindRequest != null)
                {
                    throw new Exception("NoRequest assertion fail");

                }
                return this;
            }
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

            public override Regex Clone()
            {
                return new DumRegex(Text);
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
