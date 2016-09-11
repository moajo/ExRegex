using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ExRegex.Regexies.Aliases
{
    public class UnEscapedOrBrace:Alias
    {
        private const char O = '[';
        private const char C = ']';
        public UnEscapedOrBrace() : base(() => new UnEscaped(O).To(new ZeroOrMore(new Capture(new Or(new Literal(@"\").To(new Any()), new OrInvert(O, C))))).To(new UnEscaped(C)))
        {
        }

        public override string Name
        {
            get { return "UnEscapedOrBrace"; }
        }

        protected override Regex SingleClone()
        {
            return new UnEscapedOrBrace();
        }
    }
}
