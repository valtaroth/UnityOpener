using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace UnitySelector
{
    public class Controller
    {
        private readonly Options m_options;

        public Controller(Options options)
        {
            m_options = options;
        }
        
        public ExitCode Run(string directory)
        {
            ExitCode result = LocateProject(directory, out string project);
            if (result != ExitCode.Success)
            {
                return result;
            }

            result = LocateUnity(project, out string unity);
            if (result != ExitCode.Success)
            {
                return result;
            }

            return OpenUnity(unity, project);
        }
        
        private ExitCode LocateProject(string directory, out string path)
        {
            Log($"Locating project in directory '{directory}'");

            path = null;
            
            if (string.IsNullOrEmpty(directory))
            {
                return ExitCode.MissingProjectPath;
            }
            
            string[] projects = GetUnityProjects(directory);
            if (projects.Length == 0)
            {
                return ExitCode.NoUnityProjectFound;
            }
            if (projects.Length > 1)
            {
                return ExitCode.MultipleUnityProjectsFound;
            }

            path = projects[0];
            return ExitCode.Success;
        }

        private string[] GetUnityProjects(string directory)
        {
            List<string> projects = new List<string>();
            List<string> directories = new List<string>();
            List<string> nextDirectories = new List<string>();
            
            directories.Add(directory);

            int depth = 0;
            while ((m_options.ProjectSearchDepth > 0 && depth < m_options.ProjectSearchDepth) && directories.Count > 0)
            {
                for (int j = 0; j < directories.Count; j++)
                {
                    string[] subdirectories = Directory.GetDirectories(directories[j], "*", SearchOption.TopDirectoryOnly);
                    if (IsUnityProject(subdirectories))
                    {
                        Log($"Found Unity project '{directories[j]}'");
                        projects.Add(directories[j]);
                    }
                    nextDirectories.AddRange(subdirectories);
                }

                if (projects.Count > 0)
                {
                    break;
                }

                directories.Clear();
                directories.AddRange(nextDirectories);
                nextDirectories.Clear();
                depth++;
            }
            
            return projects.ToArray();
        }

        private bool IsUnityProject(string[] subdirectories)
        {
            return subdirectories.Select(Path.GetFileName).Count(dir => dir == "Assets" || dir == "ProjectSettings") == 2;
        }

        private ExitCode LocateUnity(string project, out string path)
        {
            Log($"Locating Unity for project '{project}'");
            
            path = null;
            
            ExitCode result = FetchVersion(project, out string version);
            if (result != ExitCode.Success)
            {
                return result;
            }
            
            Log($"Fetched version '{version}', trying to locate installation.");

            path = Path.Combine(m_options.RootInstallationDirectory, string.Format(m_options.InstallationDirectoryFormat, version), "Editor", "Unity.exe");
            if (!File.Exists(path))
            {
                return ExitCode.UnityInstallationMissing;
            }

            return ExitCode.Success;
        }
        
        private ExitCode FetchVersion(string project, out string version)
        {
            version = null;
            if (!string.IsNullOrEmpty(m_options.VersionOverride))
            {
                version = m_options.VersionOverride;
                return ExitCode.Success;
            }
            
            string path = Path.Combine(project, "ProjectSettings", "ProjectVersion.txt");
            if (!File.Exists(path))
            {
                return ExitCode.ProjectVersionFileMissing;
            }

            try
            {
                version = File.ReadAllText(path).Split(':')[1].Trim();
            }
            catch (Exception)
            {
                return ExitCode.ProjectVersionParsingFailed;
            }
            
            return ExitCode.Success;
        }

        private ExitCode OpenUnity(string unity, string project)
        {
            Log("Opening Unity..");
            
            Process.Start(new ProcessStartInfo
                {
                    FileName = unity,
                    Arguments = $"-projectPath {project}"
                }
            );

            return ExitCode.Success;
        }

        private void Log(string message)
        {
            if (!m_options.Verbose)
            {
                return;
            }
            
            Console.WriteLine(message);
        }
    }
}