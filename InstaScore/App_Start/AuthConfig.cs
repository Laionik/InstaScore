using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using InstaScore.Models;

namespace InstaScore
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "462882370526476",
                appSecret: "938b0d194772c891891f82384201ffd4");

            OAuthWebSecurity.RegisterTwitterClient(
                 consumerKey: "eD3sdPM3A1dOon6KuSJVFDzb3",
                 consumerSecret: "RLo0Kqyh5yaypOVRwaFKnsftNEWxCfz7MbTa2lBRCDkknVPEdd");
            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
