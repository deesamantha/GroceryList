using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GroceryList
{
    public partial class Form1 : Form
    {
        List<Item> needs = new List<Item>();
        List<Item> haves = new List<Item>();


        public Form1()
        {
            InitializeComponent();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblErr.Text = "";

            string purpose = (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Have" || comboBox1.SelectedItem != null &&  comboBox1.SelectedItem.ToString() == "Need") ? comboBox1.SelectedItem.ToString() : "";
            int quantity = textBox1.Text != "qty." ? Convert.ToInt32(textBox1.Text) : 0;
            string item = textBox2.Text != "Items" ? textBox2.Text : "";

            if (purpose == "") { lblErr.Text += "Choose a Purpose!"; }
            if (quantity == 0) { lblErr.Text += "\r\nChoose a Quantity!"; }
            if (item == "") { lblErr.Text += "\r\nAdd an item!"; }


            if (purpose != "" && quantity != 0 && item != ""){
                if(purpose == "Have"){
                    haves.Add(new Item(item, quantity));
                }else if(purpose == "Need"){
                    needs.Add(new Item(item, quantity));
                }
            }

            Re();

        }

        private void btnToRight_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null) {
                int pos = listBox1.SelectedIndex;
                needs.Add(haves[pos]);
                haves.RemoveAt(pos);

                Re();
            }
        }

        private void btnToLeft_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                int pos = listBox2.SelectedIndex;
                haves.Add(needs[pos]);
                needs.RemoveAt(pos);

                Re();
            }
        }

        public void Re()
        {
            listBox1.DataSource = null;
            listBox2.DataSource = null;
            listBox1.DataSource = haves;
            listBox2.DataSource = needs;
            listBox1.SelectedItem = null;
            listBox2.SelectedItem = null;
            Invalidate();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                haves.RemoveAt(listBox1.SelectedIndex);
            }
            if (listBox2.SelectedItem != null)
            {
                needs.RemoveAt(listBox2.SelectedIndex);
            }
            Re();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XElement grocery = new XElement("Grocery");
            XElement haveXML = new XElement("Haves");
            XElement needXML = new XElement("Needs");
            foreach (Item item in haves)
            {
                XElement have = new XElement("Have");
                have.Add(new XElement("Name", item.Name));
                have.Add(new XElement("Quantity", item.Quantity));

                haveXML.Add(have);
            }
            foreach (Item item in needs)
            {
                XElement need = new XElement("Need");
                need.Add(new XElement("Name", item.Name));
                need.Add(new XElement("Quantity", item.Quantity));

                needXML.Add(need);
            }

            

            XDocument document = new XDocument();
            grocery.Add(haveXML);
            grocery.Add(needXML);
            document.Add(grocery);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                document.Save(saveFileDialog1.FileName, SaveOptions.DisableFormatting); //create items.xml file in bin folder
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                XDocument load = XDocument.Load(openFileDialog1.OpenFile());

                foreach (XElement h in load.Descendants("Have"))
                {
                    haves.Add(new Item(h.Element("Name").Value, Convert.ToInt32(h.Element("Quantity").Value)));
                }

                foreach (XElement n in load.Descendants("Need"))
                {
                    needs.Add(new Item(n.Element("Name").Value, Convert.ToInt32(n.Element("Quantity").Value)));
                }

                Re();
            }
        }
    }
}
