using System.Reflection;
using System.Runtime.InteropServices;
using Impostor.Properties;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Impostor")]
[assembly: AssemblyDescription("Owin mock service middleware.")]
[assembly: AssemblyCompany("Andrey Shchekin")]
[assembly: AssemblyProduct("Impostor")]
[assembly: AssemblyCopyright("Copyright © Andrey Shchekin 2015")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("24d19c21-a530-4a30-9f03-5a5d3183714f")]

[assembly: AssemblyVersion(AssemblyInfo.VersionString)]
[assembly: AssemblyFileVersion(AssemblyInfo.VersionString)]
[assembly: AssemblyInformationalVersion(AssemblyInfo.VersionString)]

namespace Impostor.Properties {
    internal static class AssemblyInfo {
        public const string VersionString = "0.1.0";
    }
}