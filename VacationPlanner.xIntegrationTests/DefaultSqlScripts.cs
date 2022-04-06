namespace VacationPlanner.xIntegrationTests
{
  public class DefaultSqlScripts
  {
    public const string CreateEmployeeTestData =
      "insert into [Employee] ([Id],[Name],[TeamId],[Role]) values " +
      "(0, 'Vasya Ivanov',0,0)," +
      "(1, 'Petr Petrov',0,1)," +
      "(2, 'Maksim Maksimov',1,0)";

    public const string SelectEmployeeTestData =
      "select * from EMPLOYEE where [Id] in (0,1,2)";

    public const string DeleteEmployeeTestData =
      "delete from [Employee] where [Id] in (0,1,2)";

    public const string CreateVacationTestData =
      "insert into [Vacation] ([Id], [Start],[End],[State],[EmployeeId] values)" +
      "(2, '2020-01-01', '2020-02-02', 0, 0)," +
      "(3, '2022-03-24', '2022-04-03', 0, 0)" +
      "(1002, '2022-04-11', '2022-04-21', 1, 2)";

    public const string SelectVacationTestData = "select * from [Vacation] where [Id] in (2,3,1002)";

    public const string DeleteVacationTestData = "delete from [Employee] where [Id] in (2,3,1002)";
  }
}