﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Regexies.Aliases;

namespace ExRegex.Regexies.Syntax
{
    public class Repeater:Alias
    {
        public Repeater() : base(new Or("+","*","?",new CountRepeaterSyntax(false)))
        {
        }

        public override string Name
        {
            get { return "Repeater"; }
        }

        protected override Regex SingleClone()
        {
            return new Repeater();
        }
    }
}
