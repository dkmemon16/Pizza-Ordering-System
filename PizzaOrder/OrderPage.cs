using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PizzaOrder
{
    //The OrderPage form allows the user to place an order
    public partial class OrderPage : Form
    {
        //The variables that are used in this form
        Orders currentOrder = new Orders();
        //The list of materials that makes up the cart
        List<Materials> Cart = new List<Materials>();
        //The cartTable holds the cart in a table form
        DataTable cartTable = new DataTable();
        Account loggedInAccount;
        //The order total
        double total = 0;
        //Whether the user clicked deliver or pickup
        Boolean delivery;
        //i is used as the index for the picture list
        private int i = 0;
        //The captions for the pictures
        private List<String> captionList = new List<string>();

        //Constructor for the OrderPage
        public OrderPage(Account currentAccount)
        {
            InitializeComponent();
            OrderPage1_Load();
            loggedInAccount = currentAccount;
            currentOrder.accountID = currentAccount.accountID;

        }

        //Loads the Menu when the page is opened
        private void OrderPage1_Load()
        {
            //The items in the menu are stored in the menuTable
            DataTable menuTable = new DataTable();

            //Connect to the database and make the query
            string sql = "SELECT item AS Item, cost AS Price FROM memonMaterials;";
            string connStr = "server=csdatabase.eku.edu;user=stu_csc340;database=csc340_db;port=3306;password=Colonels18;SslMode=none";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataAdapter myAdapter = new MySqlDataAdapter(cmd);
                myAdapter.Fill(menuTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();

            //Menu is an array of Items that are in the menu
            Materials[] Menu = new Materials[menuTable.Rows.Count];
            int i = 0;
            //for each item in the menuTable, add it to the Menu array
            foreach (DataRow row in menuTable.Rows)
            {
                Menu[i] = new Materials();
                Menu[i].item = row["Item"].ToString();
                Menu[i].cost = Double.Parse(row["Price"].ToString());
            }
            //Add the column names for the cart
            cartTable.Columns.Add("Item", typeof(string));
            cartTable.Columns.Add("Price", typeof(double));
            cartTable.Columns.Add("Amount in Cart", typeof(int));
            cartTable.Columns.Add("Subtotal", typeof(Decimal));

            //For each item in the cart, list their name, their cost, the amount in the order, and the subtotal for that item
            foreach (Materials currentItem in Cart)
            {
                cartTable.Rows.Add(currentItem.item, currentItem.cost.ToString("F"), currentItem.amountInOrder, (currentItem.cost * currentItem.amountInOrder));
            }


            //Set the DataSources for the dataGridViews
            dataGridView1.DataSource = menuTable;
            dataGridView2.DataSource = cartTable;

            //Add the Add to Cart and Remove buttons
            CreateUnboundButtonColumn();
            CreateUnboundButtonColumn2();

            //Display the initial image and initial caption
            pictureBox1.Image = imageList1.Images[i];
            captionList.Add("Coke");
            captionList.Add("Cheese Pizza");
            captionList.Add("Veggie Pizza");
            label1.Text = captionList[i];
        }

        //Crates the Add to Cart button column
        private void CreateUnboundButtonColumn()
        {
            // Initialize the button column.
            DataGridViewButtonColumn addToCartButtonColumn = new DataGridViewButtonColumn();
            //addToCartButtonColumn.Name = "Details";
            addToCartButtonColumn.Text = "Add to Cart";

            // Use the Text property for the button text for all cells rather
            // than using each cell's value as the text for its own button.
            addToCartButtonColumn.UseColumnTextForButtonValue = true;
            addToCartButtonColumn.Width = 2;
            // Add the button column to the control.
            dataGridView1.Columns.Insert(2, addToCartButtonColumn);
        }

        //Creates the Remove button column
        private void CreateUnboundButtonColumn2()
        {
            // Initialize the button column.
            DataGridViewButtonColumn removeFromCartButtonColumn = new DataGridViewButtonColumn();
            //addToCartButtonColumn.Name = "Details";
            removeFromCartButtonColumn.Text = "Remove";

            // Use the Text property for the button text for all cells rather
            // than using each cell's value as the text for its own button.
            removeFromCartButtonColumn.UseColumnTextForButtonValue = true;
            removeFromCartButtonColumn.Width = 8;
            // Add the button column to the control.
            dataGridView2.Columns.Insert(0, removeFromCartButtonColumn);
        }

        //When the Add to Cart button is clicked
        private void DataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                Console.WriteLine(e.RowIndex);
                //Capture the index of the item they clicked
                int indexOfItem = Cart.FindIndex(z => z.item == dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                //If the index is nonzero, increment to the amount of the item in the order
                if (indexOfItem >= 0)
                {
                    Cart.ElementAt(indexOfItem).amountInOrder += 1;
                }
                //otherwise add the cart item to the cart
                else
                {
                    Materials itemToAdd = new Materials
                    {
                        item = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(),
                        cost = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString())
                    };
                    Cart.Add(itemToAdd);
                }

                //Reset the cart and reload it to reflect the current items in the cart
                cartTable.Rows.Clear();
                total = 0;
                foreach (Materials currentItem in Cart)
                {
                    double subtotal = currentItem.cost * currentItem.amountInOrder;
                    cartTable.Rows.Add(currentItem.item, currentItem.cost.ToString("F"), currentItem.amountInOrder, subtotal.ToString("F"));
                    total += subtotal;
                }

                //Display the current total
                textBox1.Text = "Total: $" + total.ToString("F");
            }
        }

        //When Back is clicked, return to the Main Menu
        private void Button5_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            MainMenu mm = new MainMenu(loggedInAccount);
            mm.ShowDialog();
            Close();
        }

        //When the back button is clicked, return to the previous panel
        private void Button1_Click_1(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
        }

        //If the user clicked Pick Up
        private void Button4_Click(object sender, EventArgs e)
        {
            //Set Delivery to false move to the next panel, and turn the Pay with Cash option on
            delivery = false;
            panel4.Visible = true;
            panel2.Visible = false;
            radioButton1.Visible = true;

            //Set the location of the panel and the order total
            panel4.Location = new Point(12, 100);
            textBox2.Text = "Total: $" + total.ToString("F");


        }

        //If the user clicked Delivery
        private void Button2_Click_1(object sender, EventArgs e)
        {
            //Move to the next panel
            panel2.Visible = false;
            panel3.Visible = true;
            //Move the panel to this location
            panel3.Location = new Point(12, 100);

            //Load the user's information that is in their profile
            textBox7.Text = loggedInAccount.phone.ToString();
            textBox3.Text = loggedInAccount.address;
            textBox6.Text = loggedInAccount.creditCard.ToString();
            //Set the delivery variable to true
            delivery = true;

            //Disable the pay with cash option 
            radioButton1.Visible = false;
        }
    

    //If the user clicked back, go back to the second panel
    private void Button6_Click(object sender, EventArgs e)
    {
        panel2.Visible = true;
        panel3.Visible = false;
    }

    //If the user clicked continue
    private void Button7_Click(object sender, EventArgs e)
    {
        //Call the changeProfile function with the loggedInAccount
        Account.changeProfile(loggedInAccount);
        //Move to the next panel
        panel3.Visible = false;
        panel4.Visible = true;
        //Set the location of the next panel
        panel4.Location = new Point(12, 100);
        //Set the text of the text boxes to the order total and the credit card number
        textBox2.Text = "Total: $" + total.ToString("F");
        textBox6.Text = loggedInAccount.creditCard.ToString();
        //save the user's input as what they typed in the textboxes
        loggedInAccount.phone = Int64.Parse(textBox7.Text);
        loggedInAccount.address = textBox3.Text;

        //If delivery was not selected, turn the pay with cash button on
        if (!delivery)
        {
            radioButton1.Visible = true;
        }
        //Otherwise turn the pay with cash button off
        else
        {
            radioButton1.Visible = false;
        }
    }

    //If the user clicked back, go back to the second panel
    private void Button9_Click(object sender, EventArgs e)
    {
        panel4.Visible = false;
        panel2.Visible = true;

    }

    //When Confirm Payment is clicked
    private void Button8_Click(object sender, EventArgs e)
    {
        //If pay with card is selected, add it to the CreditCards table
        if (radioButton2.Checked)
            CreditCards.AddNewCard(Int64.Parse(textBox6.Text), loggedInAccount.accountID, textBox4.Text, textBox8.Text, int.Parse(textBox5.Text));

        //Then place the order and return to the menu
        Orders.placeOrder(loggedInAccount.accountID, Cart, total);
        MessageBox.Show("Order Received");
        MainMenu mainMenu = new MainMenu(loggedInAccount);
        this.Visible = false;
        mainMenu.ShowDialog();
        Close();
    }

    //If the user selects pay with card, show the text boxes for paying with card
    private void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        if (radioButton2.Checked)
        {
            textBox6.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            textBox8.Visible = true;
        }
    }

    //If the user selects pay with cash, hide the credit card text boxes
    private void RadioButton1_CheckedChanged(object sender, EventArgs e)
    {
        if (radioButton1.Checked)
        {
            textBox6.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox8.Visible = false;
        }
    }

    //If the user clicks remove from cart
    private void DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        var senderGrid = (DataGridView)sender;

        if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
        {
            Console.WriteLine(e.RowIndex);
            //Set the index of the item to the row of the button they clicked
            int indexOfItem = Cart.FindIndex(z => z.item == dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString());
            if (indexOfItem >= 0)
            {
                //if there is more than one of the item in the cart
                if (Cart.ElementAt(indexOfItem).amountInOrder > 1)
                {
                    //Decrement one from the amount in the cart
                    Cart.ElementAt(indexOfItem).amountInOrder -= 1;
                }
                //If there is exactly one of that item in the cart
                else if (Cart.ElementAt(indexOfItem).amountInOrder == 1)
                {
                    //decrement the amount in order to 0 and remove it from the cart list
                    Cart.ElementAt(indexOfItem).amountInOrder -= 1;
                    Cart.Remove(Cart.ElementAt(indexOfItem));
                }
                //Otherwise remove the item from the cart
                else
                {
                    Cart.Remove(Cart.ElementAt(indexOfItem));
                }
            }
            
            //Clear the car table, and reload with the current values
            cartTable.Rows.Clear();
            total = 0;
            foreach (Materials currentItem in Cart)
            {
                double subtotal = currentItem.cost * currentItem.amountInOrder;
                cartTable.Rows.Add(currentItem.item, currentItem.cost.ToString("F"), currentItem.amountInOrder, subtotal.ToString("F"));
                total += subtotal;
            }
            textBox1.Text = "Total: $" + total.ToString("F");

        }
    }

    //When they click the picture, cycle through the list of pictures
    private void pictureBox1_Click(object sender, EventArgs e)
    {
        label1.Text = captionList[i];
        pictureBox1.Image = imageList1.Images[i];
        if (i >= imageList1.Images.Count - 1)
        {
            i = 0;
        }
        else
        {
            i++;
        }
    }

    //When checkout is clicked, move to the next panel
    private void button3_Click(object sender, EventArgs e)
    {
        panel2.Visible = true;
        panel1.Visible = false;
    }
}
}
