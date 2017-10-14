using System;
using BOSoundShare;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace PLSoundShare
{
    public partial class UserMusicOverview : System.Web.UI.Page
    {
        List<User> allUsers;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Checking whether the User session exists, otherwhise create a new request
            if (Session["allUsers"] == null)
            {
                allUsers = Starter.getAllUsers();
                Session["allUsers"] = allUsers;
            }
            else
            {
                allUsers = (List<User>)Session["allUsers"];
            }

            //This Error will be shown when a user manually changes the GET Parameter to an incorrect user
            if (!(allUsers.Any(s => s.username == Request["username"])))
            {
                UserMusicOverview_Error.Text = "Error, no Users with the specific name found!";
                return;
            }

            //Retreiving the correct user out of the user list
            User currentUser = allUsers.Find(x => x.username == Request["username"]);
            //Get the Audio Elements for the selected user
            //List<Audio> currentAudio = currentUser.uploadedFiles;

            //Binding all Audio Fils to the PL Table
            UserMusicOverview_Table.DataSource = currentUser.uploadedFiles;
            UserMusicOverview_Table.DataBind();
        }

        protected void UserMusicOverview_Table_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Checking whether a user is logged in in order to display the like/unlike buttons
            if (!this.Page.User.Identity.IsAuthenticated || Session["loggedInUser"] == null)
            {
                try
                {
                    e.Row.Cells[3].Style["display"] = "none";
                    e.Row.Cells[4].Style["display"] = "none";
                }
                catch (ArgumentOutOfRangeException a)
                {
                    //Happens if the User has no uploaded audio files
                }
            }
            else
            {
                //Only for Data Rows, we dont care about the header bar
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    List<User> allUsers = (List<User>)Session["allUsers"];

                    User audioUser = allUsers.Find(x => x.username == Request["username"]);
                    List<Audio> currentUserAudio = audioUser.uploadedFiles;

                    int index = e.Row.RowIndex;

                    //Check whether the audio file has the currently loggedInUser in the UsersLikes List
                    //Quite redundant, as all of this request are already made in the page load section.
                    int currentAudioID = (int)UserMusicOverview_Table.DataKeys[index].Values[0];
                    Audio audio = currentUserAudio.Find(x => x.ID == currentAudioID);

                    //Check whether the logged in user has already liked the audio element
                    LoggedInUser currentUser = (LoggedInUser)Session["loggedInUser"];
                    if ((currentUser.userHasLiked.Any(s => s.ID == currentAudioID)))
                        e.Row.Cells[3].Style["display"] = "none";
                    else
                        e.Row.Cells[4].Style["display"] = "none";
                }
            }
        }

        //This function is called on both like and unlike button press
        protected void UserMusicOverview_Table_Like(object sender, GridViewCommandEventArgs e)
        {
            int ID, result;
            try
            {
                //Specifying the row index of the liked/unliked audio
                int rowIndex = int.Parse(e.CommandArgument.ToString());
                ID = (int)((GridView)sender).DataKeys[rowIndex].Value;
            }
            catch (ArgumentOutOfRangeException arg)
            {
                return;
            }

            LoggedInUser currentUser = (LoggedInUser)Session["loggedInUser"];

            //Switch the command based on the requested command name
            if (e.CommandName.Equals("Like"))
                result = currentUser.likeAudio(ID);

            else if (e.CommandName.Equals("Unlike"))
                result = currentUser.unlikeAudio(ID);

            //return a new list because the current one has changed due to the like function
            List<Audio> currentAudio = allUsers.Find(x => x.username == Request["username"]).uploadedFiles;

            UserMusicOverview_Table.DataSource = currentAudio;
            UserMusicOverview_Table.DataBind();
        }
    }
}