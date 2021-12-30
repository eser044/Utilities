// See https://aka.ms/new-console-template for more information
using Helper;

WriteAllFilesOnDirectory();
//MapperEx();



static void WriteAllFilesOnDirectory()
{
    var files = Util.ReadAllfilesOnSelectedPath(@"C:\Windows");
    foreach (FileInfo file in files)
        Console.WriteLine(file.FullName);
}

static void MapperEx()
{
    DbPerson source = new DbPerson
    {
        Guid = Guid.NewGuid(),
        Name = "Eser",
        SurName = "Cengiz",
        Address = "Malatya",
        Phone = "05012534444",
        Email = "eser.cengiz@softtech.com.tr",
        Title = "L4"
    };

    Person person = Util.Mapper<DbPerson, Person>(source);
}


public class Person
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}

public class DbPerson
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Title { get; set; }
}
