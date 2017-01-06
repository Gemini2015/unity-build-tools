using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;
using System.IO;

namespace CC.Release
{
	class ReleaseConsole
	{
        public const string ReleaseCommandLine = "CC.Release.ReleaseConsole.Release";

        public static void Release()
        {
            ReleaseConfig.Reset();

            ProcessCommandLineArgs();

            if(!VerifySetting())
            {
                Debug.LogError("Console Release Setting Error");
                return;
            }

            ReleaseCore.Release();
        }

        private static bool VerifySetting()
        {
            var version = ReleaseConfig.Setting[ReleaseConfig.SettingDefine.Version];
            // version: x.x.x
            if(!Regex.IsMatch(version, @"\d+\.\d+\.\d+"))
            {
                Debug.LogError("Console Release Setting [version] error: " + version);
                return false;
            }
            return true;
        }

        private static void ProcessCommandLineArgs()
        {
            var args = System.Environment.GetCommandLineArgs();
            bool flag = false;
            for(int i = 0; i < args.Length; ++i)
            {
                Debug.Log(args[i]);
                if(flag)
                {
                    var arg = args[i];
                    int pos = arg.IndexOf('=');
                    if(pos != -1)
                    {
                        var argName = arg.Substring(0, pos);
                        var argValue = arg.Substring(pos + 1);
                        
                        if(Directory.Exists(argValue))
                        {
                            argValue = Path.GetFullPath(argValue);
                        }

                        ReleaseConfig.Setting[argName] = argValue;
                    }
                }

                if(args[i] == ReleaseCommandLine)
                    flag = true;
            }

            Debug.Log("Settings:");
            foreach(var item in ReleaseConfig.Setting)
            {
                Debug.LogFormat("{0} = {1}", item.Key, item.Value);
            }
        }
    }
}

