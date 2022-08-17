using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaConsoleApp
{
    public class Sauce
    {
        public int SauceId { get; set; }
        public string SauceName { get; set; }
        public string SaucePrice { get; set; }
        public List<Sauce> Sauces = new List<Sauce>();
        public override string ToString()
        {
            return SauceId + "\t" + SauceName + "\t" + String.Format("{0:0.00}", Convert.ToDouble(SaucePrice));
        }
    }
}
