using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    /// <summary>
    /// 定義された名前つきRegexに再帰する
    /// </summary>
    public class Reference:Regex
    {
        private readonly string _targetLabel;

        public Reference(string targetLabel)
        {
            _targetLabel = targetLabel;
        }
        public override string Name
        {
            get { return "Reference"; }
        }

        public override Regex Clone()
        {
            return new Reference(_targetLabel);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            foreach (var match in context.NamedRegexes[_targetLabel].SimpleMatchings(str, context))
            {
                yield return new CompositeMatch(this,str,match);
            }
        }

        public override string ToString()
        {
            return String.Format("{0}({1})",Name,_targetLabel);
        }
    }
}
