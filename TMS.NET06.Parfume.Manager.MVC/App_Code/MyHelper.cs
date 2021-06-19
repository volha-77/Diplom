using Microsoft.AspNetCore.Mvc.Rendering;
using TMS.NET06.Parfume.Manager.MVC.Data.Models;

namespace TMS.NET06.Parfume.Manager.MVC.App_Code
{
    public static class MyHelper
    {
        public static string MakeUrl(this IHtmlHelper html, object paramValue, string paramName)
        {
            var queryString = html.ViewContext.HttpContext.Request.QueryString;

            string res;
            string path = html.ViewContext.HttpContext.Request.Path;
            var request = html.ViewContext.HttpContext.Request;

            string s_queryString = queryString.ToString().Replace("?", "");
            string[] s_params = s_queryString.Split("&");
            bool ex = false;
            for (int i = 0; i < s_params.Length; i++)
            {
                var sp = s_params[i];
                string[] kv = sp.Split("=");
                if (kv[0] == paramName)
                {
                    if(paramValue==null)
                        kv[1] = "null";
                    else
                    kv[1] = paramValue.ToString();
                    sp = string.Join("=", kv);
                    s_params[i] = sp;
                    ex = true;
                }
            }

            if (!(ex))
                if (paramValue==null)
                    res = path + queryString.Add(paramName, "null").ToString();
                else
                res = path + queryString.Add(paramName, paramValue.ToString()).ToString();
            else res = path + "?" + string.Join("&", s_params);


            return res;
        }

        public static string ClassMainMenu(this IHtmlHelper html, string name)
        {
            string res;
            string path = html.ViewContext.HttpContext.Request.Path;
         
            string path1 = "/";
            string path2 = "/home";
            if ((name == "home" && path.ToLower().Equals(path1))|| (name == "home" && path.ToLower().Equals(path2)))
                res = "active";
            else
            {
                if (path.ToLower().Contains(name)&&name!="home")
                    res = "active";
                else res = "";
            }

            return res;
        }
    }
}

