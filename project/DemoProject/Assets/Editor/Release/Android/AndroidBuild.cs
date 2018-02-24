using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace CC.Release.Android
{
    internal class AndroidBuild: GeneralBuild
    {
        public override BuildTarget Target
        {
            get
            {
                return BuildTarget.Android;
            }
        }

        public override BuildTargetGroup TargetGroup
        {
            get
            {
                return BuildTargetGroup.Android;
            }
        }

        public override bool Setup()
        {
            base.Setup();

            SetKeyStore();

            PlayerSettings.Android.bundleVersionCode = int.Parse(ReleaseConfig.Setting[ReleaseConfig.SettingDefine.BundleVersionCode]);

            PlayerSettings.bundleIdentifier = ReleaseConfig.Android.BundleID;

			EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

            return true;
        }

        public override bool Build()
        {
            var levels = ReleaseConfig.BuildLevels;
            var buildPath = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.BuildPath];
            var androidExportType = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.AndroidExportType];
            if(androidExportType == ReleaseConfig.AndroidExportType.ExportAndroidProject)
            {
                var buildProjectPath = Path.Combine(buildPath, ReleaseConfig.Setting[ReleaseConfig.SettingDefine.ProjectCodeName]);

                BuildPipeline.BuildPlayer(levels, buildProjectPath, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer | BuildOptions.ShowBuiltPlayer);
            }
            else
            {
                var apkName = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.ProjectCodeName] + ".apk";

                var outputPath = Path.Combine(buildPath, "output");
                if(!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }

                var apkPath = Path.Combine(outputPath, apkName);
                BuildPipeline.BuildPlayer(levels, apkPath, BuildTarget.Android, BuildOptions.ShowBuiltPlayer);
            }

            return true;
        }

        public override bool PostBuild(BuildTarget target, string pathToBuiltProject)
        {
            return base.PostBuild(target, pathToBuiltProject);
        }

        private void SetKeyStore()
        {
            PlayerSettings.Android.keystoreName = ReleaseConfig.Android.KeyStoreFilePath;
            PlayerSettings.Android.keystorePass = ReleaseConfig.Android.KeyStorePassword;
            PlayerSettings.Android.keyaliasName = ReleaseConfig.Android.KeyStoreAliasName;
            PlayerSettings.Android.keyaliasPass = ReleaseConfig.Android.KeyStorePassword;
        }
    }
}
