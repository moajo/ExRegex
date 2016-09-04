namespace ExRegex.Match
{
    /// <summary>
    /// new AtomicMatch(regex,"")がめんどくさかった。
    /// </summary>
    public class PositionMatch:AtomicMatch
    {
        public PositionMatch(Regex regex,StringPointer str) : base(regex,str, 0)
        {
        }
    }
}
