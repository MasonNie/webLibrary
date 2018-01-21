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
    /// Select 的摘要说明
    /// </summary>
    public class Select : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
             string IdentityCard = context.Session["IdentityCard"] + "";
            
            if (string.IsNullOrEmpty(IdentityCard))
            {
                context.Response.Redirect("templates/signIn.html");
            }
            /* string logo = context.Request["Logo"];//是从登陆页面进的还是点击搜索按钮进的后台的标记
             if (string.IsNullOrEmpty(logo))
             {
                 //说明是从登陆那进的，即第一次进入搜索页面
                 var data = new { Books = "" };
                 string html = CommonHelper.RenderHtml("book.html", data);
                 context.Response.Write(html);

             }*/
            else
            {
                string keyword = context.Request["search"];
                /*  int num = 1;//分页的初始标记，点击搜索按钮后要显示的第一页。
                  if (context.Request["Num"] != null)
                  {
                      num = Convert.ToInt32(context.Request["Num"]);
                      //这里是在点击别的页数时把相应的参数赋给num
                  }
                  */
                DataTable rdr = SqlHelper.ExecuteDataTable("select ReaderName,IdentityCard from reader where IdentityCard=@IdentityCard",
              new SqlParameter("@IdentityCard", IdentityCard));
              /*  if (string.IsNullOrEmpty(keyword))
                {
                    var data = new { Books = "", reader = rdr.Rows[0] };
                    string html = CommonHelper.RenderHtml("book.html", data);
                    context.Response.Write(html);
                }*/
                //else
                //{
                    DataTable table = SqlHelper.ExecuteDataTable(@"select * from 
                                   (select BookInfo.*,BookTypeName from bookinfo,BookType where 
                                   (bookInfo.BookTypeId=BookType.BookTypeId)) as A WHERE 
                                    BookId like @keyword or
                                    BookName like @keyword or
                                    isbs like @keyword or
                                    Author like @keyword or
                                    BookTypeName like @keyword or
                                    PinYinCode like @keyword or
                                    Translator like @keyword"
                                       , new SqlParameter("@keyWord", "%" + keyword + "%")
                                       );//分开从数据库中拿取数据

                    if (table.Rows.Count < 1)
                    {
                        //说明没有拿到一条数据，即没有搜到结果
                        context.Response.Write("<script type='text/javascript'>alert('未找到相关数据');window.location.href = 'so.ashx'</script>");
                        //context.Response.Redirect("so.ashx");
                    }
                    else
                    {
                        var data = new { Books = table.Rows, reader = rdr.Rows[0] };
                        string html = CommonHelper.RenderHtml("book.html", data);
                        context.Response.Write(html);
                    }
                //}


            }
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