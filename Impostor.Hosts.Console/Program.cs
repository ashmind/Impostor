using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin.Hosting;

namespace Impostor.Hosts.Console {
    using Console = System.Console;

    public static class Program {
        public static void Main(string[] args) {
            var url = "http://localhost:3991";
            using (WebApp.Start<Startup>(url)) {
                Console.WriteLine("Impostor started at http://localhost:3991.");
                Console.WriteLine("Press [Enter] to stop.");
                Console.ReadLine();
            }
        }
    }
}
