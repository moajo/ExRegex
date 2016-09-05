using System;
using System.Collections.Generic;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// 一文字にマッチ
    /// </summary>
    public class Char: CharRegex
    {
        private readonly char _content;

        public Char(char c)
        {
            _content = c;
        }
        public override string Name
        {
            get { return "char"; }
        }

        public override Regex Clone()
        {
            return new Char(_content);
        }

        public override string ToString()
        {
            return base.ToString()+String.Format("({0})",_content);
        }

        /// <summary>
        /// C#はキャストをオーバーロードできる！！！最高！！！
        /// </summary>
        /// <param name="c"></param>
        public static implicit operator Char(char c)
        {
            return new Char(c);
        }

        public override bool CheckChar(char c)
        {
            return c == _content;
        }
    }
}
