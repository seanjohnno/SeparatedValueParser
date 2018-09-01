using System.IO;
using System.Linq;
using System.Text;

namespace CsvParser
{
    public class ParserBuilder<T>
    {
        private StreamReader _streamReader;
        private string[] _titles;
        private char _separator;

        public ParserBuilder<T> WithSeparator(char separator)
        {
            _separator = separator;
            return this;
        }

        public ParserBuilder<T> WithTitles(string[] titles)
        {
            _titles = titles;
            return this;
        }

        public ParserBuilder<T> WithSource(string str) => WithSource(new MemoryStream(Encoding.UTF8.GetBytes(str)));

        public ParserBuilder<T> WithSource(Stream stream) => WithSource(new StreamReader(stream));

        public ParserBuilder<T> WithSource(StreamReader streamReader)
        {
            _streamReader = streamReader;
            return this;
        }

        public Parser<T> Build()
        {
            return new Parser<T>(_separator, new Tokenizer(_streamReader, _separator), _titles?.OfType<string>()?.ToList());
        }
    }
}
