using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace webLibrary
{
    /// <summary>
    /// signout 的摘要说明
    /// </summary>
    public class signout : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Session.Clear();
            context.Session.Abandon();
            context.Response.Redirect("templates/signIn.html");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}