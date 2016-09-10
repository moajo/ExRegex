using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Regexies.Aliases;

namespace ExRegex.Regexies.Syntax
{
    public class LookBehindSyntax:Alias
    {
        public LookBehindSyntax() : base(()=>new Or(new PositiveLookBehindSyntax(),new NegativeLookBehindSyntax()))
        {
        }

        public override string Name
        {
            get { return "LookBehindSyntax"; }
        }

        public override Regex Clone()
        {
            return new LookBehindSyntax();
        }
    }
}
