﻿using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// 空のRegex(メソッドチェーンの頭、ツリールート用)
    /// </summary>
    public class Empty:Regex
    {
        public override string Name
        {
            get { return "Empty"; }
        }

        protected override Regex SingleClone()
        {
            return new Empty();
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            yield return new PositionMatch(this,str);
        }

        /// <summary>
        /// こいつは役目を終えて消える
        /// </summary>
        /// <param name="nextRegex"></param>
        /// <returns></returns>
        public override Regex To(Regex nextRegex)
        {
            return nextRegex;
        }
    }
}
