using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Impostor.Settings {
    [PublicAPI]
    public class Rule {
        public string RequestUrlPath { get; set; }
        [CanBeNull] public string ResponsePath { get; set; }
        [CanBeNull] public RuleResponse Response { get; set; }
    }
}
