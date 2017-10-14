using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using BOSoundShare;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSoundShare
{
    //The static starter class provides all the methods required to generate the first application screen,
    //whereas making it possible to generate object instances of internal classes for further screens
    public static class Starter
    {
        public static List<User> getAllUsers()
        {
            //Creating the SQLCOnnection for the DB Statement
            SqlConnection con = Starter.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT [nickname], [email] FROM [User]", con);
            SqlDataReader reader = cmd.ExecuteReader();
            List<User> allUsers = new List<User>();
            while (reader.Read())
            {
                //The User Class constructor is internal, allowing the starter class to access the constructor 
                User user = new User((String)reader["nickname"], (String)reader["email"]);
                allUsers.Add(user);
            }
            //Closing the connection after usage is VERY(!!) important,
            //otherwise it becomes impossible to open new connections elsewhere
            con.Close();
            return allUsers;
        }

        //The loginUser Method looks up in the DB table for corresponding User Credentials
        //The parameter password is still text in clear
        public static LoggedInUser loginUser(String nickname, String password)
        {
            SqlConnection con = Starter.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT [nickname], [email], [password] FROM [User] WHERE [nickname]=@nick AND [password]=@passwd", con);
            cmd.Parameters.AddWithValue("@nick", nickname);
            //The hashed password is placed in the select statement
            cmd.Parameters.AddWithValue("@passwd", Encrypt.Pwd_Encode(password));
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                String email = (String)reader["email"];
                con.Close();
                return new LoggedInUser(nickname, email, password);
            }
            con.Close();
            return null;
        }

        //The registerUser method creates a new row in the user table
        //If the username already exists, the insert will fail and return null
        public static LoggedInUser registerUser(String username, String email, String password)
        {
            SqlConnection con = Starter.GetConnection();
            String insertCommand = "INSERT INTO [User] (nickname, email, password) " +
                        "VALUES (@nick, @email, @passwd)";
            SqlCommand vSQLcommand = new SqlCommand(insertCommand, con);
            vSQLcommand.Parameters.AddWithValue("@nick", username);
            vSQLcommand.Parameters.AddWithValue("@email", email);
            vSQLcommand.Parameters.AddWithValue("@passwd", Encrypt.Pwd_Encode(password));
            int insertSuccessfull = vSQLcommand.ExecuteNonQuery();
            con.Close();

            if (insertSuccessfull > 0)
            {
                return new LoggedInUser(username, email, password);
            }
            else
            {
                return null;
            }
                
        }

        //Shortcut for opening a SQL Connection which still has to be closed seperately after usage
        internal static SqlConnection GetConnection()
        {
            List<string> dirs = new List<string>(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory).Split('\\'));
            dirs.RemoveAt(dirs.Count - 1);
            string conString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + String.Join(@"\", dirs) + @"\DL\SoundShareDB.mdf;Integrated Security=True;Connect Timeout=5";

            SqlConnection con = new SqlConnection(conString);
            con.Open();
            return con;
        }
    }
}
