using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace CC.Release
{
    interface IBuild
    {
        BuildTarget Target { get; }

        BuildTargetGroup TargetGroup { get; }

        bool Setup();

        bool PreBuild();

        bool Build();

        bool PostBuild(BuildTarget target, string pathToBuiltProject);
    }
}
