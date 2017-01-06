using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CC.Release
{
	class ReleaseConfig
	{
		public static Dictionary<string, string> Setting = new Dictionary<string, string>();

		public struct SettingDefine
		{
			public const string Target = "target";

			public const string Version = "version";

			public const string AppName = "appname";

			public const string AndroidExportType = "androidExportType";

			public const string IOSCodeSign = "codeSign";

            public const string BuildPath = "buildPath";

            public const string ProjectCodeName = "projectCodeName";
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

        #endregion

        #region Platform Spec Setting
        
        public struct Android
        {
            public static string KeyStoreFilePath = Application.dataPath + "/Editor/Release/Android/ExtraFiles/cc.keystore";

            public const string KeyStoreAliasName = "cc";

            public const string KeyStorePassword = "123456";

            public const string BundleID = "com.icodeten.cc";
        }

        public struct iOS
        {
            public const string BundleID = "com.icodeten.cc";
        }

        #endregion

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

            Setting[SettingDefine.Target] = Platform.Windows;

            Setting[SettingDefine.Version] = "0.0.1";

            Setting[SettingDefine.AppName] = "Demo";

            Setting[SettingDefine.ProjectCodeName] = "CC";

            Setting[SettingDefine.AndroidExportType] = AndroidExportType.ExportApk;

            Setting[SettingDefine.BuildPath] = targetBuildPath;

            Setting[SettingDefine.IOSCodeSign] = iOSCodeSign.Distribution;
		}


	}
}
