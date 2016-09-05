using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex.Regexies.Aliases
{
    /// <summary>
    /// 使い回すときはこれ使ってください
    /// </summary>
    public class Alias:Regex
    {
        private readonly Func<Regex> _generator;

        public Alias(Func<Regex> generator)
        {
            _generator = generator;
        }
        public override string Name
        {
            get { return "Alias"; }
        }

        public override Regex Clone()
        {
            return new Alias(_generator);
        }


        public sealed override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            foreach (var match in _generator().HeadMatches(str, context))
            {
                yield return new CompositeMatch(this, str, match);
            }
        }
        public override string ToString()
        {
            return "(ALIAS)" + base.ToString();
        }

        protected override string StructureString()
        {
            return _generator().ToStructureString();
        }
    }
}
