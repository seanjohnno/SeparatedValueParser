using CsvParser;
using Shouldly;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace CsvParserTests
{
    public class ParserTests
    {
        [Theory]
        [InlineData("csvWithTitles.csv", ',')]
        [InlineData("tsvWithTitles.tsv", '\t')]
        public void TestCsvWithTitle(string filename, char separator)
        {
            var results = new ParserBuilder<Record>()
                .WithSeparator(separator)
                .WithSource(ReadEmbeddedResource(filename))
                .Build()
                .ToList();

            results.Count.ShouldBe(2);
            results[0].ShouldBe(new Record { Name = "Testy", Surname = "Testerson", University = "Acme University" });
            results[1].ShouldBe(new Record { Name = "Bob", Surname = "Burger", University = "University of Life" });
        }

        [Theory]
        [InlineData("csvNoTitles.csv", ',')]
        [InlineData("tsvNoTitles.tsv", '\t')]
        public void TestCsvWithoutTitles(string filename, char separator)
        {
            var results = new ParserBuilder<Record>()
                .WithSeparator(separator)
                .WithSource(ReadEmbeddedResource(filename))
                .WithTitles(new string[] { "Name", "Surname", "University" })
                .Build()
                .ToList();

            results.Count.ShouldBe(2);
            results[0].ShouldBe(new Record { Name = "Testy", Surname = "Testerson", University = "Acme University" });
            results[1].ShouldBe(new Record { Name = "Bob", Surname = "Burger", University = "University of Life" });
        }

        private StreamReader ReadEmbeddedResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return new StreamReader(assembly.GetManifestResourceStream($"{assembly.GetName().Name}.TestFiles.{name}"));
        }

        public class Record
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string University { get; set; }

            public override bool Equals(object obj)
            {
                var record = obj as Record;
                return record != null &&
                       Name == record.Name &&
                       Surname == record.Surname &&
                       University == record.University;
            }
        }
    }
}
