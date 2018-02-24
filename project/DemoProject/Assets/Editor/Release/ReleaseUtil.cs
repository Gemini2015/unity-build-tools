using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CC.Release
{
	class ReleaseUtil
	{
        public static string GetUnityProjectPath()
        {
            int pos = Application.dataPath.LastIndexOf("/");
            string projectPath = Application.dataPath.Substring(0, pos);
            return projectPath;
        }

        public static void CleanPlugin(BuildTarget target)
        {
            string rootPath = Application.dataPath + "/Plugins/";
            switch(target)
            {
                case BuildTarget.Android:
                    {
                        DeleteDirectory(rootPath + "iOS");
                        DeleteDirectory(rootPath + "tolua.bundle");
                        DeleteDirectory(rootPath + "x86");
                        //DeleteDirectory(rootPath + "x86_64");
                    }
                    break;
                case BuildTarget.iOS:
                    {
                        DeleteDirectory(rootPath + "Android");
                        DeleteDirectory(rootPath + "tolua.bundle");
                        DeleteDirectory(rootPath + "x86");
                        //DeleteDirectory(rootPath + "x86_64");
                    }
                    break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                default:
                    break;
            }
        }

        private static void DeleteDirectory(string dirName)
        {
            DirectoryInfo dir = new DirectoryInfo(dirName);
            if(dir.Exists)
            {
                try
                {
                    Directory.Delete(dirName, true);
                }
                catch(Exception ex)
                {
                    Debug.Log("DeleteDirectory " + ex.ToString());
                }
            }
        }

		#region Version

		public static void CreateVersion()
		{
			VersionUtil.CreateVersion(ReleaseConfig.Setting[ReleaseConfig.SettingDefine.Version]);

			AssetDatabase.Refresh();
			Debug.Log("Create New Version Success");
		}

		public static void CreateHotpatch(string hotpatchPath)
		{
			if (!Directory.Exists(hotpatchPath))
				Directory.CreateDirectory(hotpatchPath);
			else
			{
				Directory.Delete(hotpatchPath, true);
				Directory.CreateDirectory(hotpatchPath);
			}
			CopyToHotpatch(hotpatchPath);
		}

		private static void CopyToHotpatch(string hotpatchPath)
		{
			var sourcePath = Application.streamingAssetsPath;
			var targetPath = hotpatchPath;

			Queue<string> entries = new Queue<string>();
			entries.Enqueue(sourcePath);
			while (entries.Count > 0)
			{
				var entry = entries.Dequeue();
				var files = Directory.GetFiles(entry, "*", SearchOption.TopDirectoryOnly);
				foreach (var file in files)
				{
					var fileExt = Path.GetExtension(file);
					if (fileExt != ".meta")
					{
						var targetFile = file.Replace(sourcePath, targetPath);
						File.Copy(file, targetFile);
					}
				}

				var dirs = Directory.GetDirectories(entry, "*", SearchOption.TopDirectoryOnly);
				foreach (var dir in dirs)
				{
					var targetDir = dir.Replace(sourcePath, targetPath);
					if (!Directory.Exists(targetDir))
						Directory.CreateDirectory(targetDir);
					entries.Enqueue(dir);
				}
			}
		}

		#endregion
	}
}
