# CsvParser `v1.1.0`

The CsvParser version `1.1.0` is now out with some new features, bug-fixes and a new documentation to support the newly added code.


 ##  Custom Attribute usage method:
Now you can extract data from your CSV using the `CsvSourceColumn` annotation. It makes the process that was already simplified, even more easier. Let's suppose you have the following content:
- A CSV file named `example.csv`:
```
name,age
john,18
johanne,18
```
- And a data holder class:
```csharp
public class Person
{
    public string Name { get; set; }
    
    public string Age { get; set; }
}
```
So, to beggin the extraction you should mark the data holder class with the `CsvSourceColumn` custom attribute, passing as positional argument the column index that contains the data the marked attribute will hold.
In the exemplified `example.csv` file, we have the name column positioned at index `0` and the age column positioned at index `1 `, therefore, we should mark our data holder class in the following way:
```csharp
public class Person
{
    [CsvSourceColumn(0)]
    public string Name { get; set; }

    [CsvSourceColumn(1)]
    public string Age { get; set; }
}
```
And then, in ourhipothetical Program.cs class, where we'll execute the data extraction, we should read the `example.csv` hipothetical file and instantiate a `CsvParser.CsvObjectParser` class object:


```csharp
class Program
{
    static void Main(string[] args)
    {
        string csvString = File.ReadAllText("example.csv");
        CsvObjectParser extractor = new();
    }
}
```

And finally simply call the `.Parse` method, passing the data holder class type as the required generic argument, the `csvString` variable containing the CSV file content, and if desired, a few **optional options** and we are done. You can even make a few assertions to ensure everything is correct:
```csharp
class Program
{
    static void Main(string[] args)
    {
        string csvString = File.ReadAllText("example.csv");

        CsvObjectParser extractor = new();
        
        List<Person> targetedPersonList = extractor.Parse<Person>(
            csvString, 
            new ExtractOptions
            { 
                ShouldSkipHeader = true
            });
            
		Debug.Assert(targetedPersonList.Count.Equals(2));
        Debug.Assert(targetedPersonList[0].Name.Equals("john"));
        Debug.Assert(targetedPersonList[0].Age.Equals("18"));
        Debug.Assert(targetedPersonList[1].Name.Equals("johanne"));
        Debug.Assert(targetedPersonList[1].Age.Equals("18"));
    }
}
```


But if for some reason you don't want to use the custom attribute method you can still use the traditional one as explained bellow.
## Target List usage method:
Let's suppose you have the same `example.csv` file and data holder class from the previous example.
So, to start the extraction, read the `example.csv` file content,  instantiante a `CsvParser.CsvObjectParser` class object and create the target list:
```csharp
class Program
{
    static void Main(string[] args)
    {
        string csvString = File.ReadAllText("example.csv");

        CsvObjectParser extractor = new();
        List<CsvTarget> targetList = new()
        {
            new("Name", 0),
            new("Age", 1)
        };
    }
}
```
We pass the attribute of the data holder class that will hold the desired data as the first argument, and the index of the column that will contain the data as the second one. In the `example.csv` we have the person name stored in the first column, and the person age stored in the second column. And we want the person name to be stored in the `Name` attribute of the `Person` class, just like we want the person age to be stored within the `Age` attribute.
Then, we can simply call the `.Parse` method passing the target list as the first argument, the `csvString` variable as the second one, and an options object (use an empty constructor if you don't want to change anything):
```csharp
class Program
{
    static void Main(string[] args)
    {
        string csvString = File.ReadAllText("example.csv");

        CsvObjectParser extractor = new();
        List<CsvTarget> targetList = new()
            {
                new("Name", 0),
                new("Age", 1)
            };
        
        List<Person> personList = extractor.Parse<Person>(
	        targetList,
            csvString, 
            new ExtractOptions
            { 
                ShouldSkipHeader = true
            });
            
		Debug.Assert(personList.Count.Equals(2));
        Debug.Assert(personList[0].Name.Equals("john"));
        Debug.Assert(personList[0].Age.Equals("18"));
        Debug.Assert(personList[1].Name.Equals("johanne"));
        Debug.Assert(personList[1].Age.Equals("18"));
    }
}
```
## Extract Options:
You can customize your csv data extraction passing an object of the following class as the last argument for the `CsvParser.CsvObjectParser.Parse` method:
> When using the nuget installed version of this package, you obviously won't be able to access this class in this manner, I only pasted it here so you can understant it's content in a better way.
```csharp
public class ExtractOptions
{
    public string Separator { get; set; } = ",";

    public bool ShouldSkipHeader { get; set; } = false;

    public bool ShouldSkipEqualObject { get; set; } = false;
}
```
### Separator
This option is used by the parser to know how the csv column are delimited. For example; if your csv columns are separed by a semicolon,  pass `;` to this option and we'll be good to go.
### ShouldSkipHeader
If received `true`, the parser will skip the csv first line. 
### ShouldSkipEqualObject
If received `true`, the parser will not add equal objects.
> Note that for this option to behave as expected, the data holder class must implement the `IEquatable` interface.

> Note that this option may slow down the performance as the code have an algorithm complexity of `n²` where `n` is equal to the csv lines to be parsed amount when this option is activated, but nothing too expensive.

# Version `1.1.0` changelog:
## Features:
- Now you can extract csv data through the `CsvSourceColumn` custom attribute;
- The `CsvDataExtractor` and it's methods are now marked as obsolete;
- `CsvObjectParser` class created to replace it's former version `CsvDataExtractor`;
- `ShouldRepeat` option name as well it's inner logic changed to `ShouldSkipEqualObject`;
- `HaveHeader` option name changed to `ShouldSkipHeader`.

## Bugfixes
- `ShouldSkipEqualObject` inner logic algorithm complexity reduced.

# Final words
Well, I bet you were expecting more text here in this section. But if you enjoyed this package/repository or/and it was useful to you, please star it. If you have any suggestion, commentary or bug report, please, feel free to contact me :). Also you can open pull requests anytime for review and potential merge if you feel like.
> Written with [StackEdit](https://stackedit.io/).
