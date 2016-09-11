using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExRegex.Regexies.Aliases
{
    public class MetaChar:CharRegex
    {
        public override string Name
        {
            get { return "MetaChar"; }
        }

        protected override Regex SingleClone()
        {
            return new MetaChar();
        }

        public override bool CheckChar(char c)
        {
            var str = @"^$.+*\?()[]{}|";
            return str.ToCharArray().Contains(c);
        }
    }
}
