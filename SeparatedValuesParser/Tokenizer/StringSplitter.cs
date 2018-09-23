using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SeparatedValuesParser.Tokenizer
{
    public class StringSplitter : IEnumerable<string>
    {
        private const int EndOfString = -1;

        public StringSplitter(StringStream stream, char separator)
        {
            Stream = stream;
            Separator = separator;
            CharacterGrabber = new CharacterParser();
        }

        public StringStream Stream { get; }

        public char Separator { get; }

        private ICharacterParser CharacterGrabber { get; set; }

        public IEnumerator<string> GetEnumerator() => GetInternalEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetInternalEnumerator();

        private IEnumerator<string> GetInternalEnumerator()
        {
            while(!Stream.IsEnded)
            {
                yield return ReadNext();
            }      
        }

        private string ReadNext()
        {
            var builder = new StringBuilder();
            int c;
            while ((c = CharacterGrabber.Next(this)) != EndOfString)
            {
                builder.Append((char)c);
            }
            return builder.ToString();
        }

        interface ICharacterParser
        {
            int Next(StringSplitter splitter);
        }

        class CharacterParser : ICharacterParser
        {
            public int Next(StringSplitter splitter)
            {
                var nextChar = splitter.Stream.Next();
                if(nextChar == '"')
                {
                    var ignoreQuotes = new IgnoreQuotesParser();
                    splitter.CharacterGrabber = ignoreQuotes;
                    return ignoreQuotes.Next(splitter);
                }
                else if(nextChar == splitter.Separator)
                {
                    return -1;
                }
                return nextChar;
            }
        }

        class IgnoreQuotesParser : ICharacterParser
        {
            public int Next(StringSplitter splitter)
            {
                var nextChar = splitter.Stream.Next();
                if (nextChar == '"')
                {
                    var normal = new CharacterParser();
                    splitter.CharacterGrabber = normal;
                    return normal.Next(splitter);
                }
                return nextChar;
            }
        }
    }
}
