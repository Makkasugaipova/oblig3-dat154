using System;
using System.Collections.Generic;

namespace oblig3dat154.Models;

public partial class Student
{
    public int Id { get; set; }

    public string Studentname { get; set; } = null!;

    public int Studentage { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
