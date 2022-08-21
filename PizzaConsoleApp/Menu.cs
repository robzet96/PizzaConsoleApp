using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace PizzaConsoleApp
{
    public class Menu
    {
        SqlConnection SqlConnection;
        public Menu()
        {
            SqlConnection = new SqlConnection(@"Data Source=DESKTOP-KOH3FDQ\SQLEXPRESS;Initial Catalog=PizzaApp;Integrated Security=true;");
        }
        Client client1 = new Client();
        Employee employee1 = new Employee();
        Sauce Sauce = new Sauce();
        Pizza Pizza = new Pizza();
        Regex Regex;
        private string startoptions()
        {
            Console.WriteLine("1. Register\n" +
                "2. Login\n" +
                "3. Continue as guest");
            string choice = Console.ReadLine();
            return choice;
        }
        private int loggingin(string login, string password)
        {
            int id;
            Regex = new Regex("([A-Z]{2}[0-9]{3})");
            SqlConnection.Open();
            string query;
            if (Regex.IsMatch(login))
            {
                query = $"SELECT EmployeeID FROM Employees WHERE EmployeeLogin = {login} AND EmployeePassword = {password} ";
            }
            else
            {
                query = $"SELECT ClientID FROM Clients WHERE Email = {login} AND ClientPassword = {password}";
            }
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();
            id = sqlDataReader.GetInt32(0);
            SqlConnection.Close();
            return id;
        }
        private void mainmenu(string login, int logginin)
        {
            Regex = new Regex("([A-Z]{2}[0-9]{3})");
            if (Regex.IsMatch(login))
            {
                int empid = logginin;
                string choice = Console.ReadLine();
                Console.WriteLine("Welcome to Employee Menu. There are your options:\n" +
                    "1. Edit password\n" +
                    "2. Add pizza to menu\n" +
                    "3. Add sauce to menu\n" +
                    "4. Delete pizza\n" +
                    "5. Delete sauce\n" +
                    "6. Confirm client registration\n" +
                    "7. Confirm order\n" +
                    "8. Display sauce menu\n" +
                    "9. Display pizza menu");

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Insert your old password: ");
                        string oldpass = Console.ReadLine();
                        Console.WriteLine("Insert your new password");
                        string newpass = Console.ReadLine();    
                        employee1.EditPassword(newpass,oldpass);
                        break;
                        case "2":
                        Console.WriteLine("Insert pizza name: ");
                        string pizzaname = Console.ReadLine();
                        Console.WriteLine("Insert pizza ingredients: ");
                        string pizzaingredients = Console.ReadLine();
                        Console.WriteLine("Insert pizza price (for small pizza)");
                        string pizzaprice = Console.ReadLine();
                        employee1.AddPizza(pizzaname,pizzaingredients,pizzaprice);
                        break;
                        case"3":
                        Console.WriteLine("Insert sauce name: ");
                        string saucename = Console.ReadLine();
                        Console.WriteLine("Insert sauce price: ");
                        string sauceprice = Console.ReadLine();
                        employee1.AddSauce(saucename, sauceprice);
                        break;
                        case"4":
                        Console.WriteLine("Insert pizza name which you want delete");
                        string pizzadelete = Console.ReadLine();
                        employee1.DeletePizza(pizzadelete);
                        break;
                        case"5":
                        Console.WriteLine("Insert sauce name which you want delete");
                        string deletesauce = Console.ReadLine();
                        employee1.DeleteSauce(deletesauce);
                        break;
                        case"6":
                        employee1.ConfirmRegistration();
                        break;
                    case "7":
                        employee1.ConfirmOrder(empid);
                        break;
                    case "8":
                        employee1.DisplayMenus(Sauce.Sauces);
                        break;
                    case "9":
                        employee1.DisplayMenus(Pizza.Pizzas);
                        break;
                    default:
                        break;
                }
            }
            else
            {

            }
        }
        public void Login(string choice)
        {
            switch (choice)
            {
                case "1":                    
                    client1.Register();
                    break;
                    case "2":
                    Console.WriteLine("Login: ");
                    string login = Console.ReadLine();
                    Console.WriteLine("Password: ");
                    string password = Console.ReadLine();
                    mainmenu(login,loggingin(login,password));
                    break;
                default:
                    break;
            }
        }
    }
}
