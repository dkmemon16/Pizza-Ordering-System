using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PizzaOrder
{
    //The Account class has all the variables of the memonAccounts table in the database
    //It has the functions to create an account, update an account, and verify a login
    public class Account
    {
        public int accountID;
        public String name;
        public String passwd;
        //email is initialized to an empty string because the main menu uses this field to determine if someone is logged in 
        public String email = string.Empty;
        public String address; 
        public Int64 phone;
        public Int64 creditCard;

        //CreateAcct: takes the input of  all the variables of an account, makes the account, inserts it to the database
        //returns true if successful, false if not
        public static Boolean CreateAcct(String em, String n, String pw, String add, Int64 ph, Int64 cc)
        {
            //Create a new Account
            Account acc = new Account
            {
                email = em,
                name = n,
                passwd = pw,
                address = add,
                phone = ph,
                creditCard = cc
            };

            //Connect to the database and run the Insert command
            string connStr = "server=csdatabase.eku.edu;user=stu_csc340;database=csc340_db;port=3306;password=Colonels18;sslmode =none;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "INSERT INTO memonAccounts (email, name, passwd, address, phoneNum, creditCard) VALUES ('"
                    +acc.email + "', '" + acc.name + "', '" +acc.passwd + "', '" + acc.address + "', " + acc.phone + ", " + acc.creditCard + ");";
                Console.WriteLine(sql);
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);

                //If an Account was inserted, return true, if not, it returns false by default
                if (cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
            //Return false by default
            return false;
        }

        //verifyLogin takes the input of an email and password
        //if the account with that email and password exists, the rest of the Account's information is retrieved with the same query
        //The output is an Account with information of the account that has successfully logged in
        public static Account verifyLogin(String email, String password)
        {
            //Connect to the database and run the select statement with the input email and password
            string connStr = "server=csdatabase.eku.edu;user=stu_csc340;database=csc340_db;port=3306;password=Colonels18;sslmode = none;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            //toReturn will store the retrieved account details or remain empty if a matching account was not found
            Account toReturn = new Account();
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM memonAccounts WHERE email=\'" + email + "\' AND passwd = \'"
                    + password + "\';";

                MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                MySqlDataReader myReader = cmd.ExecuteReader();
                //If the query was successfully ran, store each of the column elements into toReturn 
                if (myReader.Read())
                {
                    toReturn.email = myReader["email"].ToString();
                    toReturn.name = myReader["name"].ToString();
                    toReturn.passwd = myReader["passwd"].ToString();
                    toReturn.phone = Int64.Parse(myReader["phoneNum"].ToString());
                    toReturn.address = myReader["address"].ToString();
                    toReturn.creditCard = Int64.Parse(myReader["creditCard"].ToString());
                    toReturn.accountID = int.Parse(myReader["accountID"].ToString());
                }
               
                myReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            conn.Close();
            Console.WriteLine("Done.");
            return toReturn;
        }

        //changeProfile takes the input of an Account that needs to be changed
        //It attempts to update the Account in the database
        //Outputs true if successful, false otherwise
        public static Boolean changeProfile(Account currentAccount)
        {
            string connStr = "server=csdatabase.eku.edu;user=stu_csc340;database=csc340_db;port=3306;password=Colonels18;sslmode = none;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "UPDATE memonAccounts SET email = '" + currentAccount.email+ "', name = '" + currentAccount.name +
                    "', passwd = '" + currentAccount.passwd + "', phoneNum = " + currentAccount.phone + 
                    " , address = '" + currentAccount.address + "' , creditCard = " + currentAccount.creditCard + 
                    " WHERE accountID = " + currentAccount.accountID + ";";
                Console.WriteLine(sql);    
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);


                if (cmd.ExecuteNonQuery() > 0)
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
            return false;
        }

    }
}



