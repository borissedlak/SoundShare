using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace BOSoundShare
{
    //Derived Class from the base User, additional methods for managing audio content
    public class LoggedInUser : User
    {
        //The password may only be read internal and exclusively be changed within this class
        internal String password { get; private set; }

        internal LoggedInUser(String username, String password) : base(username, password)
        {}

        internal LoggedInUser(String username, String email, String password) : base(username, password)
        {
            this.password = password;
        }

        //uploadAudio receives a file, posted from the audio management form
        public int uploadAudio(HttpPostedFile file, String alias, String description)
        {
            if ((file == null) || (file.ContentLength <= 0))
            {
                //No file delivered
                return -1;
            }
            else if (file.ContentLength > 10485766)
            {
                //File size too large
                return -2;
            }
            else
            {
                //The comment below shows the possibility to use the original filename (instead of the alias)
                //String fn = System.IO.Path.GetFileName(AudioManagement_FileUpload.PostedFile.FileName);
                String fn;
                SqlConnection con = Starter.GetConnection();
                String insertCommand = "INSERT INTO Audio (fileAlias, description, uploader) " +
                        "VALUES (@fileAlias, @description, @uploader)";
                SqlCommand vSQLcommand = new SqlCommand(insertCommand, con);
                vSQLcommand.Parameters.AddWithValue("@fileAlias", alias);
                vSQLcommand.Parameters.AddWithValue("@description", description);
                vSQLcommand.Parameters.AddWithValue("@uploader", this.username);
                
                int insertSuccessfull = vSQLcommand.ExecuteNonQuery();

                //When the insert is successfull, the inserted ID is read into a variable
                if (insertSuccessfull > 0)
                {
                    String scalarCommand = "SELECT IDENT_CURRENT('Audio')";
                    vSQLcommand = new SqlCommand(scalarCommand, con);
                    var newAudioID = vSQLcommand.ExecuteScalar();
                    //patching the new filename out of the inserted id and the ".mp3" ending
                    fn = newAudioID.ToString() + ".mp3";
                    con.Close();
                }
                else
                {
                    //DB insert unsuccessful
                    con.Close();
                    return -3;
                }

                //FilePath for the AudioFile
                String SaveLocation = HttpContext.Current.Server.MapPath("AudioUpload") + "\\" + fn;
                try
                {
                    //Trying to move the file to the specified path
                    file.SaveAs(SaveLocation);
                    return 0;
                }
                catch (Exception ex)
                {
                    return -4;
                    //HttpContext.Current.Response.Write("Error: " + ex.Message);
                }
            }
        }

        //Deletes an AudioFile from db and removed it from the file system
        public int deleteAudio(int audioID)
        {
            //1. Deleting the row from the db
            SqlConnection con = Starter.GetConnection();
            String delteteCommand = "DELETE FROM Audio WHERE ID=@ID AND uploader=@nick";
            SqlCommand vSQLcommand = new SqlCommand(delteteCommand, con);
            vSQLcommand.Parameters.AddWithValue("@ID", audioID);
            vSQLcommand.Parameters.AddWithValue("@nick", this.username);

            int insertSuccessfull = vSQLcommand.ExecuteNonQuery();
            con.Close();

            //2. if successful remove the file from the file system
            if (insertSuccessfull > 0)
            {
                String SaveLocation = HttpContext.Current.Server.MapPath("AudioUpload") + "\\" + audioID + ".mp3";
                if ((System.IO.File.Exists(SaveLocation)))
                {
                    System.IO.File.Delete(SaveLocation);
                    return 0;
                }
                else
                {
                    return -2;
                }
            }
            return -1;
        }

        //Liking an audio file is no one-way progress, the like can also be reversed. 
        //Therefore it must be determined whether the audio file has already been liked or nor.
        //As this must already be examined for the UserMusicOverview Screen, it is no more neccessary to
        //create a toggleLike function and check inside if it has already been liked in order to use the correct method
        public int likeAudio(int audioID)
        {
            SqlConnection con = Starter.GetConnection();
            String insertCommand = "INSERT INTO User_Audio_Like (UserName, AudioID) " +
                        "VALUES (@nick, @audioID)";
            SqlCommand vSQLcommand = new SqlCommand(insertCommand, con);
            vSQLcommand.Parameters.AddWithValue("@nick", this.username);
            vSQLcommand.Parameters.AddWithValue("@audioID", audioID);

            int insertSuccessfull = 0;
            try
            {
                insertSuccessfull = vSQLcommand.ExecuteNonQuery();
            }
            catch(SqlException e) { }
            con.Close();

            if (insertSuccessfull > 0)
            {
                return 0;
            }

            return -1;
        }

        public int unlikeAudio(int audioID)
        {
            SqlConnection con = Starter.GetConnection();
            //Users can only delete their own files,
            //with including the username in the delete statement this is guaranteed
            String deleteCommand = "DELETE FROM User_Audio_Like WHERE UserName=@nick AND AudioID=@audioID";
            SqlCommand vSQLcommand = new SqlCommand(deleteCommand, con);
            vSQLcommand.Parameters.AddWithValue("@nick", this.username);
            vSQLcommand.Parameters.AddWithValue("@audioID", audioID);

            int deleteSuccessfull = 0;
            try
            {
                deleteSuccessfull = vSQLcommand.ExecuteNonQuery();
            }
            catch (SqlException e) { }
            con.Close();

            if (deleteSuccessfull > 0)
            {
                return 0;
            }

            return -1;
        }
    }
}
