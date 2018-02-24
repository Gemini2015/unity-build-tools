using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CC.Release
{
	class ReleaseWindowCmd
	{
		[MenuItem("Release/ReleaseWindow")]
		public static void CreateReleaseWindow()
		{
			EditorWindow.GetWindow<ReleaseWindow>();
		}
	}

	class ReleaseWindow: EditorWindow
	{
		private Dictionary<string, string> dummySetting = null;

		private Vector2 scrollPos = Vector2.zero;

        private bool setDefaultWindowSize = true;

        public ReleaseWindow()
		{
		}

        private void OnEnable()
        {
            ReleaseConfig.Reset();
            dummySetting = new Dictionary<string, string>();
            foreach(var item in ReleaseConfig.Setting)
            {
                dummySetting[item.Key] = item.Value;
            }
        }

        private void OnGUI()
		{
            if(setDefaultWindowSize)
            {
                setDefaultWindowSize = false;
                GUILayout.Width(400);
                GUILayout.Height(600);
                GUILayout.ExpandWidth(true);
            }

			scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical("Setting", GUIStyle.none, GUILayout.ExpandWidth(true));

            #region Setting

            foreach(var item in ReleaseConfig.Setting)
            {
                GUILayout.Label(item.Key, EditorStyles.boldLabel);
                dummySetting[item.Key] = GUILayout.TextField(item.Value, GUILayout.ExpandWidth(true));

                GUILayout.Space(15);
            }

            foreach(var item in dummySetting)
            {
                ReleaseConfig.Setting[item.Key] = item.Value;
            }

            GUILayout.Space(15);

            #endregion

            if(GUILayout.Button("Release"))
			{
				Release();
			}

			GUILayout.EndVertical();
			GUILayout.EndScrollView();
		}

		private void Release()
		{
			if(!VerifySetting())
			{
				Debug.LogError("Window Release Setting Error");
				return;
			}
			ReleaseCore.Release();
		}

		private bool VerifySetting()
		{
			return true;
		}
	}
}
