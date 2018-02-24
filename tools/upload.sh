#!/bin/sh

echo "At $(pwd)"
CURRENT_PWD=$(cd $(dirname $0); pwd)
cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD


##### Setting Param Begin

# ios, android, windows
target=ios

# projectCodeName
projectCodeName=zongzi

# 0 - test, 1 - distribution
ftpType='0'

##### Setting Param End

# build
buildPath="$CURRENT_PWD"/../build

# target root
targetRoot=$buildPath/$target

# output
outputRoot=$targetRoot/output

for a in $*
do
	r=`echo $a | sed "s/--//g"`
	echo $r
	eval $r
done


# Src Begin

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


# Generate MD5

appmd5=`md5 -q $srcAppFilePath`
hotpatchmd5=`md5 -q $srcHotpatchFilePath`
echo "app md5: $appmd5 " >> $verisonFile
echo "hotpatch md5: $hotpatchmd5" >> $verisonFile


# FTP Setting Begin

# test ftp server
Inter_FTP_Server=$DEVELOPMENT_FTP_IP
Inter_FTP_Port=$DEVELOPMENT_FTP_PORT
Inter_FTP_User=$DEVELOPMENT_FTP_USER
Inter_FTP_Password=$DEVELOPMENT_FTP_PASSWD
Inter_FTP_RootPath=/"$projectCodeName"/"$target"

# distribution ftp server
Outer_FTP_Server=$PRODUCTION_FTP_IP
Outer_FTP_Port=$PRODUCTION_FTP_PORT
Outer_FTP_User=$PRODUCTION_FTP_USER
Outer_FTP_Password=$PRODUCTION_FTP_PASSWD
Outer_FTP_RootPath=/"$projectCodeName"/"$target"

if [ $ftpType == '0' ]; then
	echo "Upload to inter network."
	FTP_SERVER=$Inter_FTP_Server
	FTP_PORT=$Inter_FTP_Port
	FTP_USER=$Inter_FTP_User
	FTP_PASSWORD=$Inter_FTP_Password
	FTP_ROOTPATH=$Inter_FTP_RootPath
else
	echo "Upload to outer network."
	FTP_SERVER=$Outer_FTP_Server
	FTP_PORT=$Outer_FTP_Port
	FTP_USER=$Outer_FTP_User
	FTP_PASSWORD=$Outer_FTP_Password
	FTP_ROOTPATH=$Outer_FTP_RootPath
fi

# FTP Setting End

# DEST Begin

destRoot=$FTP_ROOTPATH

destHotpatchFilePath=$destRoot/hotpatch.tar.gz

destVersionFilePath=$destRoot/version.txt

if [ "$target" == "ios" ]; then
	destAppFilePath="$destRoot"/"$projectCodeName".ipa
elif [ "$target" == "android" ]; then
	destAppFilePath="$destRoot"/"$projectCodeName".apk
else
	echo "Undefined Release Target"
	exit 1
fi

# DEST End

# Args Check

if [ x$FTP_SERVER == x ]; then
	echo "FTP server is null, setting error?"
	exit 1
fi

if [ ! -f "$srcAppFilePath" ]; then
	echo "$srcAppFilePath not exists, build failure?"
	exit 1
fi

# put $srcHotpatchFilePath $destHotpatchFilePath

echo "Begin Upload $srcAppFilePath to $FTP_SERVER:$destAppFilePath"

ftp -v -n <<- EOF
open $FTP_SERVER $FTP_PORT
user $FTP_USER $FTP_PASSWORD

binary

put $srcAppFilePath $destAppFilePath
put $srcVersionFilePath $destVersionFilePath

exit

EOF

echo "End Upload"

cd $CURRENT_PWD
echo "Enter "$CURRENT_PWD
