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
        private static readonly Regex Content =
            new ZeroOrMore(new Or(new OrInvert('(', ')'), new Escaped('('), new Escaped(')'),
                new UnEscapedBraces()));
        private static readonly Regex Gen =
            new UnEscaped('(').Literal("?<=").To(
                new Capture(
                    new ZeroOrMore(new Or(new OrInvert('(', ')'), new Escaped('('), new Escaped(')'),
                        new UnEscapedBraces())))).To(new UnEscaped(')'));

        private readonly bool _captureContent;

        public PositiveLookBehindSyntax(bool captureContent) : base(new UnEscaped('(').Literal("?<=").To(captureContent?new Capture(Content) :Content ).To(new UnEscaped(')')))
        {
            _captureContent = captureContent;
        }

        public override string Name
        {
            get { return "PositiveLookBehindSyntax"; }
        }

        protected override Regex SingleClone()
        {
            return new PositiveLookBehindSyntax(_captureContent);
        }
    }
}
