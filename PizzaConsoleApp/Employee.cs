using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace PizzaConsoleApp
{
    public class Employee : Client, IHuman
    {
        SqlConnection SqlConnection;
        public Employee()
        {
            SqlConnection = new SqlConnection(@"Data Source=DESKTOP-KOH3FDQ\SQLEXPRESS;Initial Catalog=PizzaApp;Integrated Security=true;");
        }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public void EditPassword(string newpassword, string oldpassword)
        {
            SqlConnection.Open();
            string query = $"UPDATE Employees SET EmployeePassword = @newpass,  WHERE @oldpass; ";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@newpass", newpassword);
            sqlCommand.Parameters.AddWithValue("@oldpass", oldpassword);
            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }
        public void AddPizza(string pizzaname, string pizzaingredients, string pizzaprice)
        {
            SqlConnection.Open();
            string pizzasize = "Small";
            string query = $"INSERT INTO Pizzas VALUES(@pizzaname, @pizzaingredients, @pizzasize, @pizzaprice)";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@pizzaname", pizzaname);
            sqlCommand.Parameters.AddWithValue("@pizzaingredients", pizzaingredients);
            sqlCommand.Parameters.AddWithValue("@pizzasize", pizzasize);
            sqlCommand.Parameters.AddWithValue("@pizzaprice", pizzaprice);
            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }
        public void AddSauce(string saucename, string sauceprice)
        {
            SqlConnection.Open();
            string query = $"INSERT INTO Sauces VALUES(@saucename, @sauceprice)";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@saucename", saucename);
            sqlCommand.Parameters.AddWithValue("@sauceprice", sauceprice);
            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }
        public void ConfirmRegistration()
        {
            foreach (var item in GetClients())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("If everything is fine, you can accept registration by typing clientID down bellow.");
            int clientid = int.Parse(Console.ReadLine());
            SqlConnection.Open();
            string query = $"INSERT INTO ClientRegisterConfirm VALUES(@ClientID)";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@ClientID", clientid);
            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
            Console.WriteLine("Registration accepted.");
        }
    }
}
