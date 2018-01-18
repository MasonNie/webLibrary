using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace webLibrary
{
    /// <summary>
    /// test 的摘要说明
    /// </summary>
    public class test : IHttpHandler ,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string IdentityCard = context.Session["IdentityCard"] + "";
            if (string.IsNullOrEmpty(IdentityCard))
            {
                context.Response.Redirect("templates/denglu.html");
            }
            DataTable dt1 = SqlHelper.ExecuteDataTable("select ReaderName,IdentityCard from reader where IdentityCard=@IdentityCard",
                new SqlParameter("@IdentityCard", IdentityCard));
            DataTable dt2 = SqlHelper.ExecuteDataTable(@"SELECT BookName,Author,BorrowTime,ReturnTime,RenewCount, IdentityCard
                                            FROM BookInfo,BorrowReturn,Reader 
                                            WHERE BorrowReturn.BookId=BookInfo.BookId AND BorrowReturn.ReaderId=Reader.ReaderId AND IdentityCard=@IdentityCard",
                                            new SqlParameter("@IdentityCard", IdentityCard));
            var data = new {reader=dt1.Rows[0] ,borrows=dt2.Rows};
            string html = CommonHelper.RenderHtml("details.html" ,data);
            context.Response.Write(html);
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