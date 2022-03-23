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
            const string query = "select * from Employee where Id=@Id";
            using var connection = new SqlConnection(_dbConnectionString);
            var employee = connection.QueryFirst<DataEmployee>(query, new {id = employeeId});

            employee.Vacations.AddRange(GetEmployeeVacations(employeeId, connection));
            return employee;
        }

        private List<DataVacation> GetEmployeeVacations(int employeeId, SqlConnection connection)
        {
            const string query = "select * from Vacation where EmployeeId = @EmployeeId";
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
            const string query =
                "update Vacation output inserted.* set Start = @start, End = @end where Id = @vacationId";
            using var connection = new SqlConnection(_dbConnectionString);
            return connection.QueryFirst<DataVacation>(query, new {start, end, vacationId});
        }

        public DataVacation ChangeVacationState(int vacationId, VacationState state)
        {
            const string query = "update Vacation output inserted.* set State=@state where Id = @vacationId";
            using var connection = new SqlConnection(_dbConnectionString);
            return connection.QueryFirst<DataVacation>(query, new {state, vacationId});
        }

        public List<DataEmployee> GetTeamMembers(int teamId)
        {
            const string query = "select * from Employee where TeamId = @teamId";
            using var connection = new SqlConnection(DbConnectionString);
            return connection.Query<DataEmployee>(query, new {teamId}).ToList();
        }

        public DataVacation GetVacation(int vacationId)
        {
            const string query = "select * from Vacation where Id = @vacationId";
            using var connection = new SqlConnection(_dbConnectionString);
            return connection.QueryFirst<DataVacation>(query, new {vacationId});
        }
    }
}