# SeparatedValueParser
C# parser for csv, tsv or whatever separating character you like

## Usage

### Some poco

```
public class Record
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string University { get; set; }
}
```

### Usage - csv file that has titles on the first line

```
var streamReader = ...;
var results = new ParserBuilder<Record>()
                .WithSeparator(',')                         // '\t' for tsv files
                .WithSource(streamReader)
                .Build()
                .ToList();
```

### Usage - csv without title line

```
var streamReader = ...;
var results = new ParserBuilder<Record>()
                .WithSeparator(',')                         // '\t' for tsv files
                .WithSource(streamReader)
                .WithTitles(new string[]{ "Name", "Surname", "University"})
                .Build()
                .ToList();
```
