using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Regexies.Aliases;

namespace ExRegex.Regexies.Syntax
{
    /// <summary>
    /// (?<!)
    /// </summary>
    public class NegativeLookBehindSyntax : Alias
    {
        private static readonly Regex Content =
            new ZeroOrMore(new Or(new OrInvert('(', ')'), new Escaped('('), new Escaped(')'),
                new UnEscapedBraces()));

        private readonly bool _captureContent;

        public NegativeLookBehindSyntax(bool captureContent) : base(new UnEscaped('(').Literal("?<!").To(captureContent?
            new Capture(Content) : Content).To(new UnEscaped(')')))
        {
            this._captureContent = captureContent;
        }

        public override string Name
        {
            get { return "NegativeLookBehindSyntax"; }
        }

        protected override Regex SingleClone()
        {
            return new NegativeLookBehindSyntax(_captureContent);
        }
    }
}
