using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Regexies.Aliases;

namespace ExRegex.Regexies.Syntax
{
    public class LookBehindSyntax:Alias
    {
        private bool _captureContent;
        public LookBehindSyntax(bool captureContent=false) : base(()=>new Or(new PositiveLookBehindSyntax(captureContent),new NegativeLookBehindSyntax(captureContent)))
        {
            _captureContent = captureContent;
        }

        public override string Name
        {
            get { return "LookBehindSyntax"; }
        }

        protected override Regex SingleClone()
        {
            return new LookBehindSyntax(_captureContent);
        }
    }
}
