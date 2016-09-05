using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExRegex.Regexies.Aliases
{
    public class UnEscapedBraces:Alias
    {
        public override string Name
        {
            get { return "UnedcapedBraces"; }
        }

        public UnEscapedBraces() : base(()=> Make().To(new Named("UnedcapedBraces", new UnEscaped('(').To(new Capture(new ZeroOrMore(new Or(new OrInvert('(', ')'), new Escaped('('), new Escaped(')'), new Reference("UnedcapedBraces"))))).To(new UnEscaped(')')))))
        {
        }
    }
}
