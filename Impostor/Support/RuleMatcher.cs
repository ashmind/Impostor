using System;
using System.Collections.Generic;
using System.Linq;
using Impostor.Logging;
using Impostor.Settings;
using JetBrains.Annotations;
using Microsoft.Owin;

namespace Impostor.Support {
    public class RuleMatcher {
        public RuleMatcher() {
            
        }

        [CanBeNull]
        public ImpostorRule Match([NotNull] IOwinRequest request, [NotNull] IEnumerable<ImpostorRule> rules) {
            return rules.FirstOrDefault(r => IsMatch(request, r));
        }

        private bool IsMatch(IOwinRequest request, ImpostorRule rule) {
            return string.Equals(rule.RequestUrlPath, request.Uri.AbsolutePath, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
