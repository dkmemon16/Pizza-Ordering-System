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
    //The MainMenu form displays the main menu and allows the user to access the other forms
    public partial class MainMenu : Form
    {
        //Constructor for MainMenu if no account is logged in
        public MainMenu()
        {
            InitializeComponent();
        }
        //The variable for MainMenu
        Account currentAccount = new Account();

        //Constructor for MainMenu when someone is logged in
        public MainMenu(Account loggedInAccount)
        {
            currentAccount = loggedInAccount;
            InitializeComponent();
            //Set the textbox for  Logged in as: and the current account's email
            textBox1.Text = "Logged in as: " + currentAccount.email;
            button5.Visible = true;
            signInButton.Visible = false;
        }

        //If Create Account is clicked, and the user is not currently logged in to an account,
        // go to the CreateAccount form
        private void button1_Click(object sender, EventArgs e)
        {
            if (currentAccount.email == "")
            {
                this.Hide();
                CreateAccountPage cAP = new CreateAccountPage();
                cAP.ShowDialog();
                Close();
            }
            //If the user is already logged in, inform them
            else
            {
                MessageBox.Show("You are already logged in");
            }
        }
        
        //If Change Profile is clicked and the user is logged in, go to the ChangeProfile form
        private void button2_Click(object sender, EventArgs e)
        {
            //If the user is not logged in, inform them that they must be logged in to edit a profile
            if (currentAccount.email == String.Empty)
            {
                MessageBox.Show("You must be logged in to edit a profile");
            }
            //If the user is logged in, call a ChangeProfile form with the currentAccount
            else
            {
                this.Hide();
                ChangeProfile cP = new ChangeProfile(currentAccount);
                cP.ShowDialog();
                Close();
            }
        }

        //If Order Pizza is clicked, and the user is logged in, send them to the OrderPage form with the current account
        private void button3_Click(object sender, EventArgs e)
        {
            //If the user is not logged in, inform them that they must be logged in to make an order
            if (currentAccount.email == String.Empty)
            {
                MessageBox.Show("You must be logged in to order a pizza");
            }
            //If the user is logged in, send them to the OrderPage form with the current account
            else
            {
                this.Hide();
                OrderPage op = new OrderPage(currentAccount);
                op.ShowDialog();
                Close();
            }
        }

        //If Track Order is clicked and the user is logged in, send them to the OrderStatus form
        private void button4_Click(object sender, EventArgs e)
        {
            //If the user is not logged in, inform them that they must be logged in to make an order
            if (currentAccount.email == String.Empty)
            {
                MessageBox.Show("You must be logged in to track an order status");
            }
            //If the user is logged in, send them to the OrderStatus form with currentAccount
            else
            {
                this.Hide();
                OrderStatus os = new OrderStatus(currentAccount);
                os.ShowDialog();
                Close();
            }
        }
        
        //If the user clicks login, send them to the Login form
        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login lg = new Login();
            lg.ShowDialog();
            Close();
        }

        //If List Order History is clicked and the user is logged in, send them to the Order History page
        private void button6_Click(object sender, EventArgs e)
        {
            //If the user is not logged in, inform the user that they must be logged in to list their order history
            if (currentAccount.email == String.Empty)
            {
                MessageBox.Show("You must be logged in to list your order history");
            }
            //If the user is logged in, send them to the OrderHistory form with currentAccount
            else
            {
                this.Hide();
                OrderHistory oh = new OrderHistory(currentAccount);
                oh.ShowDialog();
                Close();

            }
        }

        //If Logout is clicked, call a new main menu form where a user is not logged in 
        private void Button5_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            MainMenu mainMenu = new MainMenu();
            mainMenu.ShowDialog();
            Close();
        }
    }
}
