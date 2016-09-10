using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Regexies.Aliases;

namespace ExRegex.Regexies.Syntax
{
    public class LookAheadSyntax:Alias
    {
        public LookAheadSyntax() : base(()=>new Or(new PositiveLookAheadSyntax(),new NegativeLookAheadSyntax()))
        {
        }

        public override string Name
        {
            get { return "LookAheadSyntax"; }
        }

        public override Regex Clone()
        {
            return new LookAheadSyntax();
        }
    }
}
