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
    //OrderHistory is the form that shows the user their order history
    public partial class OrderHistory : Form
    {
        //loggedInAccount is the Account that is currently logged in
        Account loggedInAccount;
        
        //Constructor for OrderHistory
        public OrderHistory(Account currentAccount)
        {
            loggedInAccount = currentAccount;
            InitializeComponent();
        }

        //If the user clicks back,return to the menu with the logged in account
        private void Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainMenu mm = new MainMenu(loggedInAccount);
            mm.ShowDialog();
            Close();
        }

        //This function loads the DataTable with the user's order history
        private void OrderHistory_Load(object sender, EventArgs e)
        {
            {
                DataTable orderTable = Orders.requestOrderHistory(loggedInAccount);
                dataGridView1.DataSource = orderTable;
            }
        }
    }
}
