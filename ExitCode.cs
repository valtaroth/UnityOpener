namespace UnitySelector
{
    public enum ExitCode
    {
        Success = 0,
        
        MissingProjectPath,
        NoUnityProjectFound,
        MultipleUnityProjectsFound,
        
        ProjectVersionFileMissing,
        ProjectVersionParsingFailed,
        UnityInstallationMissing
    }
}