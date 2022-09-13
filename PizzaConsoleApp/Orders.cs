using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PizzaConsoleApp
{
    public class Orders
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string OrderedThings { get; set; }
        public double TotalPrice { get; set; }
        public List<Orders> orderlist = new List<Orders>();
        public SqlConnection sqlConnection;
        public override string ToString()
        {
            return "Order ID: " + OrderID + "\tCustomer ID: " + CustomerID + "\tEmployeeID: " + EmployeeID + "\tOrdered things: " + OrderedThings + "\tTotal Price: " + String.Format("{0:0.00}", Convert.ToDouble(TotalPrice));
        }
        public Orders()
        {
            sqlConnection = new SqlConnection(@"Data Source=DESKTOP-KOH3FDQ\SQLEXPRESS;Initial Catalog=PizzaApp;Integrated Security=true;");
        }
        public void getorders()
        {
            sqlConnection.Open();
            string query = $"SELECT * FROM Orders";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                orderlist.Add(new Orders()
                {
                    OrderID = sqlDataReader.GetInt32(0),
                    CustomerID = sqlDataReader.GetInt32(1),
                    EmployeeID = sqlDataReader.GetInt32(2),
                    OrderedThings = sqlDataReader.GetString(3).ToString(),
                    TotalPrice = sqlDataReader.GetDouble(4),
                });
            }
            sqlConnection.Close();
            foreach (var item in orderlist)
            {
                Console.WriteLine(item);
            }
        }
    }
}
