using System;

namespace CsvParser
{
    public enum TokenType
    {
        Value,
        Newline,
        EndOfStream
    }

    public class Token
    {
        public Token(TokenType tokenType) : this(tokenType, null)
        {
        }

        public Token(TokenType tokenType, string val)
        {
            TokenType = tokenType;
            Value = val;
        }

        public TokenType TokenType { get; }
        public string Value { get; }

        public override bool Equals(object rhs)
        {
            var castRhs = rhs as Token;
            if(castRhs != null)
            {
                return TokenType == castRhs.TokenType &&
                    (Value == null && castRhs.Value == null) ||
                    Value == castRhs.Value;

            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TokenType, Value);
        }

        public override string ToString()
        {
            return $"{Enum.GetName(TokenType.GetType(), TokenType)}: {Value}";
        }
    }
}
