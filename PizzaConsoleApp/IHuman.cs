using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaConsoleApp
{
    internal interface IHuman
    {       
        string Name { get; set; }
        string LastName { get; set; }
        string Login { get; set; }
        string Password { get; set; }
        void EditPassword(string newpassword, int id);
    }    
}
