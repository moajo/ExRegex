using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExRegex.Regexies.Aliases
{
    public class UnEscapedBraces:AliasRegex
    {
        public override string Name
        {
            get { return "UnedcapedBraces"; }
        }

        public override Regex Content
        {
            get { return Make().To(new Named("UnedcapedBraces", new UnEscaped(new Literal("(")).To(new ZeroOrMore(new Or(new OrInvert('(', ')'), new PositiveLookbehind(new Literal("("), new Or(new Head(), new OrInvert('\\')).To(new Literal(@"\")).To(new ZeroOrMore(new Literal(@"\\")))), new PositiveLookbehind(new Literal(")"), new Or(new Head(), new OrInvert('\\')).To(new Literal(@"\")).To(new ZeroOrMore(new Literal(@"\\")))), new Reference("UnedcapedBraces")))).To(new UnEscaped(new Literal(")"))))); }
        }
    }
}
