using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// エスケープされない時のみマッチ
    /// </summary>
    public class UnEscaped:AliasRegex
    {
        private Regex _target;

        public UnEscaped(Regex target)
        {
            _target = target;
        }
        public override string Name
        {
            get { return "Unescaped"; }
        }

        public override Regex Content
        {
            get { return new PositiveLookbehind(_target, new Or(new Head(), new OrInvert('\\')).To(new ZeroOrMore(new Literal(@"\\")))); }
        }
    }
}
