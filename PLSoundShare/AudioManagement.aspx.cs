using System;
using System.Collections.Generic;
using System.Configuration;
using BOSoundShare;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PLSoundShare
{
    public partial class AudioManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                //Only accessible when logged in
                FormsAuthentication.RedirectToLoginPage();
            }

            LoggedInUser currentUser = (LoggedInUser)Session["loggedInUser"];
            //List<Audio> currentAudio = currentUser.getAudioFiles();

            AudioManagement_Table.DataSource = currentUser.uploadedFiles;
            AudioManagement_Table.DataBind();

        }

        protected void AudioManagement_Table_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ID;
            try
            {
                //Get the ID of the row that has fired the event
                ID = (int)AudioManagement_Table.DataKeys[e.RowIndex].Value;
            }
            catch(ArgumentOutOfRangeException arg)
            {
                return;
            }

            //Cathing the currentUser out of the Session and deleting the Audio
            LoggedInUser currentUser = (LoggedInUser)Session["loggedInUser"];
            currentUser.deleteAudio(ID);

            //List<Audio> currentAudio = currentUser.getAudioFiles();

            AudioManagement_Table.DataSource = currentUser.uploadedFiles;
            AudioManagement_Table.DataBind();
        }

        //Fired when the File Upload Form is submitted
        protected void AudioManagement_FileUploadButton_Click(object sender, EventArgs e)
        {
            LoggedInUser currentUser = (LoggedInUser)Session["loggedInUser"];
            Debug.Write(AudioManagement_FileUpload.PostedFile.ContentLength);
            //Uploading the submitted file as posted file
            int result = currentUser.uploadAudio(AudioManagement_FileUpload.PostedFile, AudioManagement_Alias.Text, AudioManagement_Description.Text);

            if(result == 0)
            {
                //Bind new rows to the PL table
                //List<Audio> currentAudio = currentUser.getAudioFiles();

                AudioManagement_Table.DataSource = currentUser.uploadedFiles;
                AudioManagement_Table.DataBind();
            }
        }
    }
}