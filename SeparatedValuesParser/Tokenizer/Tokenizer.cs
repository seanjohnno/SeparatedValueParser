using SeparatedValuesParser.Tokenizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsvParser
{
    public class Tokenizer
    {
        private const int EndOfLine = -1;

        private readonly Token NewlineToken = new Token(TokenType.Newline);

        private readonly StreamReader _streamReader;
        private readonly char _separatorChar;

        private List<string> _currentLine;
        private int _position;

        public Tokenizer(StreamReader streamReader, char separatorChar)
        {
            _streamReader = streamReader;
            _separatorChar = separatorChar;
        }

        public Tokenizer(StreamReader streamReader)
        {
            _streamReader = streamReader;
        }

        public Token Next()
        {
            if(_currentLine == null)
            {
                if (_streamReader.EndOfStream)
                {
                    return new Token(TokenType.EndOfStream);
                }
                else
                {
                    _currentLine = Split(_streamReader.ReadLine());
                    _position = 0;
                }
            }

            if(_position == _currentLine.Count)
            {
                _currentLine = null;
                return new Token(TokenType.Newline);
            }

            if(_currentLine != null && _position < _currentLine.Count)
            {
                return new Token(TokenType.Value, _currentLine[_position++]);
            }

            return null;
        }

        private List<string> Split(string line) => new StringSplitter(new StringStream(line), _separatorChar).ToList();

        public bool HasMore() => !_streamReader.EndOfStream;
    }
}
