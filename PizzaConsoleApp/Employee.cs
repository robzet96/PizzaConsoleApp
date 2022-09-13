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
        public List<Employee> Employees = new List<Employee>();
        
        public Employee()
        {
            SqlConnection = new SqlConnection(@"Data Source=DESKTOP-KOH3FDQ\SQLEXPRESS;Initial Catalog=PizzaApp;Integrated Security=true;");
        }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public override string ToString()
        {
            return "Employee ID: " + EmployeeID + "\tName: " + Name + "\tLast name: " + LastName  + "\tLogin: " + Login + "\tPassword: " + Password;
        }
        public void EditPassword(string newpassword, int empid)
        {
            SqlConnection.Open();
            string query = $"UPDATE Employees SET EmployeePassword = @newpass WHERE EmployeeID = @empid; ";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@newpass", newpassword);
            sqlCommand.Parameters.AddWithValue("@empid", empid);
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
            try
            {
                SqlConnection.Open();
                string query = $"DELETE FROM Pizzas WHERE PizzaName = @pizzaname";
                SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
                sqlCommand.Parameters.AddWithValue("@pizzaname", pizzaname);
                sqlCommand.ExecuteNonQuery();
                SqlConnection.Close();
            }
            catch (Exception)
            {
                SqlConnection.Close();
                Console.WriteLine("There is no pizza with this name.");
            }
        }
        public void DeleteSauce(string saucename)
        {
            try
            {
                SqlConnection.Open();
                string query = $"DELETE FROM Sauces WHERE SauceName = @saucename";
                SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
                sqlCommand.Parameters.AddWithValue("@pizzaname", saucename);
                sqlCommand.ExecuteNonQuery();
                SqlConnection.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("There is no sauce with this name."); 
            }
        }
        public void ConfirmRegistration()
        {
            unconfirmedclients();
            Console.WriteLine("If everything is fine, you can accept registration by typing clientID down bellow.");
            int clientid = 0;
            try
            {
                clientid = int.Parse(Console.ReadLine());
                SqlConnection.Open();
                string query = $"INSERT INTO ClientRegisterConfirm VALUES(@ClientID)";
                SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
                sqlCommand.Parameters.AddWithValue("@ClientID", clientid);
                sqlCommand.ExecuteNonQuery();
                SqlConnection.Close();
                Console.WriteLine("Registration accepted.");
                Console.ReadKey();
            }
            catch (Exception)
            {
                sqlConnection.Close();
                Console.WriteLine("It is not a number");
            }
            
        }
        public IEnumerable<Client> getclients()
        {
            clients.Clear();
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
        public IEnumerable<int> getconfirmclients()
        {
            confirmclient.Clear();
            SqlConnection.Open();
            string query = $"SELECT ClientID FROM ClientRegisterConfirm";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                confirmclient.Add(sqlDataReader.GetInt32(0));
            }
            SqlConnection.Close();
            return confirmclient;
        }
        public IEnumerable<Orders> getorders()
        {           
            orderlist.Clear();
            SqlConnection.Open();
            string query = $"SELECT * FROM Orders";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {                
                orderlist.Add(new Orders()
                {
                    OrderID = sqlDataReader.GetInt32(0),
                    CustomerID = sqlDataReader.GetInt32(1),
                    EmployeeID = sqlDataReader.IsDBNull(2) ? null : sqlDataReader.GetInt32(2),
                    OrderedThings = sqlDataReader.GetString(3),
                    TotalPrice = sqlDataReader.GetDouble(4),
                });
            }
            SqlConnection.Close();
            return orderlist;
        }
        public void ConfirmOrder(int empID)
        {
            try
            {
                DisplayMenus(getorders());
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
            catch (Exception)
            {
                Console.WriteLine("There is no orders at the moment.");
            }
        }
        private void confirmedclients()
        {
            List<Client> confirm = new List<Client>();
            int id = 0;
            foreach (var item in getclients())
            {
                if (getconfirmclients().Contains(item.ClientID))
                {
                    id = item.ClientID;
                    SqlConnection.Open();
                    string query = $"SELECT * FROM Clients WHERE ClientID = {id}";
                    SqlCommand sqlCommand = new SqlCommand(query,SqlConnection);
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        confirm.Add(new Client()
                        {
                            ClientID = sqlDataReader.GetInt32(0),
                            Name = sqlDataReader.GetString(1),
                            LastName = sqlDataReader.GetString(2),
                            Login = sqlDataReader.GetString(3),
                            ClientAddress = sqlDataReader.GetString(5),
                            Password = sqlDataReader.GetString(6)
                        });
                    }
                    SqlConnection.Close();                    
                }
            }
            DisplayMenus(confirm);
        }
        private void unconfirmedclients()
        {
            List<Client> confirm = new List<Client>();
            confirm.Clear();
            int id = 0;
            foreach (var item in getclients())
            {                
                if (!getconfirmclients().Contains(item.ClientID))
                {                    
                    id = item.ClientID;
                    SqlConnection.Open();
                    string query = $"SELECT * FROM Clients WHERE ClientID = {id}";
                    SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {                        
                        confirm.Add(new Client()
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
                }
            }            
            DisplayMenus(confirm);            
        }
    }
}
