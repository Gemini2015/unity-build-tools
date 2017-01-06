using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace CC.Release
{
	public abstract class GeneralBuild: IBuild
	{
		public abstract BuildTarget Target { get; }

		public abstract BuildTargetGroup TargetGroup { get; }

		public virtual bool Setup()
		{
            SetAppInfo();
			return true;
		}

        public virtual bool PreBuild()
		{
            var buildPath = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.BuildPath];
            if(!Directory.Exists(buildPath))
            {
                Directory.CreateDirectory(buildPath);
            }
			return true;
		}

        public virtual bool Build()
		{
			return true;
		}

        public virtual bool PostBuild(BuildTarget target, string pathToBuiltProject)
		{
			return true;
		}

        private void SetAppInfo()
        {
            PlayerSettings.bundleVersion = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.Version];
            PlayerSettings.productName = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.AppName];
        }
        
    }
}
