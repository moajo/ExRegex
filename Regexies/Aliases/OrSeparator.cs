using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExRegex.Regexies.Aliases
{
    public class OrSeparator:Alias
    {
        public OrSeparator() : base(()=> new ZeroOrMore(new Or(new Capture(new OneOrMore(new Or(new OrInvert('|'), new Escaped(new Char('|'))))), new UnEscaped('|'))))
        {
        }
    }
}
