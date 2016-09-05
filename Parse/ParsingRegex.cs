using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex.Parse
{
    /// <summary>
    /// 文字列からパースされるRegex
    /// </summary>
    public class ParsingRegex:Regex
    {
        private string _regexString;
        public ParsingRegex(string regexString)
        {
            _regexString = regexString;
        }
        public override string Name
        {
            get { return "Parsing"; }
        }

        public override Regex Clone()
        {
            return new ParsingRegex(_regexString);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
