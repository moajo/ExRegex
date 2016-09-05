using System;
using System.Collections.Generic;
using ExRegex.Regexies;

namespace ExRegex.Match
{
    /// <summary>
    /// マッチを表す
    /// </summary>
    public abstract class RegexMatch
    {
        /// <summary>
        /// マッチしたRegex
        /// </summary>
        public Regex Regex;

        /// <summary>
        /// 対象文字列
        /// </summary>
        public StringPointer Str;

        /// <summary>
        /// マッチ長さ
        /// </summary>
        public abstract int Length { get; }

        /// <summary>
        /// マッチ部分の文字列
        /// </summary>
        public  abstract string MatchStr { get; }

        public abstract IEnumerable<RegexMatch> GetCaptures(); 

        protected RegexMatch(Regex regex,StringPointer str)
        {
            Regex = regex;
            Str = str;
        }

        /// <summary>
        /// マッチの直前までの文字列
        /// </summary>
        public string PreStr { get { return Str.RawStr.Substring(0, Str.Pointer); } }
        /// <summary>
        /// マッチ直後以降の文字列
        /// </summary>
        public string AfterStr { get { return Str.SubString(Length).ToString(); } }

        public string ShowMatchText { get { return String.Format("{0} >>{1}<< {2}", PreStr, MatchStr, AfterStr); } }

    }
}
