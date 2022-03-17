using Dapper;
using System;
using System.Data.SqlClient;
using VacationPlanner.DataAccess;
using VacationPlanner.DataAccess.Models;

namespace VacationPlanner
{
    public class DbService : IDbService
    {
        private string DbConnectionString { get; set; }
        public DbService(string dbConnectionString)
        {
            DbConnectionString = dbConnectionString;
        }

        public DataEmployee GetEmployee(int id)
        {
            const string query = "select * from Employee where Id=@Id";
            using (var connection = new SqlConnection(DbConnectionString))
            {
                return connection.QueryFirst<DataEmployee>(query, new { id });  ;
            }
        }
        public DataVacation AddVacation(int employeeId, DateTime start, DateTime end)
        {
            const string query = "insert into Vacation output inserted.* values (@start, @end, 0, @employeeId)";
            using (var connection = new SqlConnection(DbConnectionString))
            {
                return connection.QueryFirst<DataVacation>(query, new { start, end, employeeId });
            }
        }
    }
}