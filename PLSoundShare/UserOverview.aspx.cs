using System;
using BOSoundShare;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PLSoundShare
{
    public partial class UserOverview : System.Web.UI.Page
    {
        private List<User> allUsers;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Requests all Users from the starter object, on postback the session is recycled
            if (Session["allUsers"] != null)
            {
                allUsers = (List<User>)Session["allUsers"];
            }
            else
            {
                allUsers = Starter.getAllUsers();
                Session["allUsers"] = allUsers;
            }

            //Binding the User Information to the PL table
            UserOverview_Table.DataSource = allUsers;
            UserOverview_Table.DataBind();
        }
    }
}