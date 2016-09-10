using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Regexies.Aliases;

namespace ExRegex.Regexies.Syntax
{
    /// <summary>
    /// (?<=)
    /// </summary>
    public class PositiveLookBehindSyntax:Alias
    {
        private static readonly Regex Gen =
            new UnEscaped('(').Literal("?<=").To(
                new Capture(
                    new ZeroOrMore(new Or(new OrInvert('(', ')'), new Escaped('('), new Escaped(')'),
                        new UnEscapedBraces())))).To(new UnEscaped(')'));
        public PositiveLookBehindSyntax() : base(()=>Gen)
        {
        }

        public override string Name
        {
            get { return "PositiveLookBehindSyntax"; }
        }

        public override Regex Clone()
        {
            return new PositiveLookBehindSyntax();
        }
    }
}
