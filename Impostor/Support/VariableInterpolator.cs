using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Impostor.Support {
    public class VariableInterpolator {
        public VariableInterpolator() {
            SystemFunctions = new Dictionary<string, Func<object>> {
                { "$now", () => DateTime.Now },
                { "$guid", () => Guid.NewGuid() }
            };
        }
        
        public IDictionary<string, Func<object>> SystemFunctions { get; private set; }

        public string Interpolate(string template, IReadOnlyDictionary<string, object> variables = null) {
            return Regex.Replace(template, "{([^}:]+)(?::([^}]+))?}", match => InterpolateMatch(match, variables));
        }

        private string InterpolateMatch(Match match, [CanBeNull] IReadOnlyDictionary<string, object> variables) {
            var name = match.Groups[1].Value;
            var format = match.Groups[2].Success ? match.Groups[2].Value : null;
            object value;
            if (name.StartsWith("$")) {
                Func<object> function;
                if (!SystemFunctions.TryGetValue(name, out function))
                    return match.Value;
                value = function();
            }
            else {
                if (variables == null || !variables.TryGetValue(name, out value))
                    return match.Value;
            }

            if (value == null)
                return "";

            var formattable = value as IFormattable;
            if (formattable != null)
                return formattable.ToString(format, CultureInfo.InvariantCulture);

            return value.ToString();
        }
    }
}
