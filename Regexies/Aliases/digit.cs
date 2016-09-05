using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex.Regexies.Aliases
{
    /// <summary>
    /// \d
    /// </summary>
    public class Digit:AliasRegex
    {
        public override string Name
        {
            get { return "Digit"; }
        }

        public override Regex Content
        {
            get { return new Or("0","1", "2", "3", "4", "5", "6", "7", "8", "9"); }
        }
    }
}
