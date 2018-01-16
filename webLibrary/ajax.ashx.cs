using System.Data.SqlClient;
using System.Web;
using System.Data;
using System.Web.SessionState;

namespace webLibrary
{
    /// <summary>
    /// ajax 的摘要说明
    /// </summary>
    public class ajax : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Action = context.Request["Action"];         
            if (Action.Equals("checkName"))
            {
                string userName = context.Request["UserName"];
                string password = context.Request["Password"];
                if (string.IsNullOrEmpty(userName))
                {
                    context.Response.Write("noUser");//该用户名不能为空
                    return;
                }
                else
                {
                    int b = (int)SqlHelper.ExecuteScalar("select Count(*) from reader where IdentityCard=@userid", 
                        new SqlParameter("@userid", userName));//异步查找该用户名是否存在
                    if (b < 1)
                    {
                        context.Response.Write("no");//该用户不存在
                        return;
                    }
                    if (b >= 1)
                    {
                        context.Response.Write("yes");//该用户存在
                        return;

                    }
                }

            }else if (Action.Equals("denglu"))
            {
                string userName = context.Request["UserName"];
                string password = context.Request["Password"];
                if (string.IsNullOrEmpty(password))
                {
                    context.Response.Write("passwordnull");//密码是否为空
                    return;
                }
                else
                {
                    if (!context.Request["YanZheng"].Equals(context.Session["check"]))
                    {
                        //先检查验证码          
                        context.Response.Write("yanzhengma");
                        return;
                    }
                    else
                    {
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from reader where ReaderPwd=@password and IdentityCard=@userid"
                        , new SqlParameter("@password", password)
                        , new SqlParameter("@userid", userName));
                        if (dt.Rows.Count < 1)
                        {
                            context.Response.Write("password");//说明密码错误
                            return;
                        }
                        else
                        {
                            //这里面没有对Count>1做处理是因为没有必要
                            //这只是一个搜索的功能用户名和密码都可以重复
                            context.Response.Write("succeed");
                            context.Session["usrid"] = userName;
                            return;
                        }
                    }
                }
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