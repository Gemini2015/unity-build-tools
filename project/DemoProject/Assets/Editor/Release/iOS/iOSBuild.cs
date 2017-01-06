using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using System.IO;

namespace CC.Release.iOS
{
    class iOSBuild: GeneralBuild
    {
        public override BuildTarget Target
        {
            get
            {
                return BuildTarget.iOS;
            }
        }

        public override BuildTargetGroup TargetGroup
        {
            get
            {
                return BuildTargetGroup.iOS;
            }
        }

        public override bool Setup()
        {
            base.Setup();

            PlayerSettings.bundleIdentifier = ReleaseConfig.iOS.BundleID;
            PlayerSettings.iPhoneBundleIdentifier = ReleaseConfig.iOS.BundleID;

            return true;
        }

        public override bool Build()
        {
            base.Build();

            var levels = ReleaseConfig.BuildLevels;
            var buildPath = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.BuildPath];
            var buildProjectPath = Path.Combine(buildPath, ReleaseConfig.Setting[ReleaseConfig.SettingDefine.ProjectCodeName]);

            BuildPipeline.BuildPlayer(levels, buildProjectPath, BuildTarget.iOS, BuildOptions.Il2CPP | BuildOptions.ShowBuiltPlayer);

            return true;
        }

        public override bool PostBuild(BuildTarget target, string pathToBuiltProject)
        {
            base.PostBuild(target, pathToBuiltProject);

            return true;
        }
    }
}
