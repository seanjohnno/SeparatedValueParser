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
        private readonly Dictionary<string, PropertyInfo> _titleToProperty;

        public Parser(char separator, Tokenizer streamReader, List<string> titles)
        {
            _separator = separator;
            _tokenizer = streamReader;
            _titles = titles ?? new List<string>();
            _titleToProperty = new Dictionary<string, PropertyInfo>();
        }

        public IEnumerator<T> GetEnumerator() => GetTypedEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetTypedEnumerator();

        public IEnumerator<T> GetTypedEnumerator()
        {
            PopulateTitles();

            var currentList = new Dictionary<PropertyInfo, string>();
            var finished = false;
            var index = 0;
            while(!finished)
            {
                var next = _tokenizer.Next();
                if (next.TokenType == TokenType.Value)
                {
                    var title = _titles[index];
                    _titleToProperty.TryGetValue(title, out PropertyInfo info);
                    if(info != null)
                    {
                        currentList[info] = next.Value;
                    }

                    index++;
                }
                else if(next.TokenType == TokenType.Newline)
                {
                    yield return PopulateObject(currentList);
                    currentList.Clear();
                    index = 0;
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
                var prop = typeof(T).GetProperties().FirstOrDefault(pi => Match(t, pi));
                if(prop != null)
                {
                    _titleToProperty.Add(t, prop);
                }
            });
        }

        private bool Match(string name, PropertyInfo info)
        {
            var prop = info.GetCustomAttribute<SeparatedValueProperty>();
            return (prop != null && name.Equals(prop.Title, StringComparison.OrdinalIgnoreCase)) ||
                info.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
        }

        private T PopulateObject(Dictionary<PropertyInfo, string> values)
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            values.Keys.ToList().ForEach(k =>
            {
                var val = values[k];
                if(!string.IsNullOrEmpty(val))
                {
                    var parsedVal = StringToType.ToType(val, k.PropertyType);
                    k.SetValue(obj, parsedVal);
                }
            });
            return obj;
        }
    }
}
