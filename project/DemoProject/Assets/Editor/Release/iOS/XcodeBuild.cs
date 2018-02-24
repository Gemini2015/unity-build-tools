using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using UnityEngine;

namespace CC.Release.iOS
{
    internal class XcodeBuild
    {

        private static string xcodeProjectRoot;

        private static string xcodeProject;

        private static string ipaPath;

        private static string rawIPAFilePath;

        private static string targetIPAFilePath;

        private static string archivePath;

        private static string exportOptionsPlist;

        public static void Init()
        {
            xcodeProjectRoot = "";
            xcodeProject = "Unity-iPhone.xcodeproj";
            ipaPath = "../output";
            rawIPAFilePath = "../output/Unity-iPhone.ipa";
            targetIPAFilePath = "../output/" + ReleaseConfig.Setting[ReleaseConfig.SettingDefine.ProjectCodeName] + ".ipa";
            archivePath = "./build/" + ReleaseConfig.iOS.GetValue(ReleaseConfig.iOS.KeyDefine.BundlePostfix) + ".xcarchive";
            exportOptionsPlist = Application.dataPath + "/Editor/Release/iOS/ExtraFiles/" + ReleaseConfig.iOS.GetValue(ReleaseConfig.iOS.KeyDefine.ExportFile);
        }

        public static void Build(string pathToBuiltProject)
        {
            if(string.IsNullOrEmpty(xcodeProject))
            {
                UnityEngine.Debug.LogError("XcodeBuild.Init first");
                return;
            }

            xcodeProjectRoot = pathToBuiltProject;
            UnityEngine.Debug.Log("targetIPAFilePath " + targetIPAFilePath);
            if(!InitParams())
            {
                UnityEngine.Debug.LogError("参数初始化错误");
                return;
            }

            if(!Clean())
            {
                UnityEngine.Debug.LogError("Xcode Clean Error");
                return;
            }

            if(!BuildInternal())
            {
                UnityEngine.Debug.LogError("Xcode Build Internal First Error");
                UnityEngine.Debug.Log("Xcode Build Internal First Failure, try again!");
                if(!BuildInternal())
                {
                    UnityEngine.Debug.LogError("Xcode Build Internal Second Error");
                    return;
                }
            }

            if(!GenerateIPA())
            {
                UnityEngine.Debug.LogError("Xcode GenerateIPA Error");
                return;
            }

            if(!RenameIPAFile())
            {
                UnityEngine.Debug.LogError("Rename IPA File Error");
                return;
            }

            UnityEngine.Debug.Log("Xcode Build Success");
        }

        private static bool InitParams()
        {
            if(string.IsNullOrEmpty(xcodeProjectRoot))
                return false;

            xcodeProject = Path.Combine(xcodeProjectRoot, xcodeProject);
            ipaPath = Path.Combine(xcodeProjectRoot, ipaPath);
            rawIPAFilePath = Path.Combine(xcodeProjectRoot, rawIPAFilePath);
            targetIPAFilePath = Path.Combine(xcodeProjectRoot, targetIPAFilePath);
            archivePath = Path.Combine(xcodeProjectRoot, archivePath);

            UnityEngine.Debug.Log("xcodeProject " + xcodeProject);
            UnityEngine.Debug.Log("ipaPath " + ipaPath);
            return true;
        }

        // xcodebuild -verbose -project projectAbsPath -target Unity-iPhone -configuration Release clean
        private static bool Clean()
        {

            string args = " -verbose " + " -project " + xcodeProject + " -target " + " Unity-iPhone " + " -configuration " + " Release " + " clean";

            UnityEngine.Debug.Log("xcodebuild " + args);

            return StartProcess("xcodebuild", args);
        }

        // xcodebuild archive -project projectAbsPath -scheme Unity-iPhone -configuration Release -archivePath archiveAbsPath {sign args}
        private static bool BuildInternal()
        {
            string args = " archive " + " -project " + xcodeProject + " -scheme Unity-iPhone " + " -archivePath " + archivePath + " -configuration " + " Release " + " -jobs 8";

            UnityEngine.Debug.Log("xcodebuild " + args);

            return StartProcess("xcodebuild", args);
        }

        // xcodebuild -exportArchive -exportFormat ipa -archivePath ./build.xcarchive/ -exportPath ../output
        private static bool GenerateIPA()
        {

            DirectoryInfo ipaPathFolder = Directory.GetParent(ipaPath);
            if(!ipaPathFolder.Exists)
            {
                UnityEngine.Debug.Log("Create Folder " + ipaPathFolder);
                ipaPathFolder.Create();
            }

            string args = " -verbose -exportArchive " + " -archivePath " + archivePath + " -exportOptionsPlist " + exportOptionsPlist + " -exportPath " + ipaPath;

            UnityEngine.Debug.Log("xcodebuild " + args);

            return StartProcess("xcodebuild", args);
        }

        private static bool RenameIPAFile()
        {
            if(!File.Exists(rawIPAFilePath))
            {
                UnityEngine.Debug.Log("RenameIPAFile Error: " + rawIPAFilePath + " not exist");
                return false;
            }

            if(File.Exists(targetIPAFilePath))
            {
                File.Delete(targetIPAFilePath);
            }

            UnityEngine.Debug.Log("mv " + rawIPAFilePath + " " + targetIPAFilePath);
            File.Move(rawIPAFilePath, targetIPAFilePath);

            return true;
        }

        private static bool StartProcess(string cmd, string args)
        {
            try
            {
                Process process = new Process();

                ProcessStartInfo startInfo = new ProcessStartInfo(cmd, args);

                process.StartInfo = startInfo;


                process.StartInfo.UseShellExecute = false;

                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

                process.Start();

                System.IO.StreamReader output = process.StandardOutput;

                System.IO.StreamReader err = process.StandardError;

                string outputString = output.ReadToEnd();

                string errString = err.ReadToEnd();

                process.WaitForExit();

                UnityEngine.Debug.Log(outputString);

                output.Close();

                err.Close();

                process.Close();

                if(errString != null && errString.Length != 0)
                {
                    UnityEngine.Debug.LogError(errString);
                    if(errString.IndexOf("BUILD FAILED") != -1)
                        return false;
                    if(errString.IndexOf("CompileXIB LaunchScreen-iPhone.xib") != -1)
                        return false;
                }
            }
            catch(System.Exception e)
            {
                UnityEngine.Debug.Log(e);

                return false;
            }

            return true;
        }
    }
}
