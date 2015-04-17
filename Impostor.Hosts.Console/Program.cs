using System;
using System.Collections.Generic;
using System.Linq;
using Impostor.Settings;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Logging;
using Owin;
using Serilog;
using SerilogWeb.Owin;
using ILogger = Serilog.ILogger;

namespace Impostor.Hosts.Console {
    using Console = System.Console;

    public static class Program {
        private static ILogger Logger { get; set; }

        public static int Main(string[] args) {
            try {
                Logger = new LoggerConfiguration()
                    .WriteTo.ColoredConsole(outputTemplate: "[{Timestamp:HH:mm}] {Message}{NewLine}{Exception}")
                    .CreateLogger();

                try {
                    SafeMain();
                }
                catch (Exception ex) {
                    Logger.Fatal(ex, "Impostor host failed.");
                    return ex.HResult;
                }
            }
            catch (Exception ex) {
                var consoleColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                try {
                    Console.Write(ex.ToString());
                }
                finally {
                    Console.ForegroundColor = consoleColor;
                }
                return ex.HResult;
            }

            return 0;
        }

        private static void SafeMain() {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole(outputTemplate: "[{Timestamp:HH:mm}] [{RequestId}] {Message}{NewLine}{Exception}")
                .MinimumLevel.Debug()
                .CreateLogger();

            var url = "http://localhost:3991";
            using (WebApp.Start(url, Configure)) {
                Logger.Information("Impostor started at {url}.", url);
                Console.WriteLine("Press [Enter] to stop.");
                Console.ReadLine();
            }
        }

        private static void Configure(IAppBuilder app) {
            app.SetLoggerFactory(new SerilogWeb.Owin.LoggerFactory(Log.Logger));
            // ReSharper disable once RedundantArgumentDefaultValue
            app.UseSerilogRequestContext("RequestId");
            app.UseImpostor(new ImpostorSettings {
                RecordDirectoryPath = ".",
                Rules = {
                    new ImpostorRule {
                        UrlPath = "/test"
                    }
                }
            });
        }
    }
}
