using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaConsoleApp
{
    public class Orders
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int EmployeeID { get; set; }
        public string OrderedThings { get; set; }
        public double TotalPrice { get; set; }
        public List<Orders> orders = new List<Orders>();
    }
}
