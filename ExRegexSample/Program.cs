﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex;
using ExRegex.Match;
using ExRegex.Parse;
using ExRegex.Regexies;
using ExRegex.Regexies.Aliases;
using ExRegex.Regexies.Syntax;
using Char = ExRegex.Regexies.Char;

namespace ExRegexSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //---------------------------------単体先頭マッチテスト--------------------------------------------
            const string text = "aaabbbbTTTXYZAAA123456789";
            var list = new List<Tuple<string, Regex>>();
            //list.Add(Tuple.Create("Literal: match", Regex.Make().Literal("aaa")));
            //list.Add(Tuple.Create("Literal: match", Regex.Make().Literal("aab")));
            //list.Add(Tuple.Create("Any: match", Regex.Make().To(new Any())));
            //list.Add(Tuple.Create("Any: match many times", Regex.Make().To(new Any()).To(new Any()).To(new Any()).To(new Any()).To(new Any())));
            //list.Add(Tuple.Create("Not: unmatch", Regex.Make().To(new Not("a"))));
            //list.Add(Tuple.Create("Not: unmatch", Regex.Make().To(new Not("aaa"))));
            //list.Add(Tuple.Create("Not: match", Regex.Make().To(new Not("aab"))));
            //list.Add(Tuple.Create("Or: match on second arg", Regex.Make().To(new Or("xxxxxx", "aaa", "eeeee"))));
            //list.Add(Tuple.Create("Head: match", Regex.Make().To(new Head())));
            //list.Add(Tuple.Create("Head: unmatch", Regex.Make().Literal("aaa").To(new Head())));
            //list.Add(Tuple.Create("Tail: unmatch", Regex.Make().To(new Tail())));
            //list.Add(Tuple.Create("Tail: match", Regex.Make().Literal(text).To(new Tail())));
            //list.Add(Tuple.Create("?: match one", Regex.Make().To(new ZeroOrOne("a"))));
            //list.Add(Tuple.Create("?: match zero", Regex.Make().To(new ZeroOrOne("x"))));
            //list.Add(Tuple.Create("*: match zero", Regex.Make().To(new ZeroOrMore("x"))));
            //list.Add(Tuple.Create("*: match three", Regex.Make().To(new ZeroOrMore("a"))));
            //list.Add(Tuple.Create("+: unmatch", Regex.Make().To(new OneOrMore("x"))));
            //list.Add(Tuple.Create("+: match three", Regex.Make().To(new OneOrMore("a"))));
            //list.Add(Tuple.Create("+: match six", Regex.Make().To(new OneOrMore(new OneOrMore("a")))));//GREAT!
            //list.Add(Tuple.Create("(?=): unmatch", Regex.Make().To(new PositiveLookahead("aaa", "a"))));
            //list.Add(Tuple.Create("(?=): match", Regex.Make().To(new PositiveLookahead("aaa", "b"))));
            //list.Add(Tuple.Create("(?!): unmatch", Regex.Make().To(new NegativeLookahead("aaa", "a"))));
            //list.Add(Tuple.Create("(?!): match", Regex.Make().To(new NegativeLookahead("aaa", "b"))));
            //list.Add(Tuple.Create("(?!): match", Regex.Make().To(new PositiveLookbehind("bbb", "a"))));
            //list.Add(Tuple.Create("(?!): unmatch", Regex.Make().To(new PositiveLookbehind("bbb", "x"))));
            //list.Add(Tuple.Create("(?!): 1 match", Regex.Make().To(new NegativeLookbehind("bbb", "a"))));
            //list.Add(Tuple.Create("(?!): 2 match", Regex.Make().To(new NegativeLookbehind("bbb", "x"))));

            for (int i = 0; i < list.Count; i++)
            {
                ShowLog(list[i].Item1,text,list[i].Item2);
            }

            //---------------------------------結合先頭マッチテスト--------------------------------------------
            var list2 = new List<Tuple<string, Regex, string>>();
            var rgx1 =
                Regex.Make()
                    .Literal("r")
                    .To(new ZeroOrOne("e"))
                    .To(new Any())
                    .To(new OrInvert('g'))
                    .To(new Or("e", "x"))
                    .To(new Or("e"));
            //list2.Add(Tuple.Create("re?.[^g][ex][e]: match", rgx1, "rgrxe"));
            //list2.Add(Tuple.Create("re?.[^g][ex][e]: match", rgx1, "rexee"));
            //list2.Add(Tuple.Create("re?.[^g][ex][e]: match", rgx1, "rekvee"));
            //list2.Add(Tuple.Create("re?.[^g][ex][e]: unmatch", rgx1, "rekverrr"));



            foreach (var tuple in list2)
            {
                ShowLog(tuple.Item1,tuple.Item3,tuple.Item2);
            }

            //---------------------------------全体マッチテスト--------------------------------------------
            var list3 = new List<Tuple<string, Regex>>();
            var regg = new Capture(new OneOrMore(new Or(new OrInvert('|'),new Escaped(new Char('|')))));//|に挟まれる奴
            var a = new ZeroOrMore(new Or(regg,new UnEscaped('|')));//[]の中身
            var aa = new UnEscaped('[').To(a).To(new UnEscaped(']'));
            var aasaa = new OneOrMore(regg.To(new Literal("a"))).To(regg);
            var b0 = new OrInvert('\\','[',']');//\じゃないやつ
            var b1 = new Literal(@"\").To(new Any());
            var b2 = new OneOrMore(new Capture(new Or(b0, b1)));
            var b3 = new UnEscaped('[');
            var b4 = new UnEscaped(']');
            var b = b3.To(b2);
            //list3.Add(Tuple.Create(@"as\[asd[a\d\]d\ds]", new UnEscapedOrBrace() as Regex));
            //list3.Add(Tuple.Create("ffabtabeaabbab", Regex.Make().To(new OneOrMore(new Literal("a").To(new Literal("b"))))));
            //list3.Add(Tuple.Create("aatestatest", Regex.Make().Literal("test")));
            //list3.Add(Tuple.Create("aatestatesttasttust", Regex.Make().Literal("t").To(new Capture(new Any())).Literal("st")));
            //list3.Add(Tuple.Create("aatestteaatestesaates", Regex.Make().To(new Named("Label",new Literal("a"))).To(new Reference("Label"))));
            var rg = Regex.Make().Literal("(").To(new ZeroOrMore(new Any())).Literal(")");//単純括弧ok
            var rg2 = Regex.Make().To(new ZeroOrMore(new OrInvert('(',')')));//括弧じゃない奴らの連続ちょっとちがうけどok
            var rg3 = Regex.Make().To(new Or("()",new OneOrMore(new OrInvert('(', ')'))));//ok
            var rgx4 = Regex.Make().To(new Named("kakko", new UnEscaped('(').To(new ZeroOrMore(new Or(new OrInvert('(', ')'), new Reference("kakko")))).Literal(")")));//括弧とれた！！！！
            //var rg5 = new UnEscaped(new Literal("("));//エスケープされない括弧開き
            var escapedB=new PositiveLookbehind(new Literal("("), new Or(new Head(), new OrInvert('\\')).To(new Literal(@"\")).To(new ZeroOrMore(new Literal(@"\\"))));
            var escapedB2=new PositiveLookbehind(new Literal(")"), new Or(new Head(), new OrInvert('\\')).To(new Literal(@"\")).To(new ZeroOrMore(new Literal(@"\\"))));

            var independentPatern = new Or(new Literal(@"\"), new UnEscapedBraces(), new OrInvert(new MetaChar()), ".", "^", "$");
            var patern =new ZeroOrOne(new LookBehindSyntax()).To(independentPatern).To(new ZeroOrOne(new Or(new LookAheadSyntax(), new Repeater().To(new ZeroOrOne("?")))));//後置、前置ともに最大ひとつしか取れない仕様で
            var patterns = new OneOrMore(patern);
            var regexPattern = new Named("RGP", new Capture(patterns).To(new ZeroOrOne(new Literal("|").To(new Reference("RGP")))));

            //var rgx44 = Regex.Make().To(new Named("kakko", new UnEscaped(new Literal("(")).To(new ZeroOrMore(new Or(new OrInvert('(', ')'),escapedB,escapedB2, new Reference("kakko")))).To(new UnEscaped(new Literal(")")))));//括弧とれた！！！！
            //list3.Add(Tuple.Create("aaa(ddd)fff", rg));//ok
            //list3.Add(Tuple.Create("a.a?a*a+(ddd)f+f*f", rgrg2 as Regex));//ok
            //list3.Add(Tuple.Create("aa(?<=a)aa", rgrg2 as Regex));//ok
            //list3.Add(Tuple.Create("aa(?<!a)aa", rgrg2 as Regex));//ok
            //list3.Add(Tuple.Create("aa(?=a)aa", rgrg2 as Regex));//ok
            //list3.Add(Tuple.Create("aa(?!a)aa", rgrg2 as Regex));//ok
            //list3.Add(Tuple.Create("aa(?<=a)a(?=a)a", rgrg2 as Regex));//ok
            //list3.Add(Tuple.Create("aa(?!a)aa(?=a)a", rgrg2 as Regex));//ok
            //list3.Add(Tuple.Create("aa(?!a)aa*a", rgrg2 as Regex));//ok
            //list3.Add(Tuple.Create("aa(?!a)a|a*a|a", regexPattern as Regex));//ok
            //list3.Add(Tuple.Create("aa(?!a)a++a", rgrg2 as Regex));//ok
            //list3.Add(Tuple.Create("{1,3a}", new CountRepeaterSyntax() as Regex));//ok
            //list3.Add(Tuple.Create("{1, }", new CountRepeaterSyntax() as Regex));//ok
            //list3.Add(Tuple.Create("{14}", new CountRepeaterSyntax() as Regex));//ok
            //list3.Add(Tuple.Create("aaa(ddd)f(f)f", rg));//ok
            //list3.Add(Tuple.Create("aaa(d(d)d)f(f)f", rg));//ok
            //list3.Add(Tuple.Create("aatestatest", Regex.Make().To(new OneOrMore(new Literal("a")))));
            //list3.Add(Tuple.Create("xy", Regex.Make().To(new ZeroOrMore("a"))));
            //list3.Add(Tuple.Create("aaasd(dsff)fsdf()(sdf)sd((dfg(df)A(A)S()F(A",rg2));
            //list3.Add(Tuple.Create("fsdf()(sdf))A(A)S()F(A",rg3));
            //list3.Add(Tuple.Create("()",rgx4));
            //list3.Add(Tuple.Create("(a)",rgx4));
            //list3.Add(Tuple.Create("(aa)",rgx4));
            //list3.Add(Tuple.Create("(a(a))",rgx4));
            //list3.Add(Tuple.Create("bb(a(a))", rgx4));
            //list3.Add(Tuple.Create("nn(a(a)nn", rgx4));
            //list3.Add(Tuple.Create("a", new CountRepeater("a",0,1) as Regex));
            //list3.Add(Tuple.Create("a,aa,aaa,aaaa,aaaaa", new CountRepeater("a",1,1) as Regex));
            //list3.Add(Tuple.Create("a,aa,aaa,aaaa,aaaaa", new CountRepeater("a",2,2) as Regex));
            //list3.Add(Tuple.Create("a,aa,aaa,aaaa,aaaaa", new CountRepeater("a",1,2) as Regex));
            //list3.Add(Tuple.Create("a,aa,aaa,aaaa,aaaaa", new CountRepeater("a",2,3) as Regex));
            //list3.Add(Tuple.Create("nn(ann", RegexParser.RegexPattern));
            //list3.Add(Tuple.Create("123", Regex.Make().To(new Digit())));
            //list3.Add(Tuple.Create("123", Regex.Make().To(new OneOrMore(new Digit()))));
            //list3.Add(Tuple.Create(@"\\(()a\()aa(\\\(a)", new UnEscapedBraces() as  Regex));
            //list3.Add(Tuple.Create(@"\\a\))\da(a\\s\(", new NegativeLookBehindSyntax(true).To(new Capture(independentPatern.To(new ZeroOrOne(new Or(new LookAheadSyntax(), new Repeater().To(new ZeroOrOne("?"))))))) as Regex));

            foreach (var tuple in list3)
            {

                ShowLog("3rdTest::", tuple.Item1, tuple.Item2);
                Console.ReadLine();

            }
            //Console.ReadLine();

            var strList = new List<string>();
            //strList.Add(@"aaaaaaaaaaa");//単純リテラル
            //strList.Add(@"\\aaa\\ss\\sss");//エスケープリテラル
            //strList.Add(@"\\a\daa\\s\d\ds\\\dsss");//エスケープ
            //strList.Add(@"\\a\))\da(a\\s\(");//エスケープ
            //strList.Add(@"\\d");//エスケープ
            //strList.Add(@"\\(a)\d");//エスケープ
            //strList.Add(@"\\a(\))a)(as\(");//エスケープ
            //strList.Add(@"\\a(\))\da)(a\\s\(");//エスケープ
            //strList.Add(@"\\ddf(gh(df)gh)(df)gh");//エスケープ
            //strList.Add(@"aaaaa(bbbb)ccc");//エスケープ
            //strList.Add(@"aaaaa(?:bbbb)ccc");//エスケープ
            //strList.Add(@"aaaaa(?=bbbb)ccc");//エスケープ
            //strList.Add(@"aaaaa(?!bbbb)ccc");//エスケープ
            //strList.Add(@"aaaaa(?<=bbbb)ccc");//エスケープ
            //strList.Add(@"aaaa(?=a(??bbbb)c)cc");//エスケープ
            //strList.Add(@"aaaa(?=a(??bbbb)c)cc");//エスケープ

            //strList.Add(@"a+bc");//エスケープ
            //strList.Add(@"a?bc");//エスケープ
            //strList.Add(@"a*bc");//エスケープ
            //strList.Add(@"a.bc");//エスケープ
            //strList.Add(@"a^bc");//エスケープ
            //strList.Add(@"a\$\[$b]c");//エスケープ
            //strList.Add(@"a[abcde]b");//エスケープ
            //strList.Add(@"a[^abcde]b");//エスケープ
            //strList.Add(@"a(");//エスケープ

            int count =0;
            foreach (var regStr in strList)
            {
                Console.WriteLine("------------"+(count++)+"----------------");
                Console.WriteLine("\n@@@ParseProccess@@@");
                var reg = RegexParser.Parse(regStr);

                Console.WriteLine("\n@@@Structure@@@");
                Console.WriteLine(reg.ToStructureString());
                Console.ReadLine();
            }

            var parseList = new List<Tuple<string, string>>();

            //parseList.Add(Tuple.Create(@"\d\d\d-\d\d\d\d", "00000000"));
            //parseList.Add(Tuple.Create(@"\d\d\d-\d\d\d\d", "000-0000"));
            //parseList.Add(Tuple.Create(@"b.k", "bak"));
            //parseList.Add(Tuple.Create(@"b.k", "btk"));
            //parseList.Add(Tuple.Create(@"b.k", "btrk"));
            //parseList.Add(Tuple.Create(@"b.+k", "btrk"));
            //parseList.Add(Tuple.Create(@"b.+k", "bk"));
            //parseList.Add(Tuple.Create(@"b.+k", "btssrkss"));
            //parseList.Add(Tuple.Create(@"b.*k", "btrk"));
            //parseList.Add(Tuple.Create(@"b.*k", "bk"));
            //parseList.Add(Tuple.Create(@"[13579]", "4"));
            //parseList.Add(Tuple.Create(@"[13579]", "4123445678"));

            foreach (var tuple in parseList)
            {
                var reg = RegexParser.Parse(tuple.Item1);
                ShowLog(tuple.Item1+" ===> "+tuple.Item2,tuple.Item2,reg);
                Console.ReadLine();
            }

            while (true)
            {
                Console.Write("imput [Rr]egex:  \t>");
                string reg = Console.ReadLine();
                Console.Write("input [Tt]ext:   \t>");
                string te = Console.ReadLine();
                var regex = RegexParser.Parse(reg);
                ShowLog(reg+" ===> "+te,te,regex);
                Console.WriteLine("\n\n");
            }

        }

        private static void ShowLog(string message, string targetText, Regex regex)
        {
            if (regex is Empty)
            {
                return;
            }
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine(message);
            Console.WriteLine(String.Format("target:{0}", targetText));

            Console.WriteLine("@@@MatchRegular@@@");
            var count = 0;
            foreach (var match in regex.MatchesRegular((StringPointer)targetText))
            {
                Console.WriteLine();
                Console.WriteLine("@" + count++);
                Console.WriteLine(match.ShowMatchText);

                Console.WriteLine("@@Capture@@");
                foreach (var capture in match.GetCaptures())
                {
                    Console.WriteLine(String.Format("{0}", capture.MatchStr));
                }

                Console.WriteLine("@@Match@@");
                Console.WriteLine(match);
            }

            //Console.WriteLine("@@@MatchALL@@@");
            //count = 0;
            //foreach (var match in regex.Matches(targetText))
            //{
            //    Console.WriteLine();
            //    Console.WriteLine("@" + count++);
            //    Console.WriteLine(match.ShowMatchText);

            //    Console.WriteLine("@@Capture@@");
            //    foreach (var capture in match.GetCaptures())
            //    {
            //        Console.WriteLine(String.Format("{0}", capture.MatchStr));
            //    }

            //    Console.WriteLine("@@Match@@");
            //    Console.WriteLine(match);
            //}




            //こんな感じで書く
            var orRegex =
                new ZeroOrOne(
                    new Capture(
                        new OneOrMore(new Or(new OrInvert('|'),
                            new PositiveLookbehind(new Char('|'),
                                new Or(new Head(), new OrInvert('\\')).To(new Literal(@"\"))
                                    .To(new ZeroOrMore(new Literal(@"\\")))))))).To(new PositiveLookbehind(
                                        new Char('|'),
                                        new Or(new Head(), new OrInvert('\\')).To(new ZeroOrMore(new Literal(@"\\")))))
                    .To(
                        new ZeroOrMore(
                            new Or(
                                new Capture(
                                    new OneOrMore(new Or(new OrInvert('|'),
                                        new PositiveLookbehind(new Char('|'),
                                            new Or(new Head(), new OrInvert('\\')).To(new Literal(@"\"))
                                                .To(new ZeroOrMore(new Literal(@"\\"))))))),
                                new PositiveLookbehind(new Char('|'),
                                    new Or(new Head(), new OrInvert('\\')).To(new ZeroOrMore(new Literal(@"\\")))))));



        }
    }
}
