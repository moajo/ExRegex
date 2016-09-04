namespace ExRegex
{
    /// <summary>
    /// 文字列と現在位置を表す。（イミュータブルってしってる？）
    /// </summary>
    public class StringPointer
    {
        /// <summary>
        /// 生文字列
        /// </summary>
        public readonly string RawStr;

        /// <summary>
        /// 現在位置
        /// </summary>
        public int Pointer { get; }

        public StringPointer(string str)
        {
            RawStr = str;
        }

        private StringPointer(string str, int pointer) : this(str)
        {
            Pointer = pointer;
        }

        /// <summary>
        /// 文字列化する
        /// </summary>
        /// <returns></returns>
        public string Value()
        {
            return RawStr.Substring(Pointer);
        }

        public int Length { get { return RawStr.Length - Pointer; } }

        /// <summary>
        /// ポインタを進める
        /// </summary>
        /// <param name="startIndex"></param>
        public StringPointer SubString(int startIndex)
        {
            return new StringPointer(RawStr,Pointer+startIndex);
        }

        /// <summary>
        /// C#はキャストをオーバーロードできる！！！素晴らしい！！！
        /// </summary>
        /// <param name="str"></param>
        public static explicit operator StringPointer (string str)
        {
            return new StringPointer(str);
        }

        public override string ToString()
        {
            return Value();
        }
    }
}
