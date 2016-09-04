using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// *
    /// 0回以上の反復にマッチ
    /// </summary>
    public class ZeroOrMore : Regex
    {
        private readonly Regex _target;

        public ZeroOrMore(Regex target)
        {
            _target = target;
        }

        public override string Name
        {
            get { return "*"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str)//TODO:fix
        {
            var stack = new Stack<RegexMatch>();//データ構造：スタックとは先入れ後出し(FILO)のリストです
            var pointer = str;
            while (true)
            {
                var match = _target.HeadMatch(pointer);
                if (match == null) break;
                stack.Push(match);
                pointer = pointer.SubString(match.Length);
            }
            if (stack.Count >= 1)
            {
                while (stack.Count != 0)
                {
                    yield return new CompositeMatch(this, str, stack.ToArray());
                    stack.Pop();
                }
            }
            else
            {
                yield return new PositionMatch(this,str);
            }

        }
    }
}
