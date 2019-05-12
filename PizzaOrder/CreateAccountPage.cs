using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.Serialization.Json;


namespace PizzaOrder
{
    //CreateAccountPage is the form that allows the user to create an account
    public partial class CreateAccountPage : Form
    {
        //Constructor for CreateAccountPage
        public CreateAccountPage()
        {
            InitializeComponent();
        }

        //When back is clicked, return to the main menu
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainMenu mm = new MainMenu();
            mm.ShowDialog();
        }
        //Variables used in the CreateAccountPage
        private string email;
        private string name;
        private string password;
        private string address;
        private Int64 phone;
        private Int64 creditCard;

        //When next is clicked, save the email and password the user entered, and switch to the next panel
        private void button1_Click(object sender, EventArgs e)
        {
            email = textBox1.Text;
            password = textBox5.Text;
            panel1.Visible = false;
            panel2.Visible = true;
        }

        //When save is clicked, store the information in the text boxes and attempt to create the account
        private void button4_Click(object sender, EventArgs e)
        {
            phone = Int64.Parse( textBox7.Text);
            address = textBox3.Text;
            creditCard = Int64.Parse(textBox6.Text);
            name = textBox2.Text;

            //If the account is created , inform the user and return to the menu
            if(Account.CreateAcct(email, name, password, address, phone, creditCard))
            {
                MessageBox.Show("Account created");
                this.Hide();
                MainMenu mm = new MainMenu();
                mm.ShowDialog();
                Close();
            }
            //If the account could not be created, inform the user
            else
            {
                MessageBox.Show("Account with that email already exists");
            }
        }

        //When back is clicked, return to the main menu
        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Visible = false; 
            panel1.Visible = true;
        }
    }


        

}
