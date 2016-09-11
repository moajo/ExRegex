using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Regexies.Aliases;

namespace ExRegex.Regexies.Syntax
{
    public class CountRepeaterSyntax:Alias
    {
        private static readonly Regex Num = new OneOrMore(new Digit());
        private static readonly Regex Gen =
            new UnEscaped('{').To(Num)
                .To(new ZeroOrOne(new Char(',').To(new ZeroOrOne(Num))))
                .To(new UnEscaped('}'));
        private static readonly Regex Capture =
            new UnEscaped('{').To(new Capture(Num))
                .To(new ZeroOrOne(new Capture(new Char(',')).To(new ZeroOrOne(new Capture(Num)))))
                .To(new UnEscaped('}'));

        private bool _captureContent;

        public CountRepeaterSyntax(bool captureContent) : base(captureContent?Capture:Gen)
        {
            _captureContent = captureContent;
        }

        public override string Name
        {
            get { return "CountRepeaterSyntax"; }
        }

        protected override Regex SingleClone()
        {
            return new CountRepeaterSyntax(_captureContent);
        }
    }
}
