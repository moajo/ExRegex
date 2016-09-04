using ExRegex.Regexies;

namespace ExRegex
{
    /// <summary>
    /// メソッドチェーンで書きたかっただけ
    /// </summary>
    public static class RegexStatic
    {
        public static Regex Literal(this Regex self, string literal)
        {
            return self.To(new Literal(literal));
        }
    }
}
