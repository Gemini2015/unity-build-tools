using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace CC.Release.Windows
{
    class WindowsBuild: GeneralBuild
    {
        public override BuildTarget Target
        {
            get
            {
                return BuildTarget.StandaloneWindows64;
            }
        }

        public override BuildTargetGroup TargetGroup
        {
            get
            {
                return BuildTargetGroup.Standalone;
            }
        }

        public override bool Build()
        {
            var levels = ReleaseConfig.BuildLevels;
            var buildPath = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.BuildPath];
            var exeName = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.ProjectCodeName] + ".exe";

            var outputPath = Path.Combine(buildPath, "output");
            if(!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var exePath = Path.Combine(outputPath, exeName);
            BuildPipeline.BuildPlayer(levels, exePath, BuildTarget.StandaloneWindows64, BuildOptions.ShowBuiltPlayer);

            return true;
        }
    }
}
