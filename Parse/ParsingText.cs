using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex.Parse
{
    public class ParsingText
    {
        private readonly List<ParsingTextNode> _list = new List<ParsingTextNode>();

        public ParsingText(string str)
        {
            _list.Add(new TextNode(str));
        }

        private ParsingText(IEnumerable<ParsingTextNode> list)
        {
            _list = list.ToList();
        }

        ///// <summary>
        ///// 指定した部分をジェネレータで作ったRegexノードで置き換える
        ///// </summary>
        ///// <param name="match"></param>
        ///// <param name="generator"></param>
        //public void Replace(RegexMatch match, Func<ParsingText, Regex> generator)
        //{
        //    var startIndex = GetRealIndex(match.Str.Pointer);
        //    var endIndex = GetRealIndex(startIndex + match.Length);
        //    var startNodeIndex = CreateStartNodeIndex(startIndex);
        //    var endNodeIndex = CreateEndNodeIndex(endIndex);

        
        //    var next = new ParsingText(_list.Take(endNodeIndex).Skip(startNodeIndex));//指定位置
        //    var regex = generator(next);
        //    _list.RemoveRange(startNodeIndex,endNodeIndex-startNodeIndex);
        //    _list.Insert(startNodeIndex,new RegexNode(regex,next.ToString()) );

        //}

        public void ReplaceMatches(IEnumerable<RegexMatch> matches, Func<RegexMatch,ParsingText, Regex> generator)
        {
            var matchArray = matches.ToArray();
            var startIndexies = matchArray.Select(m => GetRealIndex(m.Str.Pointer)).ToArray();
            var endIndexies = startIndexies.Zip(matchArray,Tuple.Create).Select(tuple=>
            GetRealIndex(tuple.Item2.Str.Pointer+tuple.Item2.Length)).ToArray();

            for (int i = 0; i < matchArray.Length; i++)
            {
                var startNodeIndex = CreateStartNodeIndex(startIndexies[i]);
                var endNodeIndex = CreateEndNodeIndex(endIndexies[i]);

                var next = new ParsingText(_list.Take(endNodeIndex).Skip(startNodeIndex));//指定位置
                var regex = generator(matchArray[i],next);
                _list.RemoveRange(startNodeIndex, endNodeIndex - startNodeIndex);
                _list.Insert(startNodeIndex, new RegexNode(regex, next.ToString()));
            }
        }

        public Regex ToRegex()
        {
            var regList = _list.Select(pt =>
            {
                if (pt is TextNode)
                {
                    return new RegexParser.DumRegex(pt.Text);
                }
                return (pt as RegexNode).Regex;
            }).ToList();
            for (int i = 1; i < regList.Count; i++)
            {
                regList[i - 1].To(regList[i]);
            }
            return regList[0];
        }
        
        private int CreateStartNodeIndex(int startIndex)//破壊的！
        {
            var total = 0;
            for (int i = 0; i < _list.Count; i++)
            {
                if (total == startIndex)//このノードの頭から
                {
                    return i;
                }
                if (total + _list[i].RealText.Length > startIndex)//このノードの途中から
                {
                    if (_list[i] is TextNode)
                    {
                        //このノード分割
                        var text = _list[i].RealText;
                        var index = startIndex - total;
                        var pre = text.Substring(0, index);
                        var after = text.Substring(index);

                        //replace
                        _list.RemoveAt(i);
                        _list.Insert(i,new TextNode(after));
                        _list.Insert(i,new TextNode(pre));

                        return i + 1;
                    }
                    else
                    {
                        throw new Exception("これは分割できない");
                    }
                }
                total += _list[i].RealText.Length;
            }
            throw new Exception("みつからない");
        }
        private int CreateEndNodeIndex(int endIndex)//返り値の直前まで(末尾だったらList.Count)
        {
            var total = 0;
            for (int i = 0; i < _list.Count; i++)
            {
                var next = total + _list[i].RealText.Length;
                if (next == endIndex)//このノードの末尾まで
                {
                    return i + 1;
                }
                if (next > endIndex)//このノードの途中まで
                {
                    if (_list[i] is TextNode)
                    {
                        //このノード分割
                        var text = _list[i].RealText;
                        var index = endIndex - total;
                        var pre = text.Substring(0, index);
                        var after = text.Substring(index);

                        //replace
                        _list.RemoveAt(i);
                        _list.Insert(i, new TextNode(after));
                        _list.Insert(i, new TextNode(pre));

                        return i + 1;
                    }
                    else
                    {
                        throw new Exception("これは分割できない");
                    }

                }
                total = next;
            }
            throw new Exception("みつからない");
        }

        private int GetRealIndex(int index)
        {
            var total = 0;
            var diff = 0;
            for (int i = 0; i < _list.Count; i++)
            {
                total += _list[i].Text.Length;
                if (total>=index)
                {
                    return index + diff;
                }
                diff += _list[i].RealText.Length - _list[i].Text.Length;
            }
            throw new Exception("みつからない");
        }

        public override string ToString()
        {
            return String.Join("", _list.Select(node => node.Text));
        }


        abstract class ParsingTextNode
        {
            public abstract string Text { get; }
            public abstract string RealText { get; }
        }

        class TextNode : ParsingTextNode
        {
            public TextNode(string str)
            {
                Text = str;
            }

            public override string Text { get; }

            public override string RealText
            {
                get { return Text; }
            }
        }

        class RegexNode:ParsingTextNode
        {
            public RegexNode(Regex regex,string text)
            {
                Regex = regex;
                RealText = text;
            }
            public readonly Regex Regex;

            public override string Text
            {
                get { return ""; }
            }

            public override string RealText { get; }
        }
    }
}
