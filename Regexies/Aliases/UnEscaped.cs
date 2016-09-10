namespace ExRegex.Regexies.Aliases
{
    /// <summary>
    /// エスケープされない時のみマッチ
    /// 行頭または\以外の文字に、\が偶数個ついてる
    /// </summary>
    public class UnEscaped:Alias
    {
        private readonly char _target;

        public UnEscaped(char target) : base(()=> new PositiveLookbehind(new Char(target), new Or(new Head(), new OrInvert('\\')).To(new ZeroOrMore(new Literal(@"\\")))))
        {
            _target = target;
        }

        public override string Name
        {
            get { return "Unescaped"; }
        }

        public override Regex Clone()
        {
            return new UnEscaped(_target);
        }
    }
}
