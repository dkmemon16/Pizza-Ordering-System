using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.Remoting.Messaging;
using MySql.Data.MySqlClient;

namespace PizzaOrder
{
    //The Orders Page has the variables of the Orders table in the database, and allows the user to place orders,
    //request order status, and list their order history
    public class Orders
    {
        public int orderID;
        public int orderStatus;
        public int accountID;
        public string datePlaced;
        public List<Materials> itemsInOrder;
        
        //placeOrder takes an account ID, a list of materials that form the customer's cart, and a number that represents the order total
        public static void placeOrder(int a, List<Materials> cart, double total)
        { 
            //Connect to the database
            string connStr = "server=csdatabase.eku.edu;user=stu_csc340;database=csc340_db;port=3306;password=Colonels18;sslmode = none;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                //The first SQL command inserts the order into the database
                string sql = "INSERT INTO memonOrders(orderStatus, accountID, total) VALUES ("
                    + "'Preparing'" + ", " + a + ", " + total + ");";
                Console.WriteLine(sql);
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                //The next SQL command gets the orderID from the order table as it was created via autoincrement
                string sql3 = "SELECT LAST_INSERT_ID() AS orderID FROM memonOrders;";

                int lastOrderID = 0;
                MySqlCommand cmd3 = new MySql.Data.MySqlClient.MySqlCommand(sql3, conn);
                MySqlDataReader myReader = cmd3.ExecuteReader();
                if (myReader.Read())
                {
                    lastOrderID = int.Parse(myReader["orderID"].ToString());
                }
                myReader.Close();

                //For each item in the cart
                foreach (Materials m in cart)
                {
                    //The third SQL command inserts the Order, one of the items in the order, and the amount of that item in the order
                    string sql2 = "INSERT INTO memonOrderMaterials(orderID, item, amount) VALUES (" + lastOrderID+ ", '" + m.item + "', " + m.amountInOrder + ");";
                    Console.WriteLine(sql2);
                    MySql.Data.MySqlClient.MySqlCommand cmd2 = new MySql.Data.MySqlClient.MySqlCommand(sql2, conn);
                    cmd2.ExecuteNonQuery();

                    //This SQL command updates the materials table with the new quantity of the item left
                    string sql4 = "UPDATE memonMaterials SET quantity = quantity-" + m.amountInOrder +
                    " WHERE item = '" +m.item + "' ;";
                    Console.WriteLine(sql4);
                    MySql.Data.MySqlClient.MySqlCommand cmd4 = new MySql.Data.MySqlClient.MySqlCommand(sql4, conn);
                    cmd4.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            Console.WriteLine("Done.");
        }
        
        //requestOrderHistory takes the input of an Account that is logged in an returns a datatable of the order history
        public static DataTable requestOrderHistory(Account loggedInAccount)
        {
            //toReturnDataTable is the table that will be returned
            DataTable toReturnDataTable = new DataTable();

            //Connect to the database and make the query
            string sql = "SELECT orderID AS `Order Number`, datePlaced AS `Date Placed`, orderStatus as `Order Status`,"
                         + " total AS `Total Paid ($)` FROM memonOrders WHERE accountID ="
                         + loggedInAccount.accountID + " ;";
            string connStr = "server=csdatabase.eku.edu;user=stu_csc340;database=csc340_db;port=3306;password=Colonels18;SslMode=none";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                Console.WriteLine(sql);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataAdapter myAdapter = new MySqlDataAdapter(cmd);
                myAdapter.Fill(toReturnDataTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            return toReturnDataTable;
        }
    
        //requestOrderStatus takes the input of a loggedInAccount and returns a datatable of their current active orders
        public static DataTable requestOrderStatus (Account loggedInAccount)
        {
            //toReturnDataTable is the table that will be returned
            DataTable toReturnDataTable = new DataTable();
            //Connect to the database and make the query
            string sql = "SELECT orderID AS `Order Number`, datePlaced as `Date Placed`, orderStatus AS `Order Status` FROM memonOrders WHERE accountID ="
                         + loggedInAccount.accountID + " AND orderStatus != 'Completed' ;";
            string connStr = "server=csdatabase.eku.edu;user=stu_csc340;database=csc340_db;port=3306;password=Colonels18;SslMode=none";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                Console.WriteLine(sql);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataAdapter myAdapter = new MySqlDataAdapter(cmd);
                myAdapter.Fill(toReturnDataTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            return toReturnDataTable;
        }
    }

}
