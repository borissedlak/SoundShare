using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSoundShare
{
    public class User
    {
        //Properties for managing the read/write permissions
        public String username { get; internal set; }
        public String email { get; internal set; }
        private List<Audio> _uploadedFiles;
        public List<Audio> uploadedFiles {
            get {
                return getAudioFiles();
            }
            internal set {
                _uploadedFiles = value;
            }
        }

        private List<Audio> _userHasLiked;
        public List<Audio> userHasLiked
        {
            get
            {
                return getLikedAudio();
            }
            internal set
            {
                _userHasLiked = value;
            }
        }

        //Internal Constructor, so the PL Developer cannot access it right away
        internal User(String username, String email)
        {
            this.username = username;
            this.email = email;
            this.uploadedFiles = null;
            this.userHasLiked = null;
        }

        //Return all Audio Files of a specific user
        public List<Audio> getAudioFiles()
        {
            SqlConnection con = Starter.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT [ID], [fileAlias], [description], [uploader] FROM [Audio] WHERE [uploader]=@nick", con);
            cmd.Parameters.AddWithValue("@nick", this.username);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Audio> userFiles = new List<Audio>();
            while (reader.Read())
            {
                Audio audio = new Audio((int)reader["ID"], (String)reader["fileAlias"], (String)reader["description"]);
                userFiles.Add(audio);
            }
            con.Close();
            return userFiles;
        }

        public List<Audio> getLikedAudio()
        {
            //The usersLiked variable includes the information which user has liked the audio file
            SqlConnection con = Starter.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT ual.AudioID AS ID, a.fileAlias, a.description FROM [User_Audio_like] ual INNER JOIN [Audio] a ON ual.AudioID = a.ID INNER JOIN [User] u ON ual.UserName = u.nickname WHERE u.nickname=@nickname", con);
            cmd.Parameters.AddWithValue("@nickname", this.username);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Audio> userLikedAudio = new List<Audio>();
            while (reader.Read())
            {
                Audio audio = new Audio((int)reader["ID"], (String)reader["fileAlias"], (String)reader["description"]);
                userLikedAudio.Add(audio);
            }
            con.Close();
            return userLikedAudio;
        }
    }
}
