using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRegex.Match;

namespace ExRegex.Regexies
{
    public class CountRepeater:Regex
    {

        private readonly int _minCount;
        private readonly int _maxCount;
        private readonly Regex _target;

        /// <summary>
        /// 無制限はmaxを負に
        /// </summary>
        /// <param name="target"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public CountRepeater(Regex target,int min, int max)
        {
            _target = target;
            _minCount = min;
            _maxCount = max;
        }

        public CountRepeater(Regex target, int just):this(target,just,just)
        {
            
        }
        public override string Name
        {
            get { return "CountRepeater"; }
        }

        protected override Regex SingleClone()
        {
            return new CountRepeater(_target,_minCount,_maxCount);
        }

        public override IEnumerable<RegexMatch> SimpleMatchings(StringPointer str, MatingContext context)
        {
            return Sm(str, context, _minCount, _maxCount);
        }

        private IEnumerable<RegexMatch> Sm(StringPointer str, MatingContext context,int minCount,int maxCount)
        {
            if (maxCount == 0)
            {
                yield return new PositionMatch(this, str);
                yield break;
            }
            foreach (var selfMatch in _target.HeadMatches(str, context))
            {
                var next = str.SubString(selfMatch.Length);
                foreach (var targetMatch in Sm(next, context,minCount-1,maxCount-1))
                {
                    var composite = targetMatch as CompositeMatch;
                    var list = new List<RegexMatch>();
                    list.Add(selfMatch);
                    if (composite != null)
                    {
                        list.AddRange(composite.Matches);
                    }
                    else
                    {
                        break;
                    }
                    yield return new CompositeMatch(this, str, list.ToArray());
                }
                if (minCount <= 1)
                {
                    yield return new CompositeMatch(this, str, selfMatch);
                }
            }
            if (minCount <= 0)
            {
                yield return new PositionMatch(this, str);
            }
        }
    }
}
