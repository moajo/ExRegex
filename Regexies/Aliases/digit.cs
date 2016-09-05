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
    public class Digit:CharRegex
    {
        public override string Name
        {
            get { return "Digit"; }
        }

        public override Regex Clone()
        {
            return new Digit();
        }

        public override bool CheckChar(char c)
        {
            var list = new List<String>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            return list.Exists(str => str[0] == c);
        }
    }
}
