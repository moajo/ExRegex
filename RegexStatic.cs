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

        public static Regex Or(this Regex self, params Regex[] contents)
        {
            return self.To(new Or(contents));
        }
        public static Regex Head(this Regex self)
        {
            return self.To(new Head());
        }
        public static Regex Tail(this Regex self)
        {
            return self.To(new Tail());
        }
        public static Regex Any(this Regex self)
        {
            return self.To(new Any());
        }
        public static Regex OneOrMore(this Regex self,Regex target)
        {
            return self.To(new OneOrMore(target));
        }
        //public static Regex Any(this Regex self)
        //{
        //    return self.To(new Any());
        //}
        //public static Regex Any(this Regex self)
        //{
        //    return self.To(new Any());
        //}
        //public static Regex Any(this Regex self)
        //{
        //    return self.To(new Any());
        //}
        //public static Regex Any(this Regex self)
        //{
        //    return self.To(new Any());
        //}
    }
}
