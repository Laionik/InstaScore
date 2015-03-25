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

             OAuthWebSecurity.RegisterTwitterClient(
                 consumerKey: "eD3sdPM3A1dOon6KuSJVFDzb3",
                 consumerSecret: "RLo0Kqyh5yaypOVRwaFKnsftNEWxCfz7MbTa2lBRCDkknVPEdd");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "346506492212997",
                appSecret: "c1c2b010a291a8ba3f5c5c4983aef339");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
