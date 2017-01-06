using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Callbacks;
using UnityEditor;

namespace CC.Release
{
	class ReleaseCore
	{
        private static IBuild build = null;

		public static void Release()
		{
            var targetGroup = ReleaseConfig.GetBuildTargetGroup();
            switch(targetGroup)
            {
                case BuildTargetGroup.iOS:
                    build = new iOS.iOSBuild();
                    break;
                case BuildTargetGroup.Android:
                    build = new Android.AndroidBuild();
                    break;
                case BuildTargetGroup.Standalone:
                    build = new Windows.WindowsBuild();
                    break;
                default:
                    break;
            }

            if(build == null)
            {
                Debug.LogError("Invalid Build Target");
                return;
            }

            if(!build.Setup())
            {
                Debug.LogError("Build Setup Error");
                return;
            }

            if(!build.PreBuild())
            {
                Debug.LogError("Build PreBuild Error");
                return;
            }

            if(!build.Build())
            {
                Debug.LogError("Build Error");
                return;
            }
        }

        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if(build == null)
                return;
            if(build.Target != target)
            {
                Debug.LogError("Target Error");
                return;
            }

            build.PostBuild(target, pathToBuiltProject);
        }
	}
}

