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
        public double Price { get; set; }
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
        public void Order()
        {
            string choice = "N";
            do
            {
                Console.WriteLine("Insert pizza number: ");
                int pizznum = int.Parse(Console.ReadLine());
                Console.WriteLine("What size do you want? [S]mall/[M]edium/[L]arge");
                string pizzasize = Console.ReadLine();
                Console.WriteLine("What sauce do you want");
                string sauce = Console.ReadLine();
                Console.WriteLine("Do you want delivery? [Y]es/[N]o");
                string delivery = Console.ReadLine();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = $"SELECT PizzaPrice FROM Pizzas WHERE PizzaID = @pizzanum";
                sqlCommand.Parameters.AddWithValue("@pizzanum", pizznum);
                sqlCommand.Connection = SqlConnection;
                SqlCommand sqlCommand1 = new SqlCommand();
                sqlCommand1.CommandType = System.Data.CommandType.Text;
                sqlCommand1.CommandText = $"SELECT SaucePrice FROM Sauces WHERE SauceID = @saucenum";
                sqlCommand1.Parameters.AddWithValue("@saucenum", sauce);
                sqlCommand1.Connection = SqlConnection;
                SqlConnection.Open();
                SqlDataReader datar = sqlCommand1.ExecuteReader();
                datar.Read();
                string data = datar.GetString(0);
                SqlConnection.Close();
                SqlConnection.Open();
                SqlDataReader dr = sqlCommand.ExecuteReader();
                dr.Read();
                string d = dr.GetString(0);
                SqlConnection.Close();
                switch (pizzasize)
                {
                    case "S":
                        Price += Convert.ToInt32(d);
                        break;
                    case "M":
                        Price += Convert.ToInt32(d);
                        Price *= 1.1;
                        break;
                    case "L":
                        Price += Convert.ToInt32(d);
                        Price *= 1.2;
                        break;
                    default:
                        break;
                }
                Price += Convert.ToInt32(data);
                if (delivery == "Y")
                {
                    Price += 5;
                }
                Console.WriteLine("Do you want to order something more?");
                choice = Console.ReadLine();
            } while (choice == "Y");
            Console.WriteLine("{0:00.00}", Price);
            Price = 0;
        }
    }
}
