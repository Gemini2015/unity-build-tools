using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CC.Release
{
    internal class ReleaseConfig
    {
        public static Dictionary<string, string> Setting = new Dictionary<string, string>();

        public struct SettingDefine
        {
            public const string Target = "target";

            public const string Version = "version";

            /// <summary>
            /// 游戏的名称
            /// </summary>
            public const string AppName = "appname";

            public const string AndroidExportType = "androidExportType";

            public const string IOSCodeSign = "codeSign";

            public const string BuildPath = "buildPath";

            public const string PatchPath = "hotpatch";

            /// <summary>
            /// 用作包名
            /// </summary>
            public const string ProjectCodeName = "projectCodeName";

            public const string BuildConfig = "buildConfig";

            public const string BundleVersionCode = "bundleVersionCode";

            public const string iOSExportType = "iOSExportType";
        }

        internal static BuildTargetGroup GetBuildTargetGroup()
        {
            var target = Setting[SettingDefine.Target];
            var targetGroup = BuildTargetGroup.iOS;

            if(target == Platform.iOS)
                targetGroup = BuildTargetGroup.iOS;
            else if(target == Platform.Android)
                targetGroup = BuildTargetGroup.Android;
            else if(target == Platform.Windows)
                targetGroup = BuildTargetGroup.Standalone;

            return targetGroup;
        }

        internal static BuildTarget GetBuildTarget()
        {
            var target = Setting[SettingDefine.Target];
            var buildTarget = BuildTarget.iOS;

            if(target == Platform.iOS)
                buildTarget = BuildTarget.iOS;
            else if(target == Platform.Android)
                buildTarget = BuildTarget.Android;
            else if(target == Platform.Windows)
                buildTarget = BuildTarget.StandaloneWindows64;

            return buildTarget;
        }

        public static string[] BuildLevels =
		{
			"Assets/Scenes/Main.unity",
		};

        #region Setting Value Enum

        public struct Platform
        {
            public const string iOS = "ios";
            public const string Android = "android";
            public const string Windows = "windows";
        }

        public struct AndroidExportType
        {
            public const string ExportAndroidProject = "exportAndroidProject";

            public const string ExportApk = "exportApk";
        }

        public struct iOSCodeSign
        {
            public const string Developer = "developer";

            public const string Distribution = "distribution";
        }

        public struct BuildConfig
        {
            // InternetDev
            public const string InternetDev = "internetdev";

            // InternetDis
            public const string InternetDis = "internetdis";
        }

        public struct iOSExportType
        {
            public const string Enterprise = "enterprise";

            public const string AppStore = "appstore";
        }

        #endregion Setting Value Enum

        #region Platform Spec Setting

        public struct Android
        {
            public static string KeyStoreFilePath = Application.dataPath + "/Editor/Release/Android/ExtraFiles/zongzi.keystore";

            public const string KeyStoreAliasName = "zongzi";

            public const string KeyStorePassword = "123456";

            public const string BundleID = "com.icodeten.zongzi";
        }

        public struct iOS
        {
            public enum KeyDefine
            {
                BundleID,
                BundlePostfix,
                DevelopmentTeam,
                DevelopmentProvision,
                DevelopmentCert,
                DistributionProvision,
                DistributionCert,
                ExportFile,
            }

            public static Dictionary<KeyDefine, string> appStore = new Dictionary<KeyDefine, string>()
            {
                { KeyDefine.BundleID, "com.icodeten.zongzi" },
                { KeyDefine.BundlePostfix, "zongzi" },
                { KeyDefine.DevelopmentTeam, "zongzi" },
                { KeyDefine.DevelopmentProvision, "zongzi-dev" },
                { KeyDefine.DevelopmentCert, "iPhone Developer: zongzi" },
                { KeyDefine.DistributionProvision, "zongzi-dis" },
                { KeyDefine.DistributionCert, "iPhone Distribution: zongzi" },
                { KeyDefine.ExportFile, "exportAppStore.plist" },
            };

            public static Dictionary<KeyDefine, string> enterprise = new Dictionary<KeyDefine, string>()
            {
                { KeyDefine.BundleID, "com.icodeten.zongzidev" },
                { KeyDefine.BundlePostfix, "zongzidev" },
                { KeyDefine.DevelopmentTeam, "zongzidev" },
                { KeyDefine.DevelopmentProvision, "zongzidev-dev" },
                { KeyDefine.DevelopmentCert, "iPhone Developer: zongzidev" },
                { KeyDefine.DistributionProvision, "zongzidev-Inhouse" },
                { KeyDefine.DistributionCert, "iPhone Distribution: zongzidev" },
                { KeyDefine.ExportFile, "exportEnterprise.plist" },
            };

            public static string GetValue(KeyDefine key)
            {
                if(Setting[SettingDefine.iOSExportType] == iOSExportType.AppStore)
                    return appStore[key];
                else
                    return enterprise[key];
            }
        }

#endregion Platform Spec Setting

        public static void Reset()
        {
            Setting.Clear();
            InitDefaultSetting();
        }

        private static void InitDefaultSetting()
        {
            var defaultTarget = Platform.Windows;
            var unityProjectPath = ReleaseUtil.GetUnityProjectPath();
            var buildRootPath = Path.GetFullPath(Path.Combine(unityProjectPath, "../../build"));
            var targetBuildPath = Path.GetFullPath(Path.Combine(buildRootPath, defaultTarget));
            var targetPatchPath = Path.GetFullPath(Path.Combine(targetBuildPath, "./hotpatch"));

            Setting[SettingDefine.Target] = defaultTarget;

            Setting[SettingDefine.Version] = "0.0.1";

            Setting[SettingDefine.AppName] = "Demo";

            Setting[SettingDefine.ProjectCodeName] = "zongzi";

            Setting[SettingDefine.AndroidExportType] = AndroidExportType.ExportApk;

            Setting[SettingDefine.BuildPath] = targetBuildPath;

            Setting[SettingDefine.PatchPath] = targetPatchPath;

            Setting[SettingDefine.IOSCodeSign] = iOSCodeSign.Developer;

            Setting[SettingDefine.BuildConfig] = BuildConfig.InternetDev;

            Setting[SettingDefine.BundleVersionCode] = "1";

            Setting[SettingDefine.iOSExportType] = iOSExportType.Enterprise;

        }


    }
}
