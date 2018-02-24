using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using CC.Common;
using UnityEditor;
using UnityEngine;


namespace CC.Release
{

	public class VersionUtil
	{
		public const string FILES_TXT = "files.txt";

		public const string STREAMPATH_TXT = "streamPath.txt";

		public const string VERSION_TXT = "version.txt";

		/// <summary>
		///
		/// | MainVersion | SubVersion | MiniVersion |
		/// | user define | user define| svn version |
		/// | xx          | xx         | xxxxxx      |
		///
		///
		/// </summary>
		/// <param name="versionNum"></param>
		public static void CreateVersion(string versionNum)
		{
			string streamPath = Application.streamingAssetsPath;

			// Create files.ext
			string outPutFiles = Path.Combine(Application.streamingAssetsPath, FILES_TXT);
			CollectFiles(streamPath, outPutFiles);

			// Create streamPath.txt
			FileInfo fi = new FileInfo(Path.Combine(streamPath, STREAMPATH_TXT));
			using(StreamWriter sw = fi.CreateText())
			{
				GetFilePath(streamPath, sw);
			}

			//更新版本号
			VersionManager.Instance.CurrentVersionString = versionNum;
			VersionManager.Instance.EditorFlush();

			AssetDatabase.Refresh();
			Debug.Log("Create Version " + versionNum);
		}

		[MenuItem("Tools/ResetVersion", priority = 1)]
		public static void Reset()
		{
			VersionManager.Instance.CurrentVersionString = "0.0.1";
			VersionManager.Instance.Flush();
			CreateVersion(VersionManager.Instance.CurrentVersionString);
		}

		private static void GetFilePath(string sourcePath, StreamWriter sw)
		{
			DirectoryInfo info = new DirectoryInfo(sourcePath);
			foreach(FileSystemInfo fsi in info.GetFileSystemInfos())
			{
				Debug.Log("GetFilePath " + fsi.Name);
				if(fsi.Extension == ".gitkeep" || fsi.Extension == ".gitignore")
					continue;
				if(fsi.Extension != ".meta" && fsi.Name != STREAMPATH_TXT)
				{
					string[] r = fsi.FullName.Split(new string[] { "StreamingAssets" }, System.StringSplitOptions.None); //得到相对路径

					//安卓上只能识别"/"
					r[1] = r[1].Replace('\\', '/');

					if(fsi is DirectoryInfo)
					{
						//是文件夹则迭代
						sw.WriteLine(r[1] + " | 0"); //按行写入
						GetFilePath(fsi.FullName, sw);
					}
					else
					{
						//按行写入
						sw.WriteLine(r[1] + " | 1" + "|" + string.Format("{0:F}", ((FileInfo)fsi).Length / 1024.0f));
					}
				}
			}
		}

        private static List<string> paths = new List<string>();
        private static List<string> files = new List<string>();

        static public void CollectFiles(string sourcePath, string outPutFiles)
		{
			if(File.Exists(outPutFiles))
				File.Delete(outPutFiles);

			paths.Clear();
			files.Clear();
			Recursive(sourcePath);

			FileStream fs = new FileStream(outPutFiles, FileMode.CreateNew);
			StreamWriter sw = new StreamWriter(fs);
			for(int i = 0; i < files.Count; i++)
			{
				string file = files[i];
				string fileName = Path.GetFileName(file);
				if(file.EndsWith(".meta") || file.Contains(".DS_Store"))
					continue;
				Debug.Log("CollectFiles " + fileName);
				if(fileName == ".gitkeep" || fileName == ".gitignore")
					continue;

				if(fileName == "staticconfig.json")
					continue;

				FileStream fileStream = new FileStream(file, FileMode.Open);

				int size = (int)fileStream.Length;

				string md5 = ComputeFileMD5(fileStream);
				string value = file.Replace(sourcePath + "/", string.Empty);
				sw.WriteLine(value + "|" + md5 + "|" + size);
				fileStream.Close();
			}
			sw.Close();
			fs.Close();
		}

		/// <summary>
		/// 遍历目录及其子目录
		/// </summary>
		static public void Recursive(string path)
		{
			string[] names = Directory.GetFiles(path);
			string[] dirs = Directory.GetDirectories(path);
			foreach(string filename in names)
			{
				string ext = Path.GetExtension(filename);
				if(ext.Equals(".meta"))
					continue;
				files.Add(filename.Replace('\\', '/'));
			}
			foreach(string dir in dirs)
			{
				paths.Add(dir.Replace('\\', '/'));
				Recursive(dir);
			}
		}

		public static string ComputeFileMD5(FileStream fs)
		{
			try
			{
				System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
				byte[] retVal = md5.ComputeHash(fs);

				StringBuilder sb = new StringBuilder();
				for(int i = 0; i < retVal.Length; i++)
				{
					sb.Append(retVal[i].ToString("x2"));
				}
				return sb.ToString();
			}
			catch(Exception ex)
			{
				throw new Exception("VersionUtil.ComputeFileMD5 Error " + ex.Message);
			}
		}
	}
}