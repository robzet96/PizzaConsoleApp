using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PizzaConsoleApp
{
    public class Admin : Employee
    {
        SqlConnection SqlConnection;
        Random rnd = new Random();
        public Admin()
        {
            SqlConnection = new SqlConnection(@"Data Source=DESKTOP-KOH3FDQ\SQLEXPRESS;Initial Catalog=PizzaApp;Integrated Security=true;");
        }
        public void DeleteEmployee(string employeeid)
        {
            SqlConnection.Open();
            string query = $"DELETE FROM Employees WHERE EmployeeID = @employeeid";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@employeeid", employeeid);
            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }
        private string employeelogin(string employeename, string employeelastname)
        {
            string login = (Convert.ToString(employeename.ElementAt(0)) + Convert.ToString(employeelastname.ElementAt(0)) + "" + rnd.Next(1000));
            return login;
        }
        public void AddEmployee(string employeename, string employeelastname)
        {
            SqlConnection.Open();
            string query = $"INSERT INTO Employees VALUES(@employeename, @employeelastname, @employeepassword, @employeelogin)";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@employeename", employeename);
            sqlCommand.Parameters.AddWithValue("@employeelastname", employeelastname);
            sqlCommand.Parameters.AddWithValue("@employeepassword", "changepassplease");
            sqlCommand.Parameters.AddWithValue("@employeelogin", employeelogin(employeename, employeelastname));
            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }
        public IEnumerable<Employee> getemployees()
        {
            SqlConnection.Open();
            string query = $"SELECT * FROM Employees";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Employees.Add(new Employee()
                {
                    EmployeeID = sqlDataReader.GetInt32(0),
                    Name = sqlDataReader.GetString(1),
                    LastName = sqlDataReader.GetString(2),
                    Password = sqlDataReader.GetString(3),
                    Login = sqlDataReader.GetString(4),                   
                });
            }
            SqlConnection.Close();
            return Employees;
        }
           
    }
}
