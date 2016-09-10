using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExRegex.Regexies.Aliases
{
    public class OrSeparator:Alias
    {
        public OrSeparator() : base(()=> new ZeroOrOne(new Capture(new OneOrMore(new Or(new OrInvert('|'), new Escaped(new Char('|')))))).To(new UnEscaped('|')).To(new ZeroOrMore(new Or(new Capture(new OneOrMore(new Or(new OrInvert('|'), new Escaped(new Char('|'))))), new UnEscaped('|')))))
        {
           
        }

        public override string Name
        {
            get { return "OrSepalator"; }
        }

        public override Regex Clone()
        {
            return new OrSeparator();
        }
    }
}
