using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PizzaConsoleApp
{
    public class Client : IHuman
    {
        SqlConnection SqlConnection;
        public Client()
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
            string query = $"UPDATE Clients SET ClientPassword = @newpass WHERE ClientPassword = @oldpass; ";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@newpass", newpassword);
            sqlCommand.Parameters.AddWithValue("@oldpass", oldpassword);
            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }
        public void Register()
        {
            Console.WriteLine("Name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Last name: ");
            string lastname = Console.ReadLine();
            Console.WriteLine("Phone number: ");
            string phonenumber = Console.ReadLine();
            Console.WriteLine("Email: ");
            string email = Console.ReadLine();
            Console.WriteLine("Address: ");
            string address = Console.ReadLine();
            Console.WriteLine("Password: ");
            string password = Console.ReadLine();
            SqlConnection.Open();
            string query = $"INSERT INTO Clients VALUES(@name, @lastname, @phonenumber, @email, @address, @pass); ";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@name", name);
            sqlCommand.Parameters.AddWithValue("@lastname", lastname);
            sqlCommand.Parameters.AddWithValue("@phonenumber", phonenumber);
            sqlCommand.Parameters.AddWithValue("@email", email);
            sqlCommand.Parameters.AddWithValue("@address", address);
            sqlCommand.Parameters.AddWithValue("@pass", password);
            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }
    }
}
