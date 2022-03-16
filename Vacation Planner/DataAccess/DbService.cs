using Dapper;
using System;
using System.Data.SqlClient;
using VacationPlanner.DataAccess;
using VacationPlanner.Models;

namespace VacationPlanner
{
    public class DbService : IDbService
    {
        private string DbConnectionString { get; set; }
        public DbService(string dbConnectionString)
        {
            DbConnectionString = dbConnectionString;
        }

        public Employee GetEmployee(int id)
        {
            const string query = "select * from Employee where Id=@Id";
            using (var connection = new SqlConnection(DbConnectionString))
            {
                return connection.QueryFirst<Employee>(query, new { id });  ;
            }
        }
        public Vacation AddVacation(int employeeId, DateTime start, DateTime end)
        {
            const string query = "insert into Vacation output inserted.* values (@start, @end, 0, @employeeId)";
            using (var connection = new SqlConnection(DbConnectionString))
            {
                return connection.QueryFirst<Vacation>(query, new { start, end, employeeId });
            }
        }
    }
}