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
        public ParsingRegex(string regexString)
        {
            
        }
        public override string Name
        {
            get { return "Parsing"; }
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
