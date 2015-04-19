using System;
using System.Collections.Generic;
using System.Linq;
using Impostor.Logging;
using Impostor.Support;
using Owin;

namespace Impostor {
    public static class AppBuilderExtensions {
        public static void UseImpostor(this IAppBuilder app, ImpostorSettings settings) {
            app.UseImpostor(settings, new ImpostorDependencies());
        }

        public static void UseImpostor(this IAppBuilder app, ImpostorSettings settings, ImpostorDependencies dependencies) {
            app.Use<ImpostorMiddleware>(dependencies, settings);
        }
    }
}
