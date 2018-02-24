#!/bin/sh

echo "At $(pwd)"
CURRENT_PWD=$(cd $(dirname $0); pwd)
cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD

##### Setting Param Begin

# ios, android, windows
target=ios

# main.sub.min
version=0.0.1

# projectCodeName
projectCodeName=zongzi

##### Setting Param End


##### Proc Input Param Begin

for a in $*
do
	r=`echo $a | sed "s/--//g"`
	echo $r
	eval $r
done

##### Proc Input Param End


# Src Begin

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

# Src End

branch=`git rev-parse --abbrev-ref HEAD`
reversion=`git rev-parse HEAD`

touch $verisonFile
echo "projectCodeName: $projectCodeName" > $verisonFile
echo "target: $target" >> $verisonFile
echo "version: $version" >> $verisonFile
echo "branch: $branch" >> $verisonFile
echo "reversion: $reversion" >> $verisonFile

srcAppFileSize=`ls -l $srcAppFilePath | awk '{print $5}'`

if [ x$APP_REPORT_DOMAIN == x ]; then
	echo 'APP_REPORT_DOMAIN is undefined'
	exit 1
fi

if [ ! -f "$srcAppFilePath" ]; then
	echo "$srcAppFilePath not exists, build failure?"
	exit 1
fi

postData="project_code=$projectCodeName&platform=$target&version=$version&commit_version=$reversion&size=$srcAppFileSize"
echo "Post Data[$postData]"

curl -d $postData $APP_REPORT_DOMAIN
echo ""

cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD
