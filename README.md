# csharp-csv-parser

Have you ever wondered if there was an easy way to convert CSV lines to an object of your choice? Yeah, I not only did, but I created an easy way for you and me.
Let's follow these steps:

First one:
- Install the nuget package:
```dotnet add package Alliguieri.CsvParser --version 1.0.0```

- And add the package namespace to your code:
```using CsvParser;```

Then let's say you have the following csv string in a file called ```example.csv```:
```
name,age
john,21
johane,22
```

- And the following class you want to generate objects using the previous mentioned string:
```
public class Person
{
    public string Name { get; set; }

    public string Age { get; set; }
}
```

- Create a new instance of ```CsvDataExtractor``` in your code:
```
class Program
{
    static void Main(string[] args)
    {
        string csvString = File.ReadAllText("./example.csv");
        CsvDataExtractor extractor = new();
    }
}
```
- Now we need to tell the extractor on what attribute it should put the selected data,
and we'll  do this by creating a ```List<CsvTarget>```;
```
class Program
{
    static void Main(string[] args)
    {
        CsvDataExtractor extractor = new();
        string csvString = File.ReadAllText("./teste.csv");
        List<CsvTarget> targets = new() {
            new("Name", 0),
            new("Age", 1)
        };
    }
}
```

- Now let's explain what those ```"Name", 0``` and ```"Age",  1``` means:

For each line of the CSV, the extractor will store the data contained within the column 0 in the attribute ```Name``` of the ```Person``` class,
and the same logic can be applied to the second item in the list. The extractor will store the data contained with the column 1 in the ```Age``` attribute
of the ```Person``` class.
Note that the names your pass as the first argument to the ```CsvTarget``` constructor must have the same name as the class equivalent attribute. An example is
the ```Person``` class, as it have both ```Name``` and ```Age``` as attributes.

- But how can we tell the extractor to wich class i want to extract the data to? And most importantly, how can I extract the data?

It's very simple indeed! Let's see the following code:
```
class Program
{
    static void Main(string[] args)
    {
        CsvDataExtractor extractor = new();
        string csvString = File.ReadAllText("./teste.csv");
        List<CsvTarget> targets = new() {
            new("Name", 0),
            new("Age", 1)
        };

        List<Person> personList = extractor.ExtractData<Person>(
            targets,
            csvString,
            new()
        );
    }
}
```
We created a ```List<Person>```, and then called the ```extractor.ExtractData<Person()``` method. Giving the ```Person``` class to the Generic parameter of this
method is all you need to do to tell the extractor to wich class you want to generate objects from and populate them with the csv.

- Well, I called the method passing the target list as first argument and the csvString as the second argument, but what about that completely subjective third
argument ```new()```?

The third argument is an options argument. Let's see how it's class is defined:
```
public class ExtractOptions
{
    public string Separator { get; set; } = ",";

    public bool HaveHeader { get; set; } = false;

    public bool ShouldRepeat { get; set; } = true;
}
```

So let's say the csv you're using does not use "," as a column separator, but uses ";" intead. Then you should call the extract method in the following way:
```
extractor.ExtractData<Person>(
    targets,
    csvString,
    new ExtractOptions {
    Separator = ";"
});
```
Or your CSV file have a header that you might want to ignore, then you could use the ```HaveHeader``` attribute:
```extractor.ExtractData<Person>(
    targets,
    csvString,
    new ExtractOptions {
    Separator = ";",
    HaveHeader = true
});
```

Bellow there's a list of each option you can add and their respective explanation:
- ```string Separator```

The extractor will use the content of this attribute to split the csv columns (```csvLine.Split(options.Separator)```);
This attribute have a default value of ```","```.

- ```bool HaveHeader```

If your CSV have a header that you want to skip, just set this attribute to ```true```.

- ```bool ShouldRepeat```

If you don't want the extractor to add simmilar objects set this attribute to ```true```;
Note that the class your're extracting csv data to, must implement the ```IEquatable<T>``` C# interface for this option to work.

I hope this package can help you. If you have any questions, sugestions or bug to tell me about, just send me a message :).

