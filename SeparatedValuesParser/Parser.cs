using SeparatedValuesParser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CsvParser
{
    public class Parser<T> : IEnumerable<T>
    {
        private readonly char _separator;
        private readonly Tokenizer _tokenizer;
        private readonly List<string> _titles;
        private readonly List<PropertyInfo> _properties = new List<PropertyInfo>();

        public Parser(char separator, Tokenizer streamReader, List<string> titles)
        {
            _separator = separator;
            _tokenizer = streamReader;
            _titles = titles ?? new List<string>();
        }

        public IEnumerator<T> GetEnumerator() => GetTypedEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetTypedEnumerator();

        public IEnumerator<T> GetTypedEnumerator()
        {
            PopulateTitles();

            List<string> currentList = new List<string>(_titles.Count);
            bool finished = false;
            while(!finished)
            {
                var next = _tokenizer.Next();
                if (next.TokenType == TokenType.Value)
                {
                    currentList.Add(next.Value);
                }
                else if(currentList.Count > 0)
                {
                    yield return PopulateObject(currentList);
                    currentList.Clear();
                }
                else if(next.TokenType == TokenType.EndOfStream)
                {
                    finished = true;
                }
            }
        }

        private void PopulateTitles()
        {
            if (_titles.Count == 0)
            {
                Token next = null;
                while ((next = _tokenizer.Next()).TokenType != TokenType.Newline && next.TokenType != TokenType.EndOfStream)
                {
                    _titles.Add(next.Value);
                }
            }

            _titles.ForEach(t =>
            {
                var prop = typeof(T).GetProperties().First(pi => Match(t, pi));
                _properties.Add(prop);
            });
        }

        private bool Match(string name, PropertyInfo info)
        {
            var prop = info.GetCustomAttribute<SeparatedValueProperty>();
            return (prop != null && name.Equals(prop.Title, StringComparison.OrdinalIgnoreCase)) ||
                info.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
        }

        private T PopulateObject(List<string> values)
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            int index = 0;
            _properties.ForEach(pi => {
                var val = StringToType.ToType(values[index++], pi.PropertyType);
                pi.SetValue(obj, val);
                });
            return obj;
        }
    }
}
