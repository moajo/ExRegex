using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExRegex.Regexies.Aliases
{
    public class UnEscapedBraces:Alias
    {
        private static Regex content =
            new Capture(
                new ZeroOrMore(new Or(new OrInvert('(', ')'), new Escaped('('), new Escaped(')'),
                    new Reference("UnedcapedBraces"))));
        public override string Name
        {
            get { return "UnedcapedBraces"; }
        }

        protected override Regex SingleClone()
        {
            return new UnEscapedBraces();
        }

        public UnEscapedBraces(bool captureContent = false) : base(()=> Make().To(new Named("UnedcapedBraces", new UnEscaped('(').To(captureContent? new Capture(content): content).To(new UnEscaped(')')))))
        {
        }
    }
}
