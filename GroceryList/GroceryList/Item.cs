using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryList
{
    class Item
    {
        string name;
        int quantity;

        public Item(string n, int q)
        {
            this.Quantity = q;
            this.Name = n;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public override string ToString()
        {
            return quantity + " " + name;
        }
    }
}
