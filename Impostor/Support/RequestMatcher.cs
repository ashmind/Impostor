using System;
using System.Collections.Generic;
using System.Linq;
using Impostor.Settings;
using JetBrains.Annotations;
using Microsoft.Owin;

namespace Impostor.Support {
    public class RequestMatcher {
        [CanBeNull]
        public Rule Match([NotNull] IOwinRequest request, [NotNull] IEnumerable<Rule> rules) {
            return rules.FirstOrDefault(r => IsMatch(request, r));
        }

        private bool IsMatch(IOwinRequest request, Rule rule) {
            return string.Equals(rule.RequestUrlPath, request.Uri.AbsolutePath, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
