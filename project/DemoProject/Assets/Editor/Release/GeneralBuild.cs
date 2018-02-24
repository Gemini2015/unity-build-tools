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
            SetDefineSymbol();
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

            ReleaseUtil.CleanPlugin(ReleaseConfig.GetBuildTarget());

            #region Create AssetBundle

            //AssetBundles.AssetBundleUtil.Build();
            //AssetBundles.AssetBundleUtil.CopyToStreamingAssets();

            #endregion Create AssetBundle


			ReleaseUtil.CreateVersion();
			var patchPath = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.PatchPath];
			if(!Directory.Exists(patchPath))
			{
				Directory.CreateDirectory(patchPath);
			}

            return true;
		}

        public virtual bool Build()
		{
			return true;
		}

        public virtual bool PostBuild(BuildTarget target, string pathToBuiltProject)
		{
			var patchPath = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.PatchPath];
			ReleaseUtil.CreateHotpatch(patchPath);
			return true;
		}

        private void SetAppInfo()
        {
            PlayerSettings.bundleVersion = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.Version];
            PlayerSettings.productName = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.AppName];
        }

        private void SetDefineSymbol()
        {
            var buildTarget = ReleaseConfig.GetBuildTargetGroup();
            var buildConfig = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.BuildConfig];
            switch(buildConfig)
            {
                case ReleaseConfig.BuildConfig.InternetDev:
                    {
                        var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget);
                        if(string.IsNullOrEmpty(symbols))
                            symbols = "InternetDev";
                        else
                        {
                            if(!symbols.Contains("InternetDev"))
                                symbols = symbols + ";" + "InternetDev";
                        }

                        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, symbols);
                    }
                    break;

                case ReleaseConfig.BuildConfig.InternetDis:
                default:
                    {
                        var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget);
                        if(string.IsNullOrEmpty(symbols))
                            symbols = "InternetDis";
                        else
                        {
                            if(!symbols.Contains("InternetDis"))
                                symbols = symbols + ";" + "InternetDis";
                        }

                        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, symbols);
                    }
                    break;
            }
        }
    }
}
