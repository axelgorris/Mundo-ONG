﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using NGODirectory.Backend.DataObjects;
using NGODirectory.Backend.Models;
using Owin;
using System.Data.Entity.Migrations;

namespace NGODirectory.Backend
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            var mobileConfig = new MobileAppConfiguration();

            mobileConfig
                .AddTablesWithEntityFramework()
                .MapApiControllers()
                .ApplyTo(httpConfig);

            //httpConfig.MapHttpAttributeRoutes();

            // Automatic Code First Migrations
            var migrator = new DbMigrator(new Migrations.Configuration());
            migrator.Update();

            MobileAppSettingsDictionary settings = httpConfig.GetMobileAppSettingsProvider().GetMobileAppSettings();
            if (string.IsNullOrEmpty(settings.HostName))
            {
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    // This middleware is intended to be used locally for debugging. By default, HostName will only have a value when running in an App Service application.
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = httpConfig.GetAppServiceTokenHandler()
                });
            }
            
            app.UseWebApi(httpConfig);
        }
    }    
}

