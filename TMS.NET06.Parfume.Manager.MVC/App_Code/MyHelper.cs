using Microsoft.AspNetCore.Mvc.Rendering;
using TMS.NET06.Parfume.Manager.MVC.Data.Models;

namespace TMS.NET06.Parfume.Manager.MVC.App_Code
{
    public static class MyHelper
    {
        public static string GenderUrl(this IHtmlHelper html, Gender? gender)
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
                if (kv[0] == "gender")
                {
                    kv[1] = gender.ToString();
                    sp = string.Join("=", kv);
                    s_params[i] = sp;
                    ex = true;
                }
            }

            if (!(ex))
                res = path + queryString.Add("gender", gender.ToString()).ToString();
            else res = path + "?" + string.Join("&", s_params);


            return res;
        }

        public static string PageUrl(this IHtmlHelper html, int page)
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
                if (kv[0] == "page")
                {
                    kv[1] = page.ToString();
                    sp = string.Join("=", kv);
                    s_params[i] = sp;
                    ex = true;
                }
            }

            if (!(ex))
                res = path + queryString.Add("page", page.ToString()).ToString();
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

