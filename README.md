# SeparatedValueParser
C# parser for csv, tsv or whatever separating character you like

## Usage

### Given some poco...

```
public class Record
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string University { get; set; }
    public int Age { get; set; }
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
                .WithTitles(new string[]{ "Name", "Surname", "University", "Age"})
                .Build()
                .ToList();
```
### Usage - Title that's a non-valid C# property name

Let's say we have an additional title 'honor student' in our csv file. We can't have a property with a space so we can use a SeparatedValueProperty instead:

```
public class Record
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string University { get; set; }
    public int Age { get; set; }

    [SeparatedValueProperty("Honor Student")]
    public bool HonorStudent { get; set; }
}
```