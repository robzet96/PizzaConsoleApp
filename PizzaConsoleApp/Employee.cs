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
        public void DeletePizza(string pizzaname)
        {
            SqlConnection.Open();
            string query = $"DELETE FROM Pizzas WHERE PizzaName = @pizzaname";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@pizzaname", pizzaname);
            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }
        public void DeleteSauce(string saucename)
        {
            SqlConnection.Open();
            string query = $"DELETE FROM Sauces WHERE SauceName = @saucename";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@pizzaname", saucename);
            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }
        public void ConfirmRegistration()
        {
            foreach (var item in getclients())
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
        private IEnumerable<Client> getclients()
        {
            SqlConnection.Open();
            string query = $"SELECT * FROM Clients";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                clients.Add(new Client()
                {
                    ClientID = sqlDataReader.GetInt32(0),
                    Name = sqlDataReader.GetString(1),
                    LastName = sqlDataReader.GetString(2),
                    ClientPhoneNumber = sqlDataReader.GetString(3),
                    Login = sqlDataReader.GetString(4),
                    ClientAddress = sqlDataReader.GetString(5),
                    Password = sqlDataReader.GetString(6)
                });
            }
            SqlConnection.Close();
            return clients;
        }
        private IEnumerable<Orders> getorders()
        {
            Orders orders = new Orders();
            SqlConnection.Open();
            string query = $"SELECT * FROM Orders";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {                
                orders.orders.Add(new Orders()
                {
                    OrderID = sqlDataReader.GetInt32(0),
                    CustomerID = sqlDataReader.GetInt32(1),
                    EmployeeID = sqlDataReader.GetInt32(2),
                    OrderedThings = sqlDataReader.GetString(3),
                    TotalPrice = sqlDataReader.GetDouble(4)
                });
            }
            SqlConnection.Close();
            return orders.orders;
        }
        public void ConfirmOrder(int empID)
        {
            foreach (var item in getorders())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("If we can do that order type order id down bellow");
            int ordid = int.Parse(Console.ReadLine());
            SqlConnection.Open();
            string query = $"INSERT INTO ConfirmOrder VALUES (@ordid)";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@ordid", ordid);
            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
            SqlConnection.Open();
            string query1 = $"INSERT INTO Orders (EmployeeID) VALUES (@empID)";
            SqlCommand sqlCommand1 = new SqlCommand(query1, SqlConnection);
            sqlCommand1.Parameters.AddWithValue("@empID", empID);
            sqlCommand1.ExecuteNonQuery();
            SqlConnection.Close();
        }
    }
}
