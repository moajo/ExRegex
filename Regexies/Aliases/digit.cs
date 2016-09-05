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
    public class Digit:Alias
    {
        public override string Name
        {
            get { return "Digit"; }
        }

        public Digit() : base(()=>new Or("0", "1", "2", "3", "4", "5", "6", "7", "8", "9"))
        {
        }
    }
}
