namespace SeparatedValuesParser.Tokenizer
{
    public class StringStream
    {
        private readonly string _str;
        private int _position;

        public StringStream(string str)
        {
            _str = str;
        }

        public int Next() => !IsEnded ? _str[_position++] : -1;

        public bool IsEnded { get => _position == (_str?.Length ?? 0); }
    }
}
