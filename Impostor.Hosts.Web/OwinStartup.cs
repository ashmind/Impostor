using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using Impostor.Hosts.Web;
using Impostor.Settings.Yaml;
using JetBrains.Annotations;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Owin;
using Serilog;
using SerilogWeb.Owin;

[assembly: OwinStartup(typeof(OwinStartup))]

namespace Impostor.Hosts.Web {
    public class OwinStartup {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app) {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();

            try {
                app.SetLoggerFactory(new SerilogWeb.Owin.LoggerFactory(Log.Logger));
                // ReSharper disable once RedundantArgumentDefaultValue
                app.UseSerilogRequestContext("RequestId");

                var settingsPath = HostingEnvironment.MapPath("~/Settings.yaml");
                var settings = new YamlSettingsParser().Parse(File.ReadAllText(settingsPath));
                settings.RequestLogPath = MapPathIfRequired(settings.RequestLogPath);
                foreach (var rule in settings.Rules) {
                    rule.ResponsePath = MapPathIfRequired(rule.ResponsePath);
                    if (rule.Response != null)
                        rule.Response.ContentPath = MapPathIfRequired(rule.Response.ContentPath);
                }

                app.UseImpostor(settings);
                app.Use(UnmapedUrlHandler);
            }
            catch (Exception ex) {
                Log.Fatal(ex, "Failed to start Impostor web host: {0}.", ex.Message);
                throw;
            }
        }

#pragma warning disable 1998
        private static async Task UnmapedUrlHandler(IOwinContext context, Func<Task> next) {
#pragma warning restore 1998
            var response = context.Response;

            response.StatusCode = 404;
            response.ContentType = "text/plain";

            using (var writer = new StreamWriter(response.Body)) {
                await writer.WriteAsync("This URL is not mapped in an Impostor settings file.");
            }
        }

        private string MapPathIfRequired(string path) {
            return path != null
                 ? Regex.Replace(path, "^~", HostingEnvironment.MapPath("~"))
                 : null;
        }
    }
}