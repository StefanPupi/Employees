
using System;

public class Rootobject
{
    public Employee[] Property1 { get; set; }
}

public class Employee
{
    public string Id { get; set; }
    public string EmployeeName { get; set; }
    public DateTime StarTimeUtc { get; set; }
    public DateTime EndTimeUtc { get; set; }
    public string EntryNotes { get; set; }
    public DateTime? DeletedOn { get; set; }

    
}


