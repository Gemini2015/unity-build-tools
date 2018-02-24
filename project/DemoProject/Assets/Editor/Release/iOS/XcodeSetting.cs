using CC.Release.iOS.Xcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CC.Release.iOS
{
    internal class XcodeSetting
    {
        public static void Setup(string pathToBuiltProject)
        {
            string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            string targetName = PBXProject.GetUnityTargetName();

            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string targetGuid = proj.TargetGuidByName(targetName);

            AddLibs(targetGuid, proj);

            AddFrameworks(targetGuid, proj);

            AddFiles(targetGuid, proj);

            AddLinkerFlags(targetGuid, proj);

            proj.WriteToFile(projPath);

            UpdateInfoPlist(pathToBuiltProject);

            AddCapability(projPath);
        }

        private static void AddCapability(string projPath)
        {
            string targetName = PBXProject.GetUnityTargetName();
            var postfix = ReleaseConfig.iOS.GetValue(ReleaseConfig.iOS.KeyDefine.BundlePostfix);
			ProjectCapabilityManager capManager = new ProjectCapabilityManager(projPath, postfix + ".entitlements", targetName);

            capManager.AddBackgroundModes(BackgroundModesOptions.BackgroundFetch | BackgroundModesOptions.RemoteNotifications);
            capManager.AddPushNotifications(true);

            capManager.AddInAppPurchase();

            capManager.WriteToFile();
        }

		private static void UpdateInfoPlist(string pathToBuiltProject)
        {
			var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            // CFBundleURLTypes
            var array = plist.root.CreateArray("CFBundleURLTypes");

            var urlDict = array.AddDict();
            urlDict.SetString("CFBundleTypeRole", "Editor");
            urlDict.SetString("CFBundleURLName", "weixin");
            var urlInnerArray = urlDict.CreateArray("CFBundleURLSchemes");
			urlInnerArray.AddString("wx123456789");

            urlDict = array.AddDict();
            urlDict.SetString("CFBundleTypeRole", "Editor");
            urlDict.SetString("CFBundleURLName", "mw");
            urlInnerArray = urlDict.CreateArray("CFBundleURLSchemes");
            urlInnerArray.AddString("magicwindow");

            // CFBundleURLTypes
            array = plist.root.CreateArray("LSApplicationQueriesSchemes");
            array.AddString("weixin");
            array.AddString("mw");

            plist.root.SetString("NSPhotoLibraryUsageDescription", "此 App 需要您的同意才能读取媒体资料库");
            plist.root.SetString("NSMicrophoneUsageDescription", "此 App 需要您的同意才能录制语音");
            plist.root.SetString("NSCameraUsageDescription", "此 App 需要您的同意才能开启摄像头");
            plist.root.SetString("NSLocationWhenInUseUsageDescription", "我们需要使用您的位置来查找周围好友");

            plist.WriteToFile(plistPath);
        }

        private static void AddLinkerFlags(string targetGuid, PBXProject proj)
        {
			proj.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");

            proj.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
			proj.SetBuildProperty(targetGuid, "GCC_ENABLE_CPP_EXCEPTIONS", "YES");
            proj.SetBuildProperty(targetGuid, "GCC_ENABLE_CPP_RTTI", "YES");
            proj.SetBuildProperty(targetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");


			if(ReleaseConfig.Setting[ReleaseConfig.SettingDefine.IOSCodeSign] == ReleaseConfig.iOSCodeSign.Developer)
			{
                var provision = ReleaseConfig.iOS.GetValue(ReleaseConfig.iOS.KeyDefine.DevelopmentProvision);
                var cert = ReleaseConfig.iOS.GetValue(ReleaseConfig.iOS.KeyDefine.DevelopmentCert);
                proj.SetBuildProperty(targetGuid, "PROVISIONING_PROFILE_SPECIFIER", provision);
				proj.SetBuildProperty(targetGuid, "CODE_SIGN_IDENTITY", cert);
			}
			else
			{
                var provision = ReleaseConfig.iOS.GetValue(ReleaseConfig.iOS.KeyDefine.DistributionProvision);
                var cert = ReleaseConfig.iOS.GetValue(ReleaseConfig.iOS.KeyDefine.DistributionCert);
                proj.SetBuildProperty(targetGuid, "PROVISIONING_PROFILE_SPECIFIER", provision);
				proj.SetBuildProperty(targetGuid, "CODE_SIGN_IDENTITY", cert);
			}

            var teamID = ReleaseConfig.iOS.GetValue(ReleaseConfig.iOS.KeyDefine.DevelopmentTeam);
            proj.SetTeamId(targetGuid, teamID);

        }

        private static void AddFiles(string targetGuid, PBXProject proj)
        {
            //var filePath = Path.Combine(Application.dataPath, "../SDKFile/GCloudVoice.bundle");
            //var file = proj.AddFile(filePath, "MagicWindowSDK/GCloudVoice.bundle", PBXSourceTree.Source);
            //proj.AddFileToBuild(targetGuid, file);

            //filePath = Path.Combine(Application.dataPath, "../SDKFile/MagicWindow.bundle");
            //file = proj.AddFile(filePath, "MagicWindowSDK/MagicWindow.bundle", PBXSourceTree.Source);
            //proj.AddFileToBuild(targetGuid, file);

            //var shareIcon = Path.Combine(Application.dataPath, "../SDKFile/shareicon.png");
            //var shareIconFile = proj.AddFile(shareIcon, "MagicWindowSDK/shareicon.png", PBXSourceTree.Source);
            //proj.AddFileToBuild(targetGuid, shareIconFile);
        }

        private static void AddFrameworks(string targetGuid, PBXProject proj)
        {
            proj.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);
            proj.AddFrameworkToProject(targetGuid, "SystemConfiguration.framework", false);
            proj.AddFrameworkToProject(targetGuid, "Security.framework", false);
            proj.AddFrameworkToProject(targetGuid, "CFNetwork.framework", false);
            proj.AddFrameworkToProject(targetGuid, "AdSupport.framework", false);
            proj.AddFrameworkToProject(targetGuid, "CoreGraphics.framework", false);
            proj.AddFrameworkToProject(targetGuid, "CoreFoundation.framework", false);
            proj.AddFrameworkToProject(targetGuid, "CoreLocation.framework", false);
            proj.AddFrameworkToProject(targetGuid, "WebKit.framework", false);
            proj.AddFrameworkToProject(targetGuid, "ImageIO.framework", false);
            proj.AddFrameworkToProject(targetGuid, "AVFoundation.framework", false);
            proj.AddFrameworkToProject(targetGuid, "CoreAudio.framework", false);
            proj.AddFrameworkToProject(targetGuid, "AudioToolbox.framework", false);
            proj.AddFrameworkToProject(targetGuid, "JavaScriptCore.framework", false);
            proj.AddFrameworkToProject(targetGuid, "StoreKit.framework", false);
            proj.AddFrameworkToProject(targetGuid, "MobileCoreServices.framework", false);
        }

        private static void AddLibs(string targetGuid, PBXProject proj)
        {
            proj.AddFrameworkToProject(targetGuid, "libz.tbd", false);
            proj.AddFrameworkToProject(targetGuid, "libsqlite3.0.dylib", false);
            proj.AddFrameworkToProject(targetGuid, "libc++.dylib", false);
            proj.AddFrameworkToProject(targetGuid, "libstdc++.6.0.9.tbd", false);
        }
    }
}
