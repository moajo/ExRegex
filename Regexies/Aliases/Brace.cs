﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExRegex.Regexies.Aliases
{
    [Obsolete]
    public class Brace:Alias
    {
        public override string Name
        {
            get { return "Brace"; }
        }

        public override Regex Clone()
        {
            return new Brace();
        }

        public Brace() : base(()=>Make().To(new Named("kakko", new Literal("(").To(new Capture(new ZeroOrMore(new Or(new OrInvert('(', ')'), new Reference("kakko"))))).Literal(")"))))
        {
        }
    }
}
