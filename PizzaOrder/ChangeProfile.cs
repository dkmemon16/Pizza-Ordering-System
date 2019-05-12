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
    //ChangeProfile is the form that allows the user to change their profile
    public partial class ChangeProfile : Form
    {
        //currentAccount is the current Account that is logged in
        Account currentAccount;
        
        //Constructor for ChangeProfile takes an input of a loggedInAccount, fills the textboxes with its information
        //then allows the user to change any of the fields and save it to the database
        public ChangeProfile(Account loggedInAccount)
        {
            //Set the current account to the loggedInAccount
            currentAccount = loggedInAccount;
            InitializeComponent();
            //Populate the textboxes with the information of the Account
            textBox1.Text = loggedInAccount.email;
            textBox7.Text = loggedInAccount.name;
            textBox5.Text = loggedInAccount.passwd;
            textBox3.Text = loggedInAccount.phone.ToString();
            textBox4.Text = loggedInAccount.address;
            textBox6.Text = loggedInAccount.creditCard.ToString();
        }
        //When back is clicked, return to the main menu with the current account unchanged
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainMenu mm = new MainMenu(currentAccount);
            mm.ShowDialog();
        }

        //When next is clicked, display the next panel
        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
       
        }

        //When confirm is clicked, change the current Account info to what is in the text boxes and call the changeProfile function
        private void button3_Click(object sender, EventArgs e)
        {
            //If the password the user entered matches the user's password, change the currentAccount's variables
            if(currentAccount.passwd == textBox2.Text)
            {
                currentAccount.email = textBox1.Text;
                currentAccount.passwd = textBox5.Text;
                currentAccount.phone = Int64.Parse(textBox3.Text);
                currentAccount.address = textBox4.Text;
                currentAccount.creditCard = Int64.Parse(textBox6.Text);

                //Call the changeProfile function. If it was successful, tell the user the profile was changed, and return to the main menu
                if (Account.changeProfile(currentAccount))
                {
                    MessageBox.Show("Profile changed");
                    this.Hide();
                    MainMenu mm = new MainMenu(currentAccount);
                    mm.ShowDialog();
                    Close();
                }
                //If it was not successful, inform the user 
                else
                {
                    MessageBox.Show("Account could not be changed");
                }
                
            }
            //If the password they entered is invalid, inform the user
            else
            {
                MessageBox.Show("Invalid Password");
            }

        }

        //When back is clicked, return to the previous panel so the user can edit the information
        private void button4_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }
    }
}
