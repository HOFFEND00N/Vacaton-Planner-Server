namespace VacationPlanner.xIntegrationTests
{
  public class DefaultSqlScripts
  {
    public static string CreateEmployeeTestData()
    {
      return "insert into [Employee] ([Name],[TeamId],[Role]) values " +
             "('Vasya Ivanov',0,0)," +
             "('Petr Petrov',0,1)," +
             "('Maksim Maksimov',1,0)";
    }

    public static string SelectEmployeeTestData()
    {
      return "select * from [Employee] where [Name] in ('Vasya Ivanov','Petr Petrov','Maksim Maksimov')";
    }

    public static string DeleteEmployeeTestData()
    {
      return "delete from [Employee] where [Name] in ('Vasya Ivanov','Petr Petrov','Maksim Maksimov')";
    }

    const string firstEmployeeId =
      "select [Id] from [Employee] where [Name] = 'Vasya Ivanov' and [TeamId] = 0 and [Role] = 0";

    const string secondEmployeeId =
      "select [Id] from [Employee] where [Name] = 'Petr Petrov' and [TeamId] = 0 and [Role] = 1";

    const string thirdEmployeeId =
      "select [Id] from [Employee] where [Name] = 'Maksim Maksimov' and [TeamId] = 1 and [Role] = 0";

    public static string CreateVacationTestData()
    {
      return "insert into [Vacation] ([Start],[End],[State],[EmployeeId]) values" +
             $"('2020-01-01', '2020-02-02', 0, ({firstEmployeeId}))," +
             $"('2022-03-24', '2022-04-03', 0, ({firstEmployeeId}))," +
             $"('2022-04-11', '2022-04-21', 1, ({thirdEmployeeId}))";
    }

    public static string SelectVacationTestData()
    {
      return
        $"select * from [Vacation] where [EmployeeId] in (({firstEmployeeId}),({secondEmployeeId}),({thirdEmployeeId}))";
    }

    public static string DeleteVacationTestData()
    {
      return
        $"delete from [Vacation] where [EmployeeId] in (({firstEmployeeId}),({secondEmployeeId}),({thirdEmployeeId}))";
    }

    public static string CreateDb()
    {
      return "IF DB_ID('VacationPlanner') IS NULL BEGIN create database [VacationPlanner] END";
    }

    public static string CreateTables()
    {
      return @"use VacationPlanner

      if OBJECT_ID('Employee') is null
      begin
        create table Employee
      (
        Id     int identity (1,1) not null
      primary key,
        Name   nvarchar(200)      not null,
        TeamId int                not null,
        Role   int
        )
      end

      if OBJECT_ID('Team') is null
      begin
        create table Team
      (
        Id   int identity (1,1) not null
      primary key,
        Name nvarchar(100)      not null
        )
      end

      if OBJECT_ID('Vacation') is null
      begin
        create table Vacation
      (
        Id         int identity (1,1) not null
      primary key,
        Start      date               not null,
        [End]      date               not null,
        State      int                not null,
        EmployeeId int                not null
      )
      end";
    }
  }
}