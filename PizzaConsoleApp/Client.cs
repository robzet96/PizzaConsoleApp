using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PizzaConsoleApp
{
    public class Client : Pizza, IHuman
    {
        SqlConnection SqlConnection;
        public List<Client> clients = new List<Client>();
        public Client()
        {
            SqlConnection = new SqlConnection(@"Data Source=DESKTOP-KOH3FDQ\SQLEXPRESS;Initial Catalog=PizzaApp;Integrated Security=true;");
        }
        public int ClientID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ClientAddress { get; set; }
        public string ClientPhoneNumber { get; set; }
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
        public void Order(int id)
        {
            Price = 0;
            List<string> pizzaorder = new List<string>();
            string delivery;
            string address = "";
            TimeOnly timeOnly;
            string choice = "N";
            DisplayMenus(GetPizza());
            Console.WriteLine();
            DisplayMenus(GetSauce());
            do
            {               
                Console.WriteLine("Insert pizza number: ");
                int pizznum = int.Parse(Console.ReadLine());
                Console.WriteLine("What size do you want? [S]mall/[M]edium/[L]arge");
                string pizzasize = Console.ReadLine();               
                Console.WriteLine("What sauce do you want");
                string sauce = Console.ReadLine();
                pizzaorder.Add(Convert.ToString(pizznum));
                pizzaorder.Add(pizzasize);
                pizzaorder.Add(sauce);
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
                switch (pizzasize.ToUpper())
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
                Console.WriteLine("Do you want to order something more?");
                choice = Console.ReadLine();
            } while (choice.ToUpper() == "Y");
            Console.WriteLine("Do you want delivery? [Y]es/[N]o");
            delivery = Console.ReadLine();
            pizzaorder.Add(delivery);
            if (delivery.ToUpper() == "Y")
            {
                Console.WriteLine("Should we deliver pizza to address from your account? [Y]es/N[o]");
                string addresschoice = Console.ReadLine();
                if (addresschoice.ToUpper() == "Y")
                {
                    SqlConnection.Open();
                    SqlCommand sqlCommand2 = new SqlCommand();
                    sqlCommand2.CommandType = System.Data.CommandType.Text;
                    sqlCommand2.CommandText = $"SELECT ClientAddres FROM Clients WHERE ClientID = {id}";
                    sqlCommand2.Connection = SqlConnection;
                    SqlDataReader sqlDataReader = sqlCommand2.ExecuteReader();
                    sqlDataReader.Read();
                    address = sqlDataReader.GetString(0);
                    SqlConnection.Close();
                }
                else if (addresschoice.ToUpper() == "N")
                {
                    Console.WriteLine("Insert delivery address: ");
                    address = Console.ReadLine();
                }
                pizzaorder.Add(address);
                Price += 5;
            }
            Console.WriteLine("When do you want receive your pizza?");
            timeOnly = TimeOnly.Parse(Console.ReadLine());
            pizzaorder.Add(timeOnly.ToString());
            Console.WriteLine("{0:00.00}", Price);
            string orderedthings = "";
            orderedthings = pizzaorder.Aggregate((current, next) => current + " " + next);
            Console.WriteLine(orderedthings);
            SqlConnection.Open();
            string query = $"INSERT INTO Orders (ClientID, OrderedThings, TotalPrice) VALUES (@id, @orderedthings, @price)";
            SqlCommand sqlCommand3 = new SqlCommand(query, SqlConnection);
            sqlCommand3.Parameters.AddWithValue("@id", id);
            sqlCommand3.Parameters.AddWithValue("@orderedthings", orderedthings);
            sqlCommand3.Parameters.AddWithValue("@price", Price);
            sqlCommand3.ExecuteNonQuery();
            SqlConnection.Close();
            Console.WriteLine("This is your order ID");
            SqlConnection.Open();
            SqlCommand sqlCommand4 = new SqlCommand();
            sqlCommand4.CommandType = System.Data.CommandType.Text;
            sqlCommand4.CommandText = $"SELECT OrderID FROM Orders WHERE ClientID = @id AND OrderedThings = @orderedthings AND TotalPrice = @price";
            sqlCommand4.Parameters.AddWithValue("@id", id);
            sqlCommand4.Parameters.AddWithValue("@orderedthings", orderedthings);
            sqlCommand4.Parameters.AddWithValue("@price", Price);
            sqlCommand4.Connection = SqlConnection;
            SqlDataReader reader = sqlCommand4.ExecuteReader();
            reader.Read();
            Console.WriteLine(reader.GetInt32(0));
            SqlConnection.Close();
        }
        public override string ToString()
        {
            return "Client ID: " + ClientID + " Name: " + Name + " Last name: " + LastName + " Phone number: " + ClientPhoneNumber + " Login: " + Login + " Client address: " + ClientAddress + " Password " + Password;
        }
        public IEnumerable<Pizza> GetPizza()
        {
            SqlConnection.Open();
            string query = $"SELECT * FROM Pizzas";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Pizzas.Add(new Pizza()
                {
                    Id = sqlDataReader.GetInt32(0),
                    PizzaName = sqlDataReader.GetString(1),
                    PizzaIngredients = sqlDataReader.GetString(2),
                    PizzaPrice = sqlDataReader.GetString(4),
                });
            }
            SqlConnection.Close();
            return Pizzas;
        }
        public IEnumerable<Sauce> GetSauce()
        {
            SqlConnection.Open();
            string query = $"SELECT * FROM Sauces";
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Sauces.Add(new Sauce()
                {
                    SauceId = sqlDataReader.GetInt32(0),
                    SauceName = sqlDataReader.GetString(1),
                    SaucePrice = sqlDataReader.GetString(2),
                });
            }
            SqlConnection.Close();
            return Sauces;
        }
        public void DisplayMenus(IEnumerable<object> objects)
        {
            foreach (var item in objects)
            {
                Console.WriteLine(item);
            }
        }
        public void CheckConfirmation()
        {
            string check = null;
            Console.WriteLine("Insert your order ID");
            int orderid = int.Parse(Console.ReadLine());
            SqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = $"SELECT ConfirmOrderID FROM ConfirmOrder WHERE OrderID = @ordid";
            sqlCommand.Connection = SqlConnection;
            sqlCommand.Parameters.AddWithValue("@ordid", orderid);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();
            check = sqlDataReader.GetString(0);
            SqlConnection.Close();
            if (check != null)
            {
                Console.WriteLine("We prepare your order");
            }
            else
            {
                Console.WriteLine("");
            }
        }
    }
}
