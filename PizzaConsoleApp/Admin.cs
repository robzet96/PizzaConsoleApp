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
            string login = (employeename.ElementAt(0) + employeelastname.ElementAt(0) +""+rnd.Next(1000));
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
    }
}
