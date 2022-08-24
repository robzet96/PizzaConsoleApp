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
        Admin admin = new Admin();
        Client client1 = new Client();
        Employee employee1 = new Employee();
        Sauce Sauce = new Sauce();
        Pizza Pizza = new Pizza();
        Regex Regex;
        private string startoptions()
        {
            Console.WriteLine("1. Register\n" +
                "2. Login\n" +
                "3. Contact information\n" +
                "Q. To quit app");
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
                query = $"SELECT EmployeeID FROM Employees WHERE EmployeeLogin = @login AND EmployeePassword = @password ";
            }
            else
            {
                query = $"SELECT ClientID FROM Clients WHERE ClientEmail = @login AND ClientPassword = @password";
            }
            SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
            sqlCommand.Parameters.AddWithValue("@login", login);
            sqlCommand.Parameters.AddWithValue("@password", password);
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
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Insert your new password");
                        string newpass = Console.ReadLine();
                        Console.WriteLine("Insert your new password once again to confirm the change");
                        string newpass1 = Console.ReadLine();
                        if (newpass == newpass1)
                        {
                            employee1.EditPassword(newpass, empid);
                            Console.WriteLine($"Your password has been changed to {newpass}");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong, your password has NOT been changed.");
                        }
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
                        employee1.DisplayMenus(employee1.GetSauce());
                        break;
                    case "9":
                        employee1.DisplayMenus(employee1.GetPizza());
                        break;
                    default:
                        Console.WriteLine("No option like this.");
                        break;
                }
            }
            else
            {
                int cliid = logginin;
                Console.WriteLine("Welcome to PizzaApp. There are your options:\n" +
                    "1. Edit password\n" +
                    "2. Order pizza\n" +
                    "3. Check confirmation");
                string clichoice = Console.ReadLine();
                switch (clichoice)
                {
                    case "1":
                        Console.WriteLine("Insert your new password: ");
                        string clinewpassword = Console.ReadLine();
                        Console.WriteLine("Insert your new password one more time to confirm: ");
                        string confirmclinewpassowrd = Console.ReadLine();
                        if (clinewpassword == confirmclinewpassowrd)
                        {
                            client1.EditPassword(clinewpassword, cliid);
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong your password has NOT been changed.");
                        }
                        break;
                        case"2":
                        client1.Order(cliid);
                        break;
                    case "3":
                        client1.CheckConfirmation();
                        break;
                    default:
                        Console.WriteLine("No option like this.");
                        break;
                }
            }
        }
        public void Login()
        {
            switch (startoptions().ToLower())
            {
                case "1":                    
                    client1.Register();
                    break;
                    case "2":
                    Console.WriteLine("Login: ");
                    string login = Console.ReadLine();
                    Console.WriteLine("Password: ");
                    string password = Console.ReadLine();
                    if (login != "Admin" & password != "admin1234")
                    {
                        try
                        {
                            mainmenu(login, loggingin(login, password));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Login or password are incorrect.");
                            Login();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Admin panel options:\n" +
                            "1. Add employee\n" +
                            "2. Delete employee");
                        string choice = Console.ReadLine();
                        switch (choice)
                        {
                            case "1":
                                Console.WriteLine("Insert new employee name: ");
                                string name = Console.ReadLine();
                                Console.WriteLine("Insert new employee last name: ");
                                string lastname = Console.ReadLine();
                                admin.AddEmployee(name,lastname);
                                break;
                            case "2":
                                admin.DisplayMenus(admin.getemployees());
                                Console.WriteLine("Insert employee to delete id: ");
                                string empid = Console.ReadLine();
                                try
                                {
                                    admin.DeleteEmployee(empid);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("No employee with this ID");
                                }
                                break;
                            default:
                                Console.WriteLine("No option like this in admin panel.");
                                break;
                        }                        
                        Console.WriteLine();
                        Console.WriteLine("If you wish to see employee options you have to log in as one of them. To do that insert his login: ");
                        string emplogin = Console.ReadLine();
                        Console.WriteLine("Password: ");
                        string emppassowrd = Console.ReadLine();
                        try
                        {
                            mainmenu(emplogin, loggingin(emplogin, emppassowrd));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("No login or password like this.");
                        }
                    }
                    break;
                    case "3":
                    Console.WriteLine("Phone number: 555-444-333\n" +
                        "Email: pizzaapp@gmail.com" +
                        "Address: Katowice 44-555, Pocztowa 12");
                    break;
                case "q":
                    return;
                default:
                    Console.WriteLine("No option like this.");
                    break;
            }
        }
    }
}
