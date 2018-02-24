# unity-build-tools

Unity打包框架。可以通过命令行或编辑器一键生成ipa或apk。


## 简介

*	编辑器：点击菜单栏**Release/ReleaseWindow**。修改参数，点击**Release**。
*	命令行：Mac系统下，cd到**tools**目录下，执行`./build --target=ios`。

详细打包参数，参考`ReleaseConfig.SettingDefine`，或是**build.sh**中的`Setting Param`。


## 设置

*	全局搜索`zongzi`，修改为自己项目的设置。
*	修改`ReleaseConfig`里面针对iOS或Android的签名设置。
*	Mac机器上要导入对应的证书，描述文件。（iOS）
*	Mac机器上设置好Android开发环境。（Android）
*	命令行增加以下环境变量：
	*	KEYCHAIN_PASSWORD：解锁钥匙串密码。
	*	APP_REPORT_DOMAIN：包体大小统计api地址。若不需要包体大小统计，可以在**build.sh**脚本中，删掉对应命令。
	*	开发ftp信息：详见**upload.sh**
	*	生产ftp信息：详见**upload.sh**


## Tips

*	部分脚本命令针对git作为版本工具，可以轻松改为svn相关命令。
*	版本号规则固定为`xx.xx.xx`。
*	每次打包结果都会缓存，具体参考**cache.sh**。