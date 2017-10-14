using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSoundShare
{
    public class Audio
    {
        public int ID { get; internal set; }
        public String fileAlias { get; internal set; }
        public String description { get; internal set; }
        //The uploader variable is not needed yet
        //public User uploader { get; internal set; }
        //public List<User> usersLiked { get; internal set; }

        internal Audio( int ID, String fileAlias, String description)
        {
            this.ID = ID;
            this.fileAlias = fileAlias;
            this.description = description;
            //this.uploader = null;
            //this.usersLiked = new List<User>();
        }
    }
}
