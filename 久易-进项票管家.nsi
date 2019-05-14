; �ýű�ʹ�� HM VNISEdit �ű��༭���򵼲���

; ��װ�����ʼ���峣��
!define PRODUCT_NAME "����-����Ʊ�ܼ�"
!define PRODUCT_VERSION "1.0.1.3"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\PRO_ReceiptsInvMgr.Client.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI �ִ����涨�� (1.67 �汾���ϼ���) ------
!include "MUI.nsh"

; MUI Ԥ���峣��
!define MUI_ABORTWARNING
!define MUI_ICON "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\ico.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; ��ӭҳ��
!insertmacro MUI_PAGE_WELCOME
; ��װĿ¼ѡ��ҳ��
!insertmacro MUI_PAGE_DIRECTORY
; ��װ����ҳ��
!insertmacro MUI_PAGE_INSTFILES
; ��װ���ҳ��
!define MUI_FINISHPAGE_RUN "$INSTDIR\PRO_ReceiptsInvMgr.Client.exe"
!insertmacro MUI_PAGE_FINISH

; ��װж�ع���ҳ��
!insertmacro MUI_UNPAGE_INSTFILES

; ��װ�����������������
!insertmacro MUI_LANGUAGE "SimpChinese"

; ��װԤ�ͷ��ļ�
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI �ִ����涨����� ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Setup.exe"
InstallDir "$PROGRAMFILES\����-����Ʊ�ܼ�"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUnInstDetails show
BrandingText " "

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite on
  File "E:\PRO_BJHX_EI_Green_PRO_PZBSOFT\PRO_ReceiptsInvMgr.Client\bin\Release\�û��ֲ�.pdf"
  CreateDirectory "$SMPROGRAMS\����-����Ʊ�ܼ�"
  CreateShortCut "$SMPROGRAMS\����-����Ʊ�ܼ�\����-����Ʊ�ܼ�.lnk" "$INSTDIR\PRO_ReceiptsInvMgr.Client.exe"
  CreateShortCut "$DESKTOP\����-����Ʊ�ܼ�.lnk" "$INSTDIR\PRO_ReceiptsInvMgr.Client.exe"
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
  CreateShortCut "$SMPROGRAMS\����-����Ʊ�ܼ�\Uninstall.lnk" "$INSTDIR\uninst.exe"
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
 *  �����ǰ�װ�����ж�ز���  *
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
  Delete "$INSTDIR\�û��ֲ�.pdf"
  Delete  "$INSTDIR\PRO_ReceiptsInvMgr.UpdateApp.exe"
  Delete "$INSTDIR\PRO_ReceiptsInvMgr.UpdateApp.exe.config"
  
  Delete "$SMPROGRAMS\����-����Ʊ�ܼ�\Uninstall.lnk"
  Delete "$DESKTOP\����-����Ʊ�ܼ�.lnk"
  Delete "$SMPROGRAMS\����-����Ʊ�ܼ�\����-����Ʊ�ܼ�.lnk"

  RMDir "$SMPROGRAMS\����-����Ʊ�ܼ�"

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd

#-- ���� NSIS �ű��༭�������� Function ���α�������� Section ����֮���д���Ա��ⰲװ�������δ��Ԥ֪�����⡣--#
Function .onInstSuccess
;	Delete "$INSTDIR\rjcf.df"
;	SetOutPath "$INSTDIR"
;	File /oname=rjcf.df rjcf.df 
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "��ȷʵҪ��ȫ�Ƴ� $(^Name) ���������е������" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) �ѳɹ��ش����ļ�����Ƴ���"
FunctionEnd
