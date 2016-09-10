using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Regexies.Aliases;

namespace ExRegex.Regexies.Syntax
{
    public class CountRepeater:Alias
    {
        private static readonly Regex Num = new OneOrMore(new Digit());
        private static readonly Regex Gen =
            new UnEscaped('{').To(Num)
                .To(new ZeroOrOne(new Char(',').To(new ZeroOrOne(Num))))
                .To(new UnEscaped('}'));

        public CountRepeater() : base(Gen)
        {
        }

        public override string Name
        {
            get { return "CountRepeater"; }
        }

        public override Regex Clone()
        {
            return new CountRepeater();
        }
    }
}
