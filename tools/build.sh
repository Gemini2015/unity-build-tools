#!/bin/sh

echo "At $(pwd)"
CURRENT_PWD=$(cd $(dirname $0); pwd)
cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD

##### Sub Bash Script Begin

MAKE_CLIENT=./makeClient.sh

MAKE_HOTPATCH=./makeHotpatch.sh

UPLOAD=./upload.sh

CACHE=./cache.sh

REPORT=./report.sh

##### Sub Bash Script End


##### Setting Param Begin

# ios, android, windows
target=ios

# main.sub.min
version=0.0.1

# appname
appname=Demo

# projectCodeName
projectCodeName=zongzi

# developer, distribution
codeSign=developer

# exportApk, exportAndroidProject
androidExportType=exportApk

# internetdev, internetdis
buildConfig=internetdis

# 1
bundleVersionCode=1

# 0 - test, 1 - distribution
ftpType='0'

# enterprise, appstore
iOSExportType=enterprise

##### Setting Param End


##### Proc Input Param Begin

for a in $*
do
	r=`echo $a | sed "s/--//g"`
	echo $r
	eval $r
done

##### Proc Input Param End

# build
buildPath="$CURRENT_PWD"/../build/$target

# hotpatch
hotpatchRoot=$buildPath/hotpatch

# output
outputRoot=$buildPath/output

# version file
verisonFile=$outputRoot/version.txt


# Clean Begin

rm -rf $buildPath/*

# create directory
if [ ! -d "$buildPath" ]; then
    echo "mkdir $buildPath"
    mkdir -p $buildPath
fi
if [ ! -d "$outputRoot" ]; then
    echo "mkdir $outputRoot"
    mkdir -p $outputRoot
fi
if [ ! -d "$hotpatchRoot" ]; then
    echo "mkdir $hotpatchRoot"
    mkdir -p $hotpatchRoot
fi


# Clean End


branch=`git rev-parse --abbrev-ref HEAD`
reversion=`git rev-parse HEAD`

touch $verisonFile
echo "projectCodeName: $projectCodeName" > $verisonFile
echo "target: $target" >> $verisonFile
echo "version: $version" >> $verisonFile
echo "branch: $branch" >> $verisonFile
echo "reversion: $reversion" >> $verisonFile


echo "$MAKE_CLIENT --appname=$appname --projectCodeName=$projectCodeName --buildPath=$buildPath --hotpatch=$hotpatchRoot --target=$target --version=$version --codeSign=$codeSign --androidExportType=$androidExportType --buildConfig=$buildConfig --bundleVersionCode=$bundleVersionCode --iOSExportType=$iOSExportType"
$MAKE_CLIENT --appname=$appname --projectCodeName=$projectCodeName --buildPath=$buildPath --hotpatch=$hotpatchRoot --target=$target --version=$version --codeSign=$codeSign --androidExportType=$androidExportType --buildConfig=$buildConfig --bundleVersionCode=$bundleVersionCode --iOSExportType=$iOSExportType

echo "$MAKE_HOTPATCH"
$MAKE_HOTPATCH --target=$target --outputRoot=$outputRoot

echo "$UPLOAD"
$UPLOAD --projectCodeName=$projectCodeName --target=$target --outputRoot=$outputRoot --ftpType=$ftpType

echo "$CACHE --projectCodeName=$projectCodeName --target=$target --version=$version"
$CACHE --projectCodeName=$projectCodeName --target=$target --version=$version

echo "$REPORT --projectCodeName=$projectCodeName --target=$target --version=$version --outputRoot=$outputRoot"
$REPORT --projectCodeName=$projectCodeName --target=$target --version=$version --outputRoot=$outputRoot

cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD
