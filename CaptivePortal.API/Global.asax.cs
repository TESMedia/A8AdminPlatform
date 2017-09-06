using CaptivePortal.API.Models.A8AdminModel;
//using FluentValidation.WebApi;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CaptivePortal.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            //Database.SetInitializer<A8AdminDbContext>(new CreateDatabaseIfNotExists<A8AdminDbContext>());
            //AdminManagementDbOperation objAdminManagementDbOperation = new AdminManagementDbOperation();
            //objAdminManagementDbOperation.PerformDatabaseOperations();
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
            ILog logger = LogManager.GetLogger(typeof(ApiController));
            logger.Info("Application started successfully.");
            
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            LogException(exception);
            Application["error"] = exception;
            Response.Clear();
            Server.ClearError();

            var routeData = new RouteData();
            routeData.Values["controller"] = "Common";
            routeData.Values["action"] = "Error";
            Response.StatusCode = 500;
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;
            //if (!DataSettingsHelper.DatabaseIsInstalled())
            //    return;

            var httpException = exc as HttpException;
            //if (httpException != null && httpException.GetHttpCode() == 404)
            //    return;

            try
            {
                ILog logger = LogManager.GetLogger(typeof(ApiController));
                logger.Error(exc.Message);
            }
            catch (Exception)
            {

            }
        }
    }



}
