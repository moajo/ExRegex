using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex;
using ExRegex.Match;
using ExRegex.Parse;
using ExRegex.Regexies;
using ExRegex.Regexies.Aliases;

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
            //list3.Add(Tuple.Create("aatestatest", Regex.Make().Literal("test")));
            //list3.Add(Tuple.Create("aatestatesttasttust", Regex.Make().Literal("t").To(new Capture(new Any())).Literal("st")));
            //list3.Add(Tuple.Create("aatestteaatestesaates", Regex.Make().To(new Named("Label",new Literal("a"))).To(new Reference("Label"))));
            var rg = Regex.Make().Literal("(").To(new ZeroOrMore(new Any())).Literal(")");//単純括弧ok
            var rg2 = Regex.Make().To(new ZeroOrMore(new OrInvert('(',')')));//括弧じゃない奴らの連続ちょっとちがうけどok
            var rg3 = Regex.Make().To(new Or("()",new OneOrMore(new OrInvert('(', ')'))));//ok
            var rgx4 = Regex.Make().To(new Named("kakko",new Literal("(").To(new ZeroOrMore(new Or(new OrInvert('(', ')'), new Reference("kakko")))).Literal(")")));//括弧とれた！！！！
            //list3.Add(Tuple.Create("aaa(ddd)fff", rg));//ok
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
            //list3.Add(Tuple.Create("bb(a(a))",rgx4));
            //list3.Add(Tuple.Create("nn(a(a)nn", rgx4));
            //list3.Add(Tuple.Create("123", Regex.Make().To(new Digit())));
            //list3.Add(Tuple.Create("123", Regex.Make().To(new OneOrMore(new Digit()))));

            foreach (var tuple in list3)
            {

                ShowLog("3rdTest::", tuple.Item1, tuple.Item2);
                //Console.WriteLine("-----------------------------------------------------------------------------------");
                //Console.WriteLine(String.Format("target:{0}", tuple.Item1));
                //foreach (var match in tuple.Item2.Matches((StringPointer)tuple.Item1))
                //{
                //    ShowLog("3rdTest::", tuple.Item1, tuple.Item2);
                //}

            }


            var strList = new List<string>();
            //strList.Add(@"aaaaaaaaaaa");//単純リテラル
            //strList.Add(@"\\aaa\\ss\\sss");//エスケープリテラル
            strList.Add(@"\\a\daa\\s\d\ds\\\dsss");//エスケープ
            strList.Add(@"\\a\))\da(a\\s\(");//エスケープ
            strList.Add(@"\\d");//エスケープ

            int count=0;
            foreach (var regStr in strList)
            {
                Console.WriteLine("------------"+(count++)+"----------------");
                Console.WriteLine("\n@@@ParseProccess@@@");
                var reg = RegexParser.Parse(regStr);

                Console.WriteLine("\n@@@Structure@@@");
                Console.WriteLine(reg.ToStructureString());
            }


            Console.Read();

        }

        private static void ShowLog(string message, string targetText, Regex regex)
        {
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
            }

            Console.WriteLine("@@@MatchALL@@@");
            count = 0;
            foreach (var match in regex.Matches((StringPointer)targetText))
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
        }
    }
}
