using UnityEngine;
using System.Collections;
using System.IO;
using System;
using CC.Common;

namespace CC.Common
{
    public class Version
    {
        public enum VersionState
        {
            Lower,
            Equal,
            Higher,
        }

        public int MainVersion { get; private set; }

        public int SubVersion { get; private set; }

        public int MiniVersion { get; private set; }

        public string VersionString { get; private set; }

        public Version()
        {
            SetVersion(0, 0, 0);
        }

        public Version(string version)
        {
            SetVersion(version);
        }

        public void SetVersion(int mainVersion, int subVersion, int miniVersion)
        {
            MainVersion = mainVersion;
            SubVersion = subVersion;
            MiniVersion = miniVersion;
            VersionString = ToString();
        }

        public void SetVersion(string version)
        {
            if (string.IsNullOrEmpty(version))
                return;

            string[] t = version.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (t.Length >= 3)
            {
                MainVersion = int.Parse(t[0]);
                SubVersion = int.Parse(t[1]);
                MiniVersion = int.Parse(t[2]);
                VersionString = ToString();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", MainVersion, SubVersion, MiniVersion);
        }

        /// <summary>
        /// | MainVersion | SubVersion | MiniVersion |
        /// | user define | user define| svn version |
        /// | xx          | xx         | xxxxxx      |
        ///
        /// eg. 1 . 0 . 21903
        /// =>  10 00  021903
        ///
        /// tips:
        /// eg. 0 . 0 . 21903
        /// =>  00 00  021903
        /// =>          21903
        ///
        /// </summary>
        /// <returns></returns>
        public int ToNumber()
        {
            return MainVersion * 100000000 + SubVersion * 1000000 + MiniVersion;
        }

        public string ToNumberString()
        {
            return string.Format("{0:D2}{1:D2}{2:D6}", MainVersion, SubVersion, MiniVersion);
        }

        public VersionState CompareWith(Version other)
        {
            if (MainVersion != other.MainVersion)
                return MainVersion < other.MainVersion ? VersionState.Lower : VersionState.Higher;
            if (SubVersion != other.SubVersion)
                return SubVersion < other.SubVersion ? VersionState.Lower : VersionState.Higher;
            if (MiniVersion != other.MiniVersion)
                return MiniVersion < other.MiniVersion ? VersionState.Lower : VersionState.Higher;
            return VersionState.Equal;
        }
    }


    public class VersionManager : TSingleton<VersionManager>
    {
        public delegate void VersionChangedDelegate(string newVersion);

        public event VersionChangedDelegate OnVersionChanged = null;

        public Version CurrentVersion { get; private set; }

        protected override void OnCreateInstance()
        {
            CurrentVersion = new Version();
        }

        public string CurrentVersionString
        {
            get
            {
                return CurrentVersion.VersionString;
            }
            set
            {
                if (CurrentVersion.VersionString != value)
                {
                    CurrentVersion.SetVersion(value);
                    if (OnVersionChanged != null)
                        OnVersionChanged(CurrentVersion.VersionString);
                }
            }
        }

        public void Flush()
        {
#if UNITY_EDITOR
            File.WriteAllBytes(Application.persistentDataPath + "/version.txt", System.Text.Encoding.UTF8.GetBytes(CurrentVersion.ToString()));
#else
            File.WriteAllBytes(Application.persistentDataPath + "/version.txt", System.Text.Encoding.UTF8.GetBytes(CurrentVersion.ToString()));
#endif
        }

		public void EditorFlush()
		{
#if UNITY_EDITOR
			File.WriteAllBytes(Application.streamingAssetsPath + "/version.txt", System.Text.Encoding.UTF8.GetBytes(CurrentVersion.ToString()));
#endif
		}

        public static Version.VersionState CompareVersion(string a, string b)
        {
            Version va = new Version(a);
            Version vb = new Version(b);
            return va.CompareWith(vb);
        }
    }

}
