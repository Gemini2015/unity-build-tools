#!/bin/sh

echo "At $(pwd)"
CURRENT_PWD=$(cd $(dirname $0); pwd)
cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD


# Unity程序路径
UNITY_PATH=/Applications/Unity/Unity.app/Contents/MacOS/Unity

##### Setting Param Begin

# ios, android, windows
target=ios

# main.sub.min
version=0.0.1

# appname
appname=Demo

# projectCodeName
projectCodeName=CC

# developer, distribution
codeSign=developer

# exportApk, exportAndroidProject
androidExportType=exportApk

# build
buildPath="$CURRENT_PWD"/../build

# unity project path
unityProjectPath="$CURRENT_PWD"/../project/DemoProject

# unity build log file
unityBuildLogFile="$CURRENT_PWD"/../log/build.log

##### Setting Param End


##### Proc Input Param Begin

for a in $*
do
	r=`echo $a | sed "s/--//g"`
	# echo $r
	eval $r
done

##### Proc Input Param End


# # Keychain Password
# # Store in Env KEYCHAIN_PASSWORD
# # Code Sign Keychain
# CODE_SIGN_KEYCHAIN=$HOME/Library/Keychains/login.keychain

# # Unlock Keychain
# echo "Unlock CodeSign Keychain"
# security unlock-keychain -p $KEYCHAIN_PASSWORD $CODE_SIGN_KEYCHAIN

$UNITY_PATH -quit -batchmode -projectPath $unityProjectPath -logFile $unityBuildLogFile -executeMethod CC.Release.ReleaseConsole.Release "target=$target" "version=$version" "buildPath=$buildPath" "codeSign=$codeSign" "androidExportType=$androidExportType" "appname=$appname" "projectCodeName=$projectCodeName"

cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD
