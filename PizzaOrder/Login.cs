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
    //The form that allows the user to Login
    public partial class Login : Form
    {
        //
        //Constructor for Login
        public Login()
        {
            InitializeComponent();
        }

        //The variables for Login
        private string email;
        private string password;

        //When back is clicked, return to the main menu
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainMenu mm = new MainMenu();
            mm.ShowDialog();
        }

        //When next is clicked, verify the login. If successful, return to the menu with the logged in account
        private void button1_Click(object sender, EventArgs e)
        {
            //Store the text the user entered into email and password
            email = textBox1.Text;
            password = textBox5.Text;
            //Call the verifyLogin function and store the result into loggedInAccount
            Account loggedInAccount = Account.verifyLogin(email, password);
            //If an account with the email and password is found, the email will no longer be blank
            //In that case, the login has been completed, return to the main menu with the loggedInAccount
            if(loggedInAccount.email != "")
            {
                MessageBox.Show("Login Complete");
                this.Hide();
                MainMenu mm = new MainMenu(loggedInAccount);
                mm.ShowDialog();
                Close();
            }
            //If the account was not valid, inform the user
            else
            {
                MessageBox.Show("Invalid Login");
            }
        }
    }
}
