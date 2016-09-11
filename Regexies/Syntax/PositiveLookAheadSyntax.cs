using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Regexies.Aliases;

namespace ExRegex.Regexies.Syntax
{
    public class PositiveLookAheadSyntax:Alias
    {
        private readonly bool _captureContent;

        private static readonly Regex Content =
            new ZeroOrMore(new Or(new OrInvert('(', ')'), new Escaped('('), new Escaped(')'),
                new UnEscapedBraces()));
        private static readonly Regex Gen =
         new UnEscaped('(').Literal("?=").To(
             new Capture(
                 new ZeroOrMore(new Or(new OrInvert('(', ')'), new Escaped('('), new Escaped(')'),
                     new UnEscapedBraces())))).To(new UnEscaped(')'));
        public PositiveLookAheadSyntax(bool captureContent) : base(new UnEscaped('(').Literal("?=").To(captureContent?new Capture(Content) : Content).To(new UnEscaped(')')))
        {
            _captureContent = captureContent;
        }

        public override string Name
        {
            get { return "PositiveLookAheadSyntax"; }
        }

        protected override Regex SingleClone()
        {
            return new PositiveLookAheadSyntax(_captureContent);
        }
    }
}
