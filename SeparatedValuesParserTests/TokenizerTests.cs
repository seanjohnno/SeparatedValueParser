using CsvParser;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace CsvParserTests
{
    public class TokenizerTests
    {
        [Fact]
        public void GivenASingleWord_WhenAskedForToken_ThenTokenizerReturnsIt()
        {
            Tokenizer tokenizer = new Tokenizer(CreateAStreamReaderFor("Hello"), ',');
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "Hello"));
            tokenizer.Next().ShouldBe(new Token(TokenType.EndOfStream));
        }

        [Fact]
        public void GivenMultipeWords_WhenAskedForTokens_ThenTokenizerReturnsThem()
        {
            Tokenizer tokenizer = new Tokenizer(CreateAStreamReaderFor("Hello,World"), ',');
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "Hello"));
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "World"));
            tokenizer.Next().ShouldBe(new Token(TokenType.EndOfStream));
        }

        [Fact]
        public void GivenMultipeLines_AndWindowsLineEndings_WhenAskedForTokens_ThenTokenizerReturnsThem()
        {
            Tokenizer tokenizer = new Tokenizer(CreateAStreamReaderFor("Hello,World\r\nEllo,Werld"), ',');
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "Hello"));
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "World"));
            tokenizer.Next().ShouldBe(new Token(TokenType.Newline));
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "Ello"));
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "Werld"));
            tokenizer.Next().ShouldBe(new Token(TokenType.EndOfStream));
        }

        [Fact]
        public void GivenMultipeLines_AndLinuxLineEndings_WhenAskedForTokens_ThenTokenizerReturnsThem()
        {
            Tokenizer tokenizer = new Tokenizer(CreateAStreamReaderFor("Hello,World\nEllo,Werld"), ',');
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "Hello"));
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "World"));
            tokenizer.Next().ShouldBe(new Token(TokenType.Newline));
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "Ello"));
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "Werld"));
            tokenizer.Next().ShouldBe(new Token(TokenType.EndOfStream));
        }

        [Fact]
        public void GivenValuesInQuotationMarks_WhenAskedForTokens_ThenTokenizerShouldIgnoreEmbeddedSeparator()
        {
            Tokenizer tokenizer = new Tokenizer(CreateAStreamReaderFor("\"Hello,World\""), ',');
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "Hello,World"));
            tokenizer.Next().ShouldBe(new Token(TokenType.EndOfStream));
        }

        [Fact]
        public void GivenEndOfStream_WhenAskedForEOS_ThenHasMoreShouldReturnFalse()
        {
            Tokenizer tokenizer = new Tokenizer(CreateAStreamReaderFor(""), ',');
            tokenizer.Next().ShouldBe(new Token(TokenType.EndOfStream));
            tokenizer.HasMore().ShouldBe(false);
        }

        [Fact]
        public void GivenMissingValues_WhenAskedForToken_ThenShouldReturnEmptyToken()
        {
            Tokenizer tokenizer = new Tokenizer(CreateAStreamReaderFor("Hello,,World"), ',');
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "Hello"));
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, string.Empty));
            tokenizer.Next().ShouldBe(new Token(TokenType.Value, "World"));
            tokenizer.Next().ShouldBe(new Token(TokenType.EndOfStream));
        }

        private StreamReader CreateAStreamReaderFor(string str)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(str));
            return new StreamReader(stream);
        }
    }
}