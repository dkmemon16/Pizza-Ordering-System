using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PizzaOrder
{
    //The OrderStatus form allows the user to track their active orders
    public partial class OrderStatus : Form
    {
        //loggedInAccount is the account that is logged into this page
        Account loggedInAccount;
        //Constructor for OrderStatus, takes an account that is logged in
        public OrderStatus(Account currentAccount)
        {
            loggedInAccount = currentAccount;
            InitializeComponent();
            OrderStatus_Load();

        }
        
        //If back is clicked, return to the main menu with the logged in account
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainMenu mm = new MainMenu(loggedInAccount);
            mm.ShowDialog();
            Close();
        }

        //This function loads the page of with the Active Orders
        private void OrderStatus_Load()
        {
            DataTable orderTable = Orders.requestOrderStatus(loggedInAccount);
            dataGridView1.DataSource = orderTable;
        }
    }
}
