using System;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using VacationPlanner.DataAccess.Models;

namespace VacationPlanner.DataAccess
{
    public class DbService : IDbService
    {
        private string DbConnectionString { get; set; }

        public DbService(IConfiguration config)
        {
            DbConnectionString = config["DbConnectionString"];
        }

        public DataEmployee GetEmployee(int employeeId)
        {
            const string query = "select * from Employee where Id=@Id";
            using var connection = new SqlConnection(DbConnectionString);
            return connection.QueryFirst<DataEmployee>(query, new {id = employeeId});
        }

        public DataVacation AddVacation(int employeeId, DateTime start, DateTime end)
        {
            const string query = "insert into Vacation output inserted.* values (@start, @end, 0, @employeeId)";
            using var connection = new SqlConnection(DbConnectionString);
            return connection.QueryFirst<DataVacation>(query, new {start, end, employeeId});
        }

        public DataVacation DeleteVacation(int employeeId, int vacationId)
        {
            const string query =
                "delete from Vacation output deleted.* where Id = @employeeId and EmployeeId = @vacationId";
            using var connection = new SqlConnection(DbConnectionString);
            return connection.QueryFirst<DataVacation>(query, new {employeeId, vacationId});
        }
    }
}