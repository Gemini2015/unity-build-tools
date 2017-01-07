#!/bin/sh

echo "At $(pwd)"
CURRENT_PWD=$(cd $(dirname $0); pwd)
cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD

##### Sub Bash Script Begin

MAKE_CLIENT=./makeClient.sh

##### Sub Bash Script End


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

##### Setting Param End


##### Proc Input Param Begin

for a in $*
do
	r=`echo $a | sed "s/--//g"`
	echo $r
	eval $r
done

##### Proc Input Param End

echo "$MAKE_CLIENT --appname=$appname --projectCodeName=$projectCodeName --buildPath=$buildPath --target=$target --version=$version --codeSign=$codeSign --androidExportType=$androidExportType"
$MAKE_CLIENT --appname=$appname --projectCodeName=$projectCodeName --buildPath=$buildPath --target=$target --version=$version --codeSign=$codeSign --androidExportType=$androidExportType

cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD
