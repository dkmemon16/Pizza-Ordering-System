using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaOrder
{
    //The Materials class holds variables of the Materials table in the database
    public class Materials
    {
        public string item;
        public double cost;
        public int quantityInStock;
        public int amountInOrder = 1;

    }
}
