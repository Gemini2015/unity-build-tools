using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	}
}
