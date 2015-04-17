using System;
using System.Collections.Generic;
using System.Linq;
using Impostor.Support;
using Owin;

namespace Impostor {
    public static class AppBuilderExtensions {
        public static void UseImpostor(this IAppBuilder app, ImpostorSettings settings) {
            app.Use<ImpostorMiddleware>(new RequestRecorder(), new RuleMatcher(), settings);
        }
    }
}
