using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VacationPlanner.Constants;

namespace VacationPlanner.Models
{
  public class Employee
  {
    public Employee()
    {
    }

    public Employee(int id, string name, List<Vacation> vacations, EmployeeRole role)
    {
      Id = id;
      Name = name;
      Vacations = vacations;
      Role = role;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public List<Vacation> Vacations { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public EmployeeRole Role { get; }
  }
}