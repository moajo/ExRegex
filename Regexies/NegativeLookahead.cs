﻿using System;
using System.Collections.Generic;
using System.Linq;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// 否定先読み
    /// 条件パターンが直後に続かなければ、対象パターンにマッチ
    /// </summary>
    public class NegativeLookahead : Regex
    {
        private readonly Regex _target;
        private readonly Regex _condition;

        public NegativeLookahead(Regex target, Regex condition)
        {
            _target = target;
            _condition = new Not(condition);
        }
        public override string Name
        {
            get { return "(?!)"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            foreach (var targetMatch in _target.SimpleMatchings(str, context))
            {
                var nextPos = str.SubString(targetMatch.Length);
                var conditionMatch = _condition.SimpleMatchings(nextPos, context).FirstOrDefault();
                if (conditionMatch != null)
                {
                    yield return new CompositeMatch(this, str, targetMatch);
                }
            }
        }

        public override string ToString()
        {
            return String.Format("{0}[condiition:{1}]\n[target:{2}]", base.ToString(), _condition.ToStructureString(), _target.ToStructureString());
        }
    }
}
