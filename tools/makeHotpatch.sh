#!/bin/sh

echo "At $(pwd)"
CURRENT_PWD=$(cd $(dirname $0); pwd)
cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD

##### Setting Param Begin

# ios, android, windows
target=ios

# build
buildPath="$CURRENT_PWD"/../build

# target root
targetRoot=$buildPath/$target

# output
outputRoot=$targetRoot/output

##### Setting Param End


##### Proc Input Param Begin

for a in $*
do
	r=`echo $a | sed "s/--//g"`
	echo $r
	eval $r
done

##### Proc Input Param End

releativeHotpatchRoot=./build/$target/hotpatch/

outputHotpatchTarFilePath=$outputRoot/hotpatch.tar.gz

echo "tar -cvf $outputHotpatchTarFilePath -C $CURRENT_PWD/.. $releativeHotpatchRoot"
tar -cvf $outputHotpatchTarFilePath -C $CURRENT_PWD/.. $releativeHotpatchRoot

cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD
