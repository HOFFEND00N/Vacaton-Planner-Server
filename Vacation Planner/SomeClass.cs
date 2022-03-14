﻿using Dapper;
using System;
using System.Data.SqlClient;
using VacationPlanner.Models;

namespace VacationPlanner
{
    public interface ISomeClass
    {
        Employee GetEmployee(int id);
        Vacation AddVacation(int employeeId, DateTime start, DateTime end);
    }

    public class SomeClass : ISomeClass
    {
        public Employee GetEmployee(int id)
        {
            const string query = "select * from Employee where Id=@Id";
            using (var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VacationPlanner;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                return connection.QueryFirst<Employee>(query, new { id });  ;
            }
        }
        public Vacation AddVacation(int employeeId, DateTime start, DateTime end)
        {
            const string query = "insert into Vacation output inserted.* values (@start, @end, 0, @employeeId)";
            using (var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VacationPlanner;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                return connection.QueryFirst<Vacation>(query, new { start, end, employeeId });
            }
        }
    }
}
