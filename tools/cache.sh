#!/bin/sh

echo "At $(pwd)"
CURRENT_PWD=$(cd $(dirname $0); pwd)
cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD

# ios, android, windows
target=ios

# main.sub.min
version=0.0.1

# projectCodeName
projectCodeName=zongzi

for a in $*
do
	r=`echo $a | sed "s/--//g"`
	echo $r
	eval $r
done

# build
buildPath="$CURRENT_PWD"/../build

# target root
targetRoot=$buildPath/$target

# output
outputRoot=$targetRoot/output

# version file
verisonFile=$outputRoot/version.txt

srcVersionFilePath=$verisonFile

srcHotpatchFilePath="$outputRoot"/hotpatch.tar.gz

if [ "$target" == "ios" ]; then
	srcAppFilePath="$outputRoot"/"$projectCodeName".ipa
elif [ "$target" == "android" ]; then
	srcAppFilePath="$outputRoot"/"$projectCodeName".apk
else
	echo "Undefined Release Target"
	exit 1
fi

if [ ! -f "$srcAppFilePath" ]; then
	echo "$srcAppFilePath not exists, build failure?"
	exit 1
fi

##### Version Code Begin

echo "Target Version: $version"
versionList=(${version//./ })
versionListCount=${#versionList[*]}
if [[ $versionListCount -lt 3 ]]; then
    echo "Target Version Error: $version"
    exit 1
fi
versionCode=$(printf '%02d%02d%06d' ${versionList[0]} ${versionList[1]} ${versionList[2]})
echo "Version Code: $versionCode"

##### Version Code End

# default ~/Documents/appcache
versionRootPath=$HOME/Documents/appcache
if [ ! -d "$versionRootPath" ]; then
	echo "mkdir $versionRootPath"
	mkdir -p $versionRootPath
fi

# default ~/Documents/appcache/zongzi/0000000001/ios
versionTargetRootPath=$versionRootPath/$projectCodeName/$versionCode/$target

if [ -d "$versionTargetRootPath" ]; then
    rm -rf $versionTargetRootPath
fi
if [ ! -d "$versionTargetRootPath" ]; then
	echo "mkdir $versionTargetRootPath"
	mkdir -p $versionTargetRootPath
fi

cp $srcAppFilePath $versionTargetRootPath
cp $srcHotpatchFilePath $versionTargetRootPath
cp $srcVersionFilePath $versionTargetRootPath

cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD
