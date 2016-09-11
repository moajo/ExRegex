using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Regexies.Aliases;

namespace ExRegex.Regexies.Syntax
{
    public class LookAheadSyntax:Alias
    {
        private bool _captureContent;
        public LookAheadSyntax(bool captureContent=false) : base(()=>new Or(new PositiveLookAheadSyntax(captureContent),new NegativeLookAheadSyntax(captureContent)))
        {
            _captureContent = captureContent;
        }

        public override string Name
        {
            get { return "LookAheadSyntax"; }
        }

        protected override Regex SingleClone()
        {
            return new LookAheadSyntax(_captureContent);
        }
    }
}
