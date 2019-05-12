using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaOrder
{
    //CreditCards class: contains the variables and functions for Credit Cards
    class CreditCards
    {
        //Variables for the CreditCards class
        Int64 number;
        int accountID;
        String billAddress;
        String expDate;
        int securityCode;

        //AddNewCard function adds a new credit card to the database with an input of the Credit Card details
        public static Boolean AddNewCard(Int64 n, int a, String bA, String e, int sc)
        {
            //Create the temporary card
            CreditCards tempCard = new CreditCards {
                number = n,
                accountID = a,
                billAddress = bA,
                expDate = e,
                securityCode = sc
            };
            //Connect to the database and attempt to insert the credit card, return true if it was inserted, false otherwise
            string connStr = "server=csdatabase.eku.edu;user=stu_csc340;database=csc340_db;port=3306;password=Colonels18;sslmode = none;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "INSERT INTO memonCreditCards VALUES ("
                    + tempCard.number + ", " + tempCard.accountID + ", '" + tempCard.billAddress + "', '" + 
                    tempCard.expDate + "', " + tempCard.securityCode + ");";
                Console.WriteLine(sql);
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
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
            return false;
        }
    }

}
