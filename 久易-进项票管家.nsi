; 该脚本使用 HM VNISEdit 脚本编辑器向导产生

; 安装程序初始定义常量
!define PRODUCT_NAME "久易-进项票管家"
!define PRODUCT_VERSION "1.0.1.3"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\PRO_ReceiptsInvMgr.Client.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_ICON "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\ico.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!define MUI_FINISHPAGE_RUN "$INSTDIR\PRO_ReceiptsInvMgr.Client.exe"
!insertmacro MUI_PAGE_FINISH

; 安装卸载过程页面
!insertmacro MUI_UNPAGE_INSTFILES

; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "SimpChinese"

; 安装预释放文件
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI 现代界面定义结束 ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Setup.exe"
InstallDir "$PROGRAMFILES\久易-进项票管家"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUnInstDetails show
BrandingText " "

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite on
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\用户手册.pdf"
  CreateDirectory "$SMPROGRAMS\久易-进项票管家"
  CreateShortCut "$SMPROGRAMS\久易-进项票管家\久易-进项票管家.lnk" "$INSTDIR\PRO_ReceiptsInvMgr.Client.exe"
  CreateShortCut "$DESKTOP\久易-进项票管家.lnk" "$INSTDIR\PRO_ReceiptsInvMgr.Client.exe"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\Util.Controls.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\System.Utility.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\System.Data.SQLite.xml"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\System.Data.SQLite.Linq.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\System.Data.SQLite.EF6.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\System.Data.SQLite.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\ReadAreaCode.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.Resources.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.Model.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.Logging.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.Domain.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.DAL.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.Core.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.Component.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.Client.exe.config"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.Client.exe"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.BLL.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.Application.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\Newtonsoft.Json.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\log4net.xml"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\log4net.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\libcef.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\JSDiskDLL.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\Token.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\config.xml"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\Area.xml"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\icudt.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\ICSharpCode.SharpZipLib.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\EntityFramework.xml"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\EntityFramework.SqlServer.xml"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\EntityFramework.SqlServer.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\EntityFramework.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\DocumentFormat.OpenXml.xml"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\DocumentFormat.OpenXml.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\Cryp_Ctl.ocx"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\cryp_api.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\CefSharp.Wpf.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\CefSharp.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\AutoMapper.xml"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\AutoMapper.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\AisinoJsbDll.dll"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.UpdateApp.exe"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\PRO_ReceiptsInvMgr.UpdateApp.exe.config"
  SetOutPath "$INSTDIR\x64"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\x64\SQLite.Interop.dll"
  SetOutPath "$INSTDIR\x86"
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\x86\SQLite.Interop.dll"
	SetOutPath "$INSTDIR\Driver"
	 File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\Driver\base_driver.exe"
	 File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\Driver\jspDriver1.1.1.12.exe"
	 File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\Driver\nisecinstaller_V1.0.8.7.1.exe"
	SetOutPath "$INSTDIR"
SectionEnd

Section -AdditionalIcons
  CreateShortCut "$SMPROGRAMS\久易-进项票管家\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\PRO_ReceiptsInvMgr.Client.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\PRO_ReceiptsInvMgr.Client.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
SectionEnd

/******************************
 *  以下是安装程序的卸载部分  *
 ******************************/

Section Uninstall
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\SQLite.Interop.dll"
  Delete "$INSTDIR\SQLite.Interop.dll"
  Delete "$INSTDIR\AisinoJsbDll.dll"
  Delete "$INSTDIR\AutoMapper.dll"
  Delete "$INSTDIR\AutoMapper.xml"
  Delete "$INSTDIR\CefSharp.dll"
  Delete "$INSTDIR\CefSharp.Wpf.dll"
  Delete "$INSTDIR\cryp_api.dll"
  Delete "$INSTDIR\Cryp_Ctl.ocx"
  Delete "$INSTDIR\DocumentFormat.OpenXml.dll"
  Delete "$INSTDIR\DocumentFormat.OpenXml.xml"
  Delete "$INSTDIR\EntityFramework.dll"
  Delete "$INSTDIR\EntityFramework.SqlServer.dll"
  Delete "$INSTDIR\EntityFramework.SqlServer.xml"
  Delete "$INSTDIR\EntityFramework.xml"
  Delete "$INSTDIR\ICSharpCode.SharpZipLib.dll"
  Delete "$INSTDIR\icudt.dll"
  Delete "$INSTDIR\JSDiskDLL.dll"
  Delete "$INSTDIR\libcef.dll"
  Delete "$INSTDIR\log4net.dll"
  Delete "$INSTDIR\log4net.xml"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.Application.dll"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.BLL.dll"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.Client.exe"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.Client.exe.config"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.Component.dll"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.Core.dll"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.DAL.dll"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.Domain.dll"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.Logging.dll"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.Model.dll"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.Resources.dll"
  Delete "$INSTDIR\ReadAreaCode.dll"
  Delete "$INSTDIR\System.Data.SQLite.dll"
  Delete "$INSTDIR\System.Data.SQLite.EF6.dll"
  Delete "$INSTDIR\System.Data.SQLite.Linq.dll"
  Delete "$INSTDIR\System.Data.SQLite.xml"
  Delete "$INSTDIR\System.Utility.dll"
  Delete "$INSTDIR\Util.Controls.dll"
  Delete "$INSTDIR\用户手册.pdf"
  Delete  "$INSTDIR\PRO_ReceiptsInvMgr.UpdateApp.exe"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.UpdateApp.exe.config"
  
  Delete "$SMPROGRAMS\久易-进项票管家\Uninstall.lnk"
  Delete "$DESKTOP\久易-进项票管家.lnk"
  Delete "$SMPROGRAMS\久易-进项票管家\久易-进项票管家.lnk"

  RMDir "$SMPROGRAMS\久易-进项票管家"

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#
Function .onInstSuccess
;	Delete "$INSTDIR\rjcf.df"
;	SetOutPath "$INSTDIR"
;	File /oname=rjcf.df rjcf.df 
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "您确实要完全移除 $(^Name) ，及其所有的组件？" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从您的计算机移除。"
FunctionEnd
