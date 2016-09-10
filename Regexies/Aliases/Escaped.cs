using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExRegex.Regexies.Aliases
{
    /// <summary>
    /// エスケープされてる
    /// 行頭または\以外の文字に、\が奇数個ついてる
    /// </summary>
    public class Escaped:Alias
    {
        private readonly Char _target;
        public Escaped(Char target) : base(()=> new PositiveLookbehind(target, new Or(new Head(), new OrInvert('\\')).To(new Literal(@"\")).To(new ZeroOrMore(new Literal(@"\\")))))
        {
            _target = target;
        }

        public override string Name
        {
            get { return "Escaped"; }
        }

        public override Regex Clone()
        {
            return new Escaped(_target);
        }
    }
}
