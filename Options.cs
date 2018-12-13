using CommandLine;

namespace UnitySelector
{
    public class Options
    {
        [Option('r', "root", Required = true, HelpText = "The root directory of all Unity installations.")]
        public string RootInstallationDirectory { get; set; }
        
        [Option('f', "format", Required = false, Default = "{0}", HelpText = "The format Unity installation folder names follow. {0} will be replaced with the actual version number.")]
        public string InstallationDirectoryFormat { get; set; }
        
        [Option('p', "path", Required = false, Default = "", HelpText = "The path at which to search for a project.")]
        public string ProjectPath { get; set; }
        
        [Option('d', "depth", Required = false, Default = 1, HelpText = "The depth until which to search for Unity projects. A depth of 0 is unlimited.")]
        public int ProjectSearchDepth { get; set; }
        
        [Option('o', "versionoverride", Required = false, Default = "", HelpText = "Overrides the project's set Unity version and uses the specified one instead.")]
        public string VersionOverride { get; set; }
        
        [Option('v', "verbose", Required = false, Default = false, HelpText = "Enables log output.")]
        public bool Verbose { get; set; }
    }
}