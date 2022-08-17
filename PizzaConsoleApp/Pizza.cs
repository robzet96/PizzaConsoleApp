using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PizzaConsoleApp
{
    public class Pizza
    {
        public int Id { get; set; }
        public string PizzaName { get; set; }
        public string PizzaIngredients { get; set; }
        public string PizzaPrice { get; set; }
        public List<Pizza> Pizzas = new List<Pizza>();
        public override string ToString()
        {
            return Id + ".\t" + PizzaName + "\t" + PizzaIngredients + "\t" + PizzaPrice + @"\" + (Convert.ToDouble(PizzaPrice) * 1.1) + @"\" + (Convert.ToDouble(PizzaPrice) * 1.2);
        }
    }
}
