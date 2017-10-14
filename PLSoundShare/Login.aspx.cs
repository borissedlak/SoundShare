using System;
using BOSoundShare;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace PLSoundShare
{
    public partial class UserLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Is called whenever the login form is submitted
        protected void ValidateUser(object sender, EventArgs e)
        {
            String nick = ((TextBox)Login1.FindControl("UserName")).Text;
            String passwd = ((TextBox)Login1.FindControl("Password")).Text;
            LoggedInUser result = Starter.loginUser(nick, passwd);
            if (result != null)
            {
                Session["loggedInUser"] = result;
                //Redirect if successful
                FormsAuthentication.RedirectFromLoginPage(nick, Login1.RememberMeSet);
                //((Label)Login1.FindControl("FailureText")).Text = "Success";
            }
            else
            {
                //((TextBox)Login1.FindControl("FailureText")).Text = "Failure";
            }
        }

        protected void CreateUser(object sender, EventArgs e)
        {
            //Because the text boxes are only pseudo elements created on runtime,
            //it is neccessary to retreive their values this way
            String nick = ((TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName")).Text;
            String email = ((TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email")).Text;
            String passwd = ((TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Password")).Text;

            LoggedInUser result = Starter.registerUser(nick, email, passwd);
            if (result != null)
            {
                Session["loggedInUser"] = result;
                FormsAuthentication.RedirectFromLoginPage(nick, Login1.RememberMeSet);
                //((Label)Login1.FindControl("FailureText")).Text = "Success";
            }
            else
            {
                //((TextBox)Login1.FindControl("FailureText")).Text = "Failure";
            }
        }
    }
}