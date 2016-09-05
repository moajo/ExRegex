using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;
using ExRegex.Regexies.Aliases;

namespace ExRegex.Regexies
{
    /// <summary>
    /// エスケープされない時のみマッチ
    /// </summary>
    public class UnEscaped:Alias
    {
        private Regex _target;

        public UnEscaped(Regex target) : base(()=> new PositiveLookbehind(target, new Or(new Head(), new OrInvert('\\')).To(new ZeroOrMore(new Literal(@"\\")))))
        {
            _target = target;
        }

        public override string Name
        {
            get { return "Unescaped"; }
        }
    }
}
