#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;
using UnityEngine.XR.Management;
using UnityEditor.XR.Management;
using System.Linq;
using Unity.XR.Oculus;
using UnityEngine.XR.OpenXR;
using System.Collections.Generic;

/// <summary>
/// A utility class with menu items to automate the build process for the server and client.
/// </summary>
public static class BuildScript
{
    // === CONFIGURATION ===
    // You can change these paths and names to match your project.
    private const string c_ServerFolderName = "Server";
    private const string c_ClientWindowsFolderName = "Client_Windows";
    private const string c_ClientAndroidFolderName = "Client_Android";
    private const string c_ExecutableName = "CesiPlayground";

    // Define the list of scenes for the CLIENT build here.
    private static readonly string[] s_ClientScenes =
    {
        "Assets/Scenes/Client_Boot.unity",
        "Assets/Scenes/StartMenu.unity",
        "Assets/Scenes/Hub.unity",
        "Assets/Scenes/Games/GunClub_Scene.unity",
        "Assets/Scenes/Games/MollSmash_Scene.unity"
    };

    // Define the single scene for the SERVER build here.
    private static readonly string[] s_ServerScene =
    {
        "Assets/Scenes/ServerScene.unity"
    };

    // === MENU ITEMS ===

    [MenuItem("Build/Client/Windows (OpenXR)...")]
    public static void BuildClientWindowsOpenXR()
    {
        BuildClient(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, c_ClientWindowsFolderName, new List<System.Type> { typeof(OpenXRLoader) }, ".exe");
    }

    [MenuItem("Build/Client/Android - APK (Oculus)...")]
    public static void BuildClientAndroidOculus()
    {
        BuildClient(BuildTarget.Android, BuildTargetGroup.Android, c_ClientAndroidFolderName, new List<System.Type> { typeof(OculusLoader) }, ".apk");
    }

    [MenuItem("Build/Client/Android - APK (OpenXR)...")]
    public static void BuildClientAndroidOpenXR()
    {
        BuildClient(BuildTarget.Android, BuildTargetGroup.Android, c_ClientAndroidFolderName, new List<System.Type> { typeof(OpenXRLoader) }, ".apk");
    }

    [MenuItem("Build/Server/Linux (Headless)...")]
    public static void BuildServerLinux()
    {
        string rootPath = AskForBuildLocation("Select Linux Server Build Folder");
        if (string.IsNullOrEmpty(rootPath)) return;

        var buildTargetGroup = BuildTargetGroup.Standalone;

        // Save original XR settings
        XRGeneralSettings settings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(buildTargetGroup);
        bool wasXREnabled = settings != null && settings.InitManagerOnStart;
        List<XRLoader> originalLoaders = GetActiveLoaders(buildTargetGroup);

        try
        {
            Debug.Log("Disabling XR for server build...");
            SetXREnabledFor(buildTargetGroup, false);

            SetLoadersForBuild(buildTargetGroup, new List<XRLoader>());

            string outputPath = Path.Combine(rootPath, c_ServerFolderName, $"{c_ExecutableName}.x86_64");
            var options = new BuildPlayerOptions
            {
                scenes = s_ServerScene,
                locationPathName = outputPath,
                target = BuildTarget.StandaloneLinux64,
                subtarget = (int)StandaloneBuildSubtarget.Server
            };

            PerformBuild(options, "Server (Linux)");
        }
        finally
        {
            Debug.Log("Restoring original XR settings for Standalone build target...");
            SetLoadersForBuild(buildTargetGroup, originalLoaders);
            SetXREnabledFor(buildTargetGroup, wasXREnabled);
        }
    }

    // === CORE BUILD LOGIC ===

    private static void BuildClient(BuildTarget buildTarget, BuildTargetGroup buildTargetGroup, string folderName, List<System.Type> desiredLoaders, string extension)
    {
        string rootPath = AskForBuildLocation($"Select Build Folder for {buildTarget}");
        if (string.IsNullOrEmpty(rootPath)) return;

        List<XRLoader> originalLoaders = GetActiveLoaders(buildTargetGroup);

        try
        {
            SetXREnabledFor(buildTargetGroup, true);
            SetLoadersForBuild(buildTargetGroup, desiredLoaders);

            string outputPath = Path.Combine(rootPath, folderName, $"{c_ExecutableName}{extension}");
            var options = new BuildPlayerOptions { scenes = s_ClientScenes, locationPathName = outputPath, target = buildTarget, options = BuildOptions.None };
            PerformBuild(options, $"Client ({buildTarget})");
        }
        finally
        {
            SetLoadersForBuild(buildTargetGroup, originalLoaders);
            Debug.Log($"Build settings for {buildTargetGroup} restored.");
        }
    }

    // === HELPER METHODS ===

    private static string AskForBuildLocation(string title)
    {
        string path = EditorUtility.SaveFolderPanel(title, "", "");
        if (string.IsNullOrEmpty(path)) { return string.Empty; }
        return path;
    }

    private static void PerformBuild(BuildPlayerOptions options, string buildTypeName)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(options.locationPathName));
        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"✅ {buildTypeName} build succeeded! ({summary.totalSize / 1024 / 1024} MB) \nOutput at: {summary.outputPath}");
            EditorUtility.RevealInFinder(options.locationPathName);
        }
        else
        {
            Debug.LogError($"❌ {buildTypeName} build failed with {summary.totalErrors} errors.");
        }
    }

    private static void SetXREnabledFor(BuildTargetGroup buildTargetGroup, bool isEnabled)
    {
        XRGeneralSettings settings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(buildTargetGroup);
        if (settings != null) { settings.InitManagerOnStart = isEnabled; }
    }
    
    private static XRManagerSettings GetXRManagerSettings(BuildTargetGroup buildTargetGroup)
    {
        XRGeneralSettings settings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(buildTargetGroup);
        return settings?.AssignedSettings;
    }
    
    private static List<XRLoader> GetActiveLoaders(BuildTargetGroup buildTargetGroup)
    {
        XRManagerSettings managerSettings = GetXRManagerSettings(buildTargetGroup);
        return managerSettings != null ? new List<XRLoader>(managerSettings.activeLoaders) : new List<XRLoader>();
    }

    private static void SetLoadersForBuild(BuildTargetGroup buildTargetGroup, List<System.Type> desiredLoaderTypes)
    {
        XRManagerSettings managerSettings = GetXRManagerSettings(buildTargetGroup);
        if (managerSettings == null) return;
        
        var allLoadersInProject = AssetDatabase.FindAssets("t:XRLoader")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<XRLoader>).Where(l => l != null).ToList();

        var newLoadersList = desiredLoaderTypes.Select(type => allLoadersInProject.FirstOrDefault(l => l.GetType() == type)).Where(l => l != null).ToList();
        
        managerSettings.TrySetLoaders(newLoadersList);
    }

    private static void SetLoadersForBuild(BuildTargetGroup buildTargetGroup, List<XRLoader> loaders)
    {
        XRManagerSettings managerSettings = GetXRManagerSettings(buildTargetGroup);
        if (managerSettings != null) { managerSettings.TrySetLoaders(loaders); }
    }
}
#endif