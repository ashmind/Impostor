using System;
using System.Collections.Generic;
using System.Linq;

namespace Impostor.Settings {
    public class ImpostorRule {
        public ImpostorRule() {
            StatusCode = 200;
        }

        public string RequestUrlPath { get; set; }
        public int StatusCode { get; set; }
        public string ResponsePath { get; set; }
    }
}
