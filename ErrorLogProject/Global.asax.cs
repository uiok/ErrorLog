using ErrorLogProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ErrorLogProject
{
    // 注意: 如需啟用 IIS6 或 IIS7 傳統模式的說明，
    // 請造訪 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Application_Error()
        {
            // 當前後台錯誤畫面有芬的時候能從這邊著手
            //HttpContext con = HttpContext.Current;
            //con.Request.Url.ToString().ToLower().Contains("backend")

            //取當前錯誤
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;

            var newRouteData = new RouteData();
            newRouteData.Values["controller"] = "Error";
            //等同 --> newRouteData.Values.Add("controller", "Error");  
            newRouteData.Values["action"] = "Index";
            newRouteData.Values["ErrorDetail"] = exception.Message;
      
            if (httpException != null)
            {
                var statusCode = httpException.GetHttpCode();
                if (statusCode == 404)
                {
                    newRouteData.Values["action"] = "PageNotFound";
                }
                if (statusCode == 500)
                {
                    newRouteData.Values["action"] = "ServerError";
                }
                
            }
         
            Response.Clear();
            //f取消用內建的錯誤
            Response.TrySkipIisCustomErrors = true;
            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), newRouteData));
        }
    }
}