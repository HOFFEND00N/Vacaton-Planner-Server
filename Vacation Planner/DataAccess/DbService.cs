using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using VacationPlanner.Constants;
using VacationPlanner.DataAccess.Models;

namespace VacationPlanner.DataAccess
{
    public class DbService : IDbService
    {
        private readonly string _dbConnectionString;

        public DbService(IConfiguration config)
        {
            _dbConnectionString = config["DbConnectionString"];
        }

        public DataEmployee GetEmployee(int employeeId)
        {
            const string query = "select * from Employee where Id=@id";
            using var connection = new SqlConnection(_dbConnectionString);
            var employee = connection.QueryFirst<DataEmployee>(query, new {id = employeeId});

            employee.Vacations = new List<DataVacation>();
            employee.Vacations.AddRange(GetEmployeeVacations(employeeId, connection));
            return employee;
        }

        private List<DataVacation> GetEmployeeVacations(int employeeId, SqlConnection connection)
        {
            const string query = "select * from Vacation where EmployeeId = @employeeId";
            return connection.Query<DataVacation>(query, new {employeeId}).ToList();
        }

        public DataVacation AddVacation(int employeeId, DateTime start, DateTime end)
        {
            const string query = "insert into Vacation output inserted.* values (@start, @end, 0, @employeeId)";
            using var connection = new SqlConnection(_dbConnectionString);
            return connection.QueryFirst<DataVacation>(query, new {start, end, employeeId});
        }

        public DataVacation DeleteVacation(int vacationId)
        {
            const string query =
                "delete from Vacation output deleted.* where Id = @vacationId";
            using var connection = new SqlConnection(_dbConnectionString);
            return connection.QueryFirst<DataVacation>(query, new {vacationId});
        }

        public DataVacation EditVacation(int vacationId, DateTime start, DateTime end)
        {
            const string updateQuery =
                "update Vacation set Start = @start, [End] = @end where Id = @vacationId";
            using var connection = new SqlConnection(_dbConnectionString);
            connection.Query(updateQuery, new {start, end, vacationId});

            const string selectQuery = "select * from Vacation where Id = @vacationId";
            return connection.QueryFirst<DataVacation>(selectQuery, new {vacationId});
        }

        public DataVacation ChangeVacationState(int vacationId, VacationState state)
        {
            const string query = "update Vacation output inserted.* set State=@state where Id = @vacationId";
            using var connection = new SqlConnection(_dbConnectionString);
            return connection.QueryFirst<DataVacation>(query, new {state, vacationId});
        }

        public IEnumerable<DataEmployee> GetTeamMembers(int teamId)
        {
            const string query =
                "select e.Id as EmployeeId, Name, TeamId, Role, v.Id as VacationId, \"Start\", \"End\", State, v.EmployeeId as VacationEmployeeId from Employee  as e left join Vacation as v on e.Id = v.EmployeeId where TeamId = 0";
            using var connection = new SqlConnection(_dbConnectionString);
            var result = connection.Query<DataEmployeeAndDataVacation>(query, new {teamId});

            List<DataEmployee> employees = new List<DataEmployee>();
            foreach (var employeeAndVacation in result)
            {
                var employee = employees.Find(employee => employee.Id == employeeAndVacation.EmployeeId);
                if (employee == null)
                {
                    employee = new DataEmployee(employeeAndVacation.EmployeeId,
                        employeeAndVacation.Name, new List<DataVacation>(),
                        employeeAndVacation.Role, employeeAndVacation.TeamId);
                    employees.Add(employee);
                }

                if (employeeAndVacation.VacationId.HasValue && employeeAndVacation.Start.HasValue &&
                    employeeAndVacation.End.HasValue && employeeAndVacation.State.HasValue &&
                    employeeAndVacation.VacationEmployeeId.HasValue)
                {
                    employee.Vacations.Add(new DataVacation(employeeAndVacation.VacationId.Value,
                        employeeAndVacation.Start.Value, employeeAndVacation.End.Value,
                        employeeAndVacation.State.Value, employeeAndVacation.VacationEmployeeId.Value));
                }
            }

            return employees;
        }

        public DataVacation GetVacation(int vacationId)
        {
            const string query = "select * from Vacation where Id = @vacationId";
            using var connection = new SqlConnection(_dbConnectionString);
            return connection.QueryFirst<DataVacation>(query, new {vacationId});
        }
    }
}