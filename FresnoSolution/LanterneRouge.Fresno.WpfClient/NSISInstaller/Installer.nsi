; Fresno installation script
; Written by AG based on
; FileZilla script Written by Tim Kosse <mailto:tim.kosse@filezilla-project.org>
;
; Requires NSIS >= 3.0b3

;--------------------------------
; Build environment
;--------------------------------

Unicode true
  !define top_srcdir    @top_srcdir@
  !define srcdir        @srcdir@
  !define VERSION       @PACKAGE_VERSION@
  !define VERSION_MAJOR @PACKAGE_VERSION_MAJOR@
  !define VERSION_MINOR @PACKAGE_VERSION_MINOR@
  !define VERSION_FULL  @PACKAGE_VERSION_MAJOR@.@PACKAGE_VERSION_MINOR@.@PACKAGE_VERSION_MICRO@.@PACKAGE_VERSION_NANO@
  !define PUBLISHER     "Annar Gaustad"
  !define WEBSITE_URL   "http://lanternerouge.no/"
  !addplugindir         @srcdir@
  !define INSTALLER_REGKEY "Software\Step Test Assistant"
  !define UNINSTALLER_REGKEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\Step Test Assistant"

;--------------------------------
; General
;--------------------------------

  ; Name and file
  Name "Step Test Assistant ${VERSION}"
  OutFile "STA_Setup.exe"

  SetCompressor /SOLID LZMA

  ; Default installation folder
  !if "@NSIS_64BIT@" == "1"
    InstallDir "$PROGRAMFILES64\Step Test Assistant"
  !else
    InstallDir "$PROGRAMFILES\Step Test Assistant"
  !endif

  ; Get installation folder from registry if available
  InstallDirRegKey HKLM "${INSTALLER_REGKEY}" ""

  RequestExecutionLevel user

  !define DUMP_KEY "SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps"

;--------------------------------
; Libtool executable target path
;--------------------------------

  !include libtoolexecutablesubdir.nsh
  !ifndef LT_EXEDIR
    !define LT_EXEDIR ""
  !endif

;--------------------------------
; Include Modern UI and functions
;--------------------------------

  !include "MUI2.nsh"
  !include "WordFunc.nsh"
  !include "Library.nsh"
  !include "WinVer.nsh"
  !include "FileFunc.nsh"
  !include "Memento.nsh"
  !include "StrFunc.nsh"
  !include "@srcdir@\process_running.nsh"
  !include "@srcdir@\UAC.nsh"

  ${StrRep}

;--------------------------------
; Installer's VersionInfo
;--------------------------------

  VIProductVersion                   "${VERSION_FULL}"
  VIAddVersionKey "CompanyName"      "${PUBLISHER}"
  VIAddVersionKey "ProductName"      "Step Test Assistant"
  VIAddVersionKey "ProductVersion"   "${VERSION}"
  VIAddVersionKey "FileDescription"  "Step Test Assistant Client"
  VIAddVersionKey "FileVersion"      "${VERSION}"
  VIAddVersionKey "LegalCopyright"   "${PUBLISHER}"
  VIAddVersionKey "OriginalFilename" "StepTestAssistant_${VERSION}_win32-setup.exe"

;--------------------------------
; Required functions
;--------------------------------

  !insertmacro GetParameters
  !insertmacro GetOptions
  !insertmacro un.GetParameters
  !insertmacro un.GetOptions

;--------------------------------
; Variables
;--------------------------------

  Var MUI_TEMP
  Var STARTMENU_FOLDER
  Var PREVIOUS_INSTALLDIR
  Var PREVIOUS_VERSION
  Var PREVIOUS_VERSION_STATE
  Var REINSTALL_UNINSTALL
  Var REINSTALL_UNINSTALLBUTTON
  Var ALL_USERS_DEFAULT
  Var ALL_USERS
  Var ALL_USERS_BUTTON
  Var IS_ADMIN
  Var USERNAME
  Var PERFORM_UPDATE
  Var SKIPLICENSE
  Var SKIPUAC
  Var GetInstalledSize.total
  Var OldRunDir
  Var CommandLine
  Var Quiet

;--------------------------------
; Interface Settings
;--------------------------------

  !define MUI_ICON "${top_srcdir}/Resources/icons8-glucometer-32.ico"
  !define MUI_UNICON "${srcdir}/icons8-glucometer-32.ico"

  !define MUI_ABORTWARNING

;--------------------------------
; Memento settings
;--------------------------------

!define MEMENTO_REGISTRY_ROOT SHELL_CONTEXT
!define MEMENTO_REGISTRY_KEY "${INSTALLER_REGKEY}"

;--------------------------------
; Pages
;--------------------------------

  !define MUI_PAGE_CUSTOMFUNCTION_PRE PageLicensePre
  !insertmacro MUI_PAGE_LICENSE "${top_srcdir}\COPYING"

!ifdef ENABLE_OFFERS
  !include "@srcdir@\offer.nsh"
  Page custom OfferInitPage
  Page custom OfferPage OfferPageLeave
!endif

  Page custom PageReinstall PageLeaveReinstall
  Page custom PageAllUsers PageLeaveAllUsers

  !define MUI_PAGE_CUSTOMFUNCTION_PRE PageComponentsPre
  !insertmacro MUI_PAGE_COMPONENTS

  !define MUI_PAGE_CUSTOMFUNCTION_PRE PageDirectoryPre
  !insertmacro MUI_PAGE_DIRECTORY

  ; Start Menu Folder Page Configuration
  !define MUI_STARTMENUPAGE_REGISTRY_ROOT "SHCTX"
  !define MUI_STARTMENUPAGE_REGISTRY_KEY "${INSTALLER_REGKEY}"
  !define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Startmenu"
  !define MUI_STARTMENUPAGE_DEFAULTFOLDER "Step Test Assistant"

  !define MUI_PAGE_CUSTOMFUNCTION_PRE PageStartmenuPre
  !insertmacro MUI_PAGE_STARTMENU Application $STARTMENU_FOLDER

  !define MUI_PAGE_CUSTOMFUNCTION_LEAVE PostInstPage
  !insertmacro MUI_PAGE_INSTFILES

  !define MUI_FINISHPAGE_RUN
  !define MUI_FINISHPAGE_RUN_FUNCTION CustomFinishRun
  !define MUI_FINISHPAGE_RUN_TEXT "&Start Step Test Assistant now"
  !define MUI_PAGE_CUSTOMFUNCTION_SHOW PageFinishPre
  !insertmacro MUI_PAGE_FINISH

  !define MUI_PAGE_CUSTOMFUNCTION_PRE un.ConfirmPagePre
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  !define MUI_PAGE_CUSTOMFUNCTION_PRE un.FinishPagePre
  !insertmacro MUI_UNPAGE_FINISH

Function GetUserInfo
  ClearErrors
  UserInfo::GetName
  ${If} ${Errors}
    StrCpy $IS_ADMIN 1
    Return
  ${EndIf}
  Pop $USERNAME

  ${If} ${UAC_IsAdmin}
    StrCpy $IS_ADMIN 1
  ${Else}
    StrCpy $IS_ADMIN 0
  ${EndIf}

FunctionEnd

Function UpdateShellVarContext

  ${If} $ALL_USERS == 1
    SetShellVarContext all
    DetailPrint "Installing for all users"
  ${Else}
    SetShellVarContext current
    DetailPrint "Installing for current user"
  ${EndIf}

FunctionEnd

!Macro MessageBoxImpl options text sdreturn checks
  ${If} $Quiet == 1
    MessageBox ${options} `${text}` ${checks}
  ${Else}
    MessageBox ${options} `${text}` /SD ${sdreturn} ${checks}
  ${Endif}

!MacroEnd

!define MessageBox `!insertmacro MessageBoxImpl`

Function ReadAllUsersCommandline

  ${GetOptions} $CommandLine "/user" $R1

  ${Unless} ${Errors}
    ${If} $R1 == "current"
    ${OrIf} $R1 == "=current"
      StrCpy $ALL_USERS 0
    ${ElseIf} $R1 == "all"
    ${OrIf} $R1 == "=all"
      StrCpy $ALL_USERS 1
    ${Else}
      ${MessageBox} MB_ICONSTOP "Invalid option for /user. Has to be either /user=all or /user=current" IDOK ''
      Abort
    ${EndIf}
  ${EndUnless}
  Call UpdateShellVarContext

FunctionEnd

Function CheckPrevInstallDirExists

  ${If} $PREVIOUS_INSTALLDIR != ""

    ; Make sure directory is valid
    Push $R0
    Push $R1
    StrCpy $R0 "$PREVIOUS_INSTALLDIR" "" -1
    ${If} $R0 == '\'
    ${OrIf} $R0 == '/'
      StrCpy $R0 $PREVIOUS_INSTALLDIR*.*
    ${Else}
      StrCpy $R0 $PREVIOUS_INSTALLDIR\*.*
    ${EndIf}
    ${IfNot} ${FileExists} $R0
      StrCpy $PREVIOUS_INSTALLDIR ""
    ${EndIf}
    Pop $R1
    Pop $R0

  ${EndIf}

FunctionEnd

Function ReadPreviousVersion

  ReadRegStr $PREVIOUS_INSTALLDIR HKLM "${INSTALLER_REGKEY}" ""

  Call CheckPrevInstallDirExists

  ${If} $PREVIOUS_INSTALLDIR != ""
    ; Detect version
    ReadRegStr $PREVIOUS_VERSION HKLM "${INSTALLER_REGKEY}" "Version"
    ${If} $PREVIOUS_VERSION != ""
      StrCpy $ALL_USERS 1
      SetShellVarContext all
      return
    ${EndIf}
  ${EndIf}

  ReadRegStr $PREVIOUS_INSTALLDIR HKCU "${INSTALLER_REGKEY}" ""

  Call CheckPrevInstallDirExists

  ${If} $PREVIOUS_INSTALLDIR != ""
    ; Detect version
    ReadRegStr $PREVIOUS_VERSION HKCU "${INSTALLER_REGKEY}" "Version"
    ${If} $PREVIOUS_VERSION != ""
      StrCpy $ALL_USERS 0
      SetShellVarContext current
      return
    ${EndIf}
  ${EndIf}

FunctionEnd

Function LoadPreviousSettings

  ; Component selection
  ${MementoSectionRestore}

  ; Startmenu
  !define ID "Application"

  !ifdef MUI_STARTMENUPAGE_${ID}_REGISTRY_ROOT & MUI_STARTMENUPAGE_${ID}_REGISTRY_KEY & MUI_STARTMENUPAGE_${ID}_REGISTRY_VALUENAME

    ReadRegStr $mui.StartMenuPage.RegistryLocation "${MUI_STARTMENUPAGE_${ID}_REGISTRY_ROOT}" "${MUI_STARTMENUPAGE_${ID}_REGISTRY_KEY}" "${MUI_STARTMENUPAGE_${ID}_REGISTRY_VALUENAME}"

    ${if} $mui.StartMenuPage.RegistryLocation != ""
      StrCpy "$STARTMENU_FOLDER" $mui.StartMenuPage.RegistryLocation
    ${else}
      StrCpy "$STARTMENU_FOLDER" ""
    ${endif}

    !undef ID

  !endif

  ${If} $PREVIOUS_INSTALLDIR != ""
    StrCpy $INSTDIR $PREVIOUS_INSTALLDIR
  ${EndIf}

FunctionEnd

Function ReadUpdateCommandline

  ${GetOptions} $CommandLine "/update" $R1

  ${If} ${Errors}
    StrCpy $PERFORM_UPDATE 0
  ${Else}
    StrCpy $PERFORM_UPDATE 1
  ${EndIf}

FunctionEnd

Function ReadSkipLicense

  ${GetOptions} $CommandLine "/skiplicense" $R1

  ${If} ${Errors}
    StrCpy $SKIPLICENSE 0
  ${Else}
    StrCpy $SKIPLICENSE 1
  ${EndIf}

FunctionEnd

Function ReadSkipUAC

  ${GetOptions} $CommandLine "/skipuac" $R1

  ${If} ${Errors}
    StrCpy $SKIPUAC 0
  ${Else}
    StrCpy $SKIPUAC 1
  ${EndIf}

FunctionEnd

Function ReadQuiet

  ${GetOptions} $CommandLine "/quiet" $R1

  ${If} ${Errors}
    StrCpy $Quiet 0
  ${Else}
    StrCpy $Quiet 1
    SetSilent silent
  ${EndIf}

FunctionEnd

Function .onInit

  ; The very first thing we should do: See if there's still a running STA, we use it to detect
  ; cases were users have installed multiple copies installed in different directories.
  Push "StepTestAssistant.exe"
  Call IsProcessRunning
  Pop $OldRunDir

  ; Store command line
  ${GetParameters} $CommandLine

  Call ReadQuiet

  ${Unless} ${AtLeastWin7}
    ${MessageBox} MB_YESNO|MB_ICONSTOP "Unsupported operating system.$\Step Test Assistant ${VERSION} requires at least Windows 7 and may not work correctly on your system.$\nDo you really want to continue with the installation?" IDNO 'IDYES installonoldwindows'
    Abort
installonoldwindows:
  ${EndUnless}

  !if "@NSIS_64BIT@" == "1"
    ${Unless} ${RunningX64}
      ${MessageBox} MB_YESNO|MB_ICONSTOP "Unsupported operating system.$\nThis is the installer for the 64bit version of Step Test Assistant ${VERSION} and does not run on your operating system which is only 32bit.$\nPlease download the 32bit Step Test Assistant installer instead.$\nDo you really want to continue with the installation?" IDNO 'IDYES install64on32'
      Abort
install64on32:
    ${EndUnless}
  !endif

  Call ReadSkipUAc

  ${If} $SKIPUAC != 1
    !insertmacro UAC_RunElevated

    ${Switch} $0
    ${Case} 0
      ${IfThen} $1 = 1 ${|} Quit ${|} ;we are the outer process, the inner process has done its work, we are done.

      ; Ignore success or failure here. Correct privileges are checked later on.
      ${Break}
    ${Case} 1223
      ; User aborted elevation, continue regardless
      ${Break}
    ${Default}
      ${MessageBox} mb_iconstop "Could not elevate process (errorcode $0), continuing with normal user privileges." IDOK ''
      ${Break}
    ${EndSwitch}

    ; The UAC plugin changes the error level even in the inner process, reset it.
    SetErrorLevel -1
  ${EndIf}

  ; /update argument
  Call ReadUpdateCommandline

  Call ReadSkipLicense

  Call GetUserInfo

  ; Initialize $ALL_USERS with default value
  ${If} $IS_ADMIN == 1
    StrCpy $ALL_USERS 1
  ${Else}
    StrCpy $ALL_USERS 0
  ${EndIf}
  Call UpdateShellVarContext

  ; See if previous version exists
  ; This can change ALL_USERS
  Call ReadPreviousVersion

  ${If} $PREVIOUS_VERSION != ""
    StrCpy $REINSTALL_UNINSTALL 1
  ${EndIf}

  ; Load _all_ previous settings.
  ; Need to do it now as up to now, $ALL_USERS was possibly reflecting a
  ; previous installation. After this call, $ALL_USERS reflects the requested
  ; installation mode for this installation.
  Call LoadPreviousSettings

  Call ReadAllUsersCommandline

  ${If} $ALL_USERS == 1
    ${If} $IS_ADMIN == 0

      ${If} $PREVIOUS_VERSION != ""
        ${MessageBox} MB_ICONSTOP "Step Test Assistant has been previously installed for all users.$\nPlease restart the installer with Administrator privileges." IDOK ''
        Abort
      ${Else}
        ${MessageBox} MB_ICONSTOP "Cannot install for all users.$\nPlease restart the installer with Administrator privileges." IDOK ''
        Abort
      ${EndIf}
    ${EndIf}
  ${EndIf}

  ${If} $PREVIOUS_VERSION == ""

    StrCpy $PERFORM_UPDATE 0
    DetailPrint "No previous version of Step Test Assistant was found"

  ${Else}

    Push "${VERSION}"
    Push $PREVIOUS_VERSION
    Call STAVersionCompare

    DetailPrint "Found previous version: $PREVIOUS_VERSION"
    DetailPrint "Installing $PREVIOUS_VERSION_STATE version ${VERSION}"

  ${EndIf}

  StrCpy $ALL_USERS_DEFAULT $ALL_USERS

FunctionEnd

Function STAVersionCompare

  Exch $1
  Exch
  Exch $0

  Push $2
  Push $3
  Push $4

versioncomparebegin:
  ${If} $0 == ""
  ${AndIf} $1 == ""
    StrCpy $PREVIOUS_VERSION_STATE "same"
    goto versioncomparedone
  ${EndIf}

  StrCpy $2 0
  StrCpy $3 0

  ; Parse rc / beta suffixes for segments
  StrCpy $4 $0 2
  ${If} $4 == "rc"
    StrCpy $2 100
    StrCpy $0 $0 "" 2
  ${Else}
    StrCpy $4 $0 4
    ${If} $4 == "beta"
      StrCpy $0 $0 "" 4
    ${Else}
      StrCpy $2 10000
    ${EndIf}
  ${EndIf}

  StrCpy $4 $1 2
  ${If} $4 == "rc"
    StrCpy $3 100
    StrCpy $1 $1 "" 2
  ${Else}
    StrCpy $4 $1 4
    ${If} $4 == "beta"
      StrCpy $1 $1 "" 4
    ${Else}
      StrCpy $3 10000
    ${EndIf}
  ${EndIf}

split1loop:

  StrCmp $0 "" split1loopdone
  StrCpy $4 $0 1
  StrCpy $0 $0 "" 1
  StrCmp $4 "." split1loopdone
  StrCmp $4 "-" split1loopdone
  StrCpy $2 $2$4
  goto split1loop
split1loopdone:

split2loop:

  StrCmp $1 "" split2loopdone
  StrCpy $4 $1 1
  StrCpy $1 $1 "" 1
  StrCmp $4 "." split2loopdone
  StrCmp $4 "-" split2loopdone
  StrCpy $3 $3$4
  goto split2loop
split2loopdone:

  ${If} $2 > $3
    StrCpy $PREVIOUS_VERSION_STATE "newer"
  ${ElseIf} $3 > $2
    StrCpy $PREVIOUS_VERSION_STATE "older"
  ${Else}
    goto versioncomparebegin
  ${EndIf}


versioncomparedone:

  Pop $4
  Pop $3
  Pop $2
  Pop $1
  Pop $0

FunctionEnd

Function PageLicensePre

  ${If} $PERFORM_UPDATE == 1
    Abort
  ${Elseif} $SKIPLICENSE == 1
    Abort
  ${EndIf}
FunctionEnd

Function PageReinstall

  ${If} $PREVIOUS_VERSION == ""
    Abort
  ${EndIf}

  ${If} $PERFORM_UPDATE == 1
    Abort
  ${EndIf}

  nsDialogs::Create /NOUNLOAD 1018
  Pop $0

  ${If} $PREVIOUS_VERSION_STATE == "newer"

    !insertmacro MUI_HEADER_TEXT "Already Installed" "Choose how you want to install Step Test Assistant."
    nsDialogs::CreateItem /NOUNLOAD STATIC ${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS} 0 0 0 100% 40 "An older version of Step Test Assistant is installed on your system. Select the operation you want to perform and click Next to continue."
    Pop $R0
    nsDialogs::CreateItem /NOUNLOAD BUTTON ${BS_AUTORADIOBUTTON}|${BS_VCENTER}|${BS_MULTILINE}|${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS}|${WS_GROUP}|${WS_TABSTOP} 0 10 55 100% 30 "Upgrade Step Test Assistant using previous settings (recommended)"
    Pop $REINSTALL_UNINSTALLBUTTON
    nsDialogs::CreateItem /NOUNLOAD BUTTON ${BS_AUTORADIOBUTTON}|${BS_TOP}|${BS_MULTILINE}|${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS} 0 10 85 100% 50 "Change settings (advanced)"
    Pop $R0

    ${If} $REINSTALL_UNINSTALL == ""
      StrCpy $REINSTALL_UNINSTALL 1
    ${EndIf}

  ${ElseIf} $PREVIOUS_VERSION_STATE == "older"

    !insertmacro MUI_HEADER_TEXT "Already Installed" "Choose how you want to install Step Test Assistant."
    nsDialogs::CreateItem /NOUNLOAD STATIC ${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS} 0 0 0 100% 40 "A newer version of Step Test Assistant is already installed! It is not recommended that you downgrade to an older version. Select the operation you want to perform and click Next to continue."
    Pop $R0
    nsDialogs::CreateItem /NOUNLOAD BUTTON ${BS_AUTORADIOBUTTON}|${BS_VCENTER}|${BS_MULTILINE}|${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS}|${WS_GROUP}|${WS_TABSTOP} 0 10 55 100% 30 "Downgrade Step Test Assistant using previous settings (recommended)"
    Pop $REINSTALL_UNINSTALLBUTTON
    nsDialogs::CreateItem /NOUNLOAD BUTTON ${BS_AUTORADIOBUTTON}|${BS_TOP}|${BS_MULTILINE}|${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS} 0 10 85 100% 50 "Change settings (advanced)"
    Pop $R0

    ${If} $REINSTALL_UNINSTALL == ""
      StrCpy $REINSTALL_UNINSTALL 1
    ${EndIf}

  ${ElseIf} $PREVIOUS_VERSION_STATE == "same"

    !insertmacro MUI_HEADER_TEXT "Already Installed" "Choose the maintenance option to perform."
    nsDialogs::CreateItem /NOUNLOAD STATIC ${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS} 0 0 0 100% 40 "Step Test Assistant ${VERSION} is already installed. Select the operation you want to perform and click Next to continue."
    Pop $R0
    nsDialogs::CreateItem /NOUNLOAD BUTTON ${BS_AUTORADIOBUTTON}|${BS_VCENTER}|${BS_MULTILINE}|${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS}|${WS_GROUP}|${WS_TABSTOP} 0 10 55 100% 30 "Add/Remove/Reinstall components"
    Pop $REINSTALL_UNINSTALLBUTTON
    nsDialogs::CreateItem /NOUNLOAD BUTTON ${BS_AUTORADIOBUTTON}|${BS_TOP}|${BS_MULTILINE}|${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS} 0 10 85 100% 50 "Uninstall Step Test Assistant"
    Pop $R0

    ${If} $REINSTALL_UNINSTALL == ""
      StrCpy $REINSTALL_UNINSTALL 1
    ${EndIf}

  ${Else}

    ${MessageBox} MB_ICONSTOP "Unknown value of PREVIOUS_VERSION_STATE, aborting" IDOK ''
    Abort

  ${EndIf}

  ${If} $REINSTALL_UNINSTALL == "1"
    SendMessage $REINSTALL_UNINSTALLBUTTON ${BM_SETCHECK} 1 0
  ${Else}
    SendMessage $R0 ${BM_SETCHECK} 1 0
  ${EndIf}

  ${If} $SKIPLICENSE == 1
    EnableWindow $mui.Button.Back 0
  ${Else}
    EnableWindow $mui.Button.Back 1
  ${Endif}

  nsDialogs::Show

FunctionEnd

Function CleanLeftovers

  DetailPrint "Cleaning leftover files"
  SetDetailsPrint none

  Delete "$INSTDIR\libgcc_s_sjlj-1.dll"
  Delete "$INSTDIR\libstdc++-6.dll"
  Delete "$INSTDIR\libwinpthread-1.dll"
  Delete "$INSTDIR\mingwm10.dll"

  Delete "$INSTDIR\resources\dialogs.xrc"
  Delete "$INSTDIR\resources\filezilla.xpm"
  Delete "$INSTDIR\resources\inputdialog.xrc"
  Delete "$INSTDIR\resources\menus.xrc"
  Delete "$INSTDIR\resources\netconfwizard.xrc"
  Delete "$INSTDIR\resources\quickconnectbar.xrc"
  Delete "$INSTDIR\resources\settings.xrc"
  Delete "$INSTDIR\resources\themes.xml"
  Delete "$INSTDIR\resources\toolbar.xrc"
  Delete "$INSTDIR\resources\update.xrc"
  Delete "$INSTDIR\resources\xxquickconnectbar.xrc"
  Delete "$INSTDIR\resources\xxtoolbar.xrc"
  Delete "$INSTDIR\resources\xrc\toolbar.xrc"
  RMDir "$INSTDIR\resources"

  Delete "$INSTDIR\locales\ar_EG\filezilla.mo"
  Delete "$INSTDIR\locales\bg\filezilla.mo"
  Delete "$INSTDIR\locales\ca_ES\filezilla.mo"
  Delete "$INSTDIR\locales\cs\filezilla.mo"
  Delete "$INSTDIR\locales\da_DK\filezilla.mo"
  Delete "$INSTDIR\locales\de_DE\filezilla.mo"
  Delete "$INSTDIR\locales\el_GR\filezilla.mo"
  Delete "$INSTDIR\locales\es_ES\filezilla.mo"
  Delete "$INSTDIR\locales\et_EE\filezilla.mo"
  Delete "$INSTDIR\locales\eu_ES\filezilla.mo"
  Delete "$INSTDIR\locales\fi\filezilla.mo"
  Delete "$INSTDIR\locales\fr_CA\filezilla.mo"
  Delete "$INSTDIR\locales\fr_FR\filezilla.mo"
  Delete "$INSTDIR\locales\gl\filezilla.mo"
  Delete "$INSTDIR\locales\gr\filezilla.mo"
  Delete "$INSTDIR\locales\hu\filezilla.mo"
  Delete "$INSTDIR\locales\it_IT\filezilla.mo"
  Delete "$INSTDIR\locales\km\filezilla.mo"
  Delete "$INSTDIR\locales\lt\filezilla.mo"
  Delete "$INSTDIR\locales\lv\filezilla.mo"
  Delete "$INSTDIR\locales\mk\filezilla.mo"
  Delete "$INSTDIR\locales\nl_NL\filezilla.mo"
  Delete "$INSTDIR\locales\ru_RU\filezilla.mo"
  Delete "$INSTDIR\locales\sk\filezilla.mo"
  Delete "$INSTDIR\locales\sl\filezilla.mo"
  Delete "$INSTDIR\locales\sv_SE\filezilla.mo"
  Delete "$INSTDIR\locales\tr_TR\filezilla.mo"

  RMDir "$INSTDIR\locales\ar_EG"
  RMDir "$INSTDIR\locales\bg"
  RMDir "$INSTDIR\locales\ca_ES"
  RMDir "$INSTDIR\locales\cs"
  RMDir "$INSTDIR\locales\da_DK"
  RMDir "$INSTDIR\locales\de_DE"
  RMDir "$INSTDIR\locales\el_GR"
  RMDir "$INSTDIR\locales\es_ES"
  RMDir "$INSTDIR\locales\et_EE"
  RMDir "$INSTDIR\locales\eu_ES"
  RMDir "$INSTDIR\locales\fi"
  RMDir "$INSTDIR\locales\fr_CA"
  RMDir "$INSTDIR\locales\fr_FR"
  RMDir "$INSTDIR\locales\gl"
  RMDir "$INSTDIR\locales\gr"
  RMDir "$INSTDIR\locales\hu"
  RMDir "$INSTDIR\locales\it_IT"
  RMDir "$INSTDIR\locales\km"
  RMDir "$INSTDIR\locales\lt"
  RMDir "$INSTDIR\locales\lv"
  RMDir "$INSTDIR\locales\mk"
  RMDir "$INSTDIR\locales\nl_NL"
  RMDir "$INSTDIR\locales\ru_RU"
  RMDir "$INSTDIR\locales\sk"
  RMDir "$INSTDIR\locales\sl"
  RMDir "$INSTDIR\locales\sv_SE"
  RMDir "$INSTDIR\locales\tr_TR"

  RMDir "$INSTDIR\locales"

  SetDetailsPrint lastused
FunctionEnd

Function RunUninstaller

  ${If} $ALL_USERS_DEFAULT == 1
    ReadRegStr $R1 HKLM "${UNINSTALLER_REGKEY}" "UninstallString"
  ${Else}
    ReadRegStr $R1 HKCU "${UNINSTALLER_REGKEY}" "UninstallString"
  ${EndIf}

  ${If} $R1 == ""
    Return
  ${EndIf}

  ; Run uninstaller
  HideWindow

  ClearErrors

  ${If} $REINSTALL_UNINSTALL == "2"
  ${AndIf} $PREVIOUS_VERSION_STATE == "same"
    StrCpy $R2 ""
  ${Else}
    StrCpy $R2 "/frominstall /keepstartmenudir"
  ${EndIf}

  ${If} $QUIET == 1
    StrCpy $R2 "$R2 /quiet"
  ${EndIf}

  ${If} ${Silent}
    StrCpy $R2 "$R2 /S"
  ${EndIf}

  StrCpy $R3 $R1 1
  ${If} $R3 == '"'
    ExecWait '$R1 $R2 _?=$INSTDIR'
  ${Else}
    ExecWait '"$R1" $R2 _?=$INSTDIR'
  ${EndIf}

  IfErrors no_remove_uninstaller

  IfFileExists "$INSTDIR\uninstall.exe" 0 no_remove_uninstaller

  Delete "$R1"

  ; Remove directory only if uninstalling for good
  ${If} $REINSTALL_UNINSTALL == "2"
    RMDir $INSTDIR
  ${EndIf}

 no_remove_uninstaller:

  BringToFront
  Sleep 500

FunctionEnd

Function PageLeaveReinstall

  SendMessage $REINSTALL_UNINSTALLBUTTON ${BM_GETCHECK} 0 0 $R0
  ${If} $R0 == 1
    ; Option to uninstall old version selected
    StrCpy $REINSTALL_UNINSTALL 1
  ${Else}
    ; Custom up/downgrade or add/remove/reinstall
    StrCpy $REINSTALL_UNINSTALL 2
    ${If} $PREVIOUS_VERSION_STATE == "same"
      Call RunUninstaller
      Quit
    ${EndIf}
  ${EndIf}

  ; Need to reload defaults. User could have
  ; chosen custom, change something, went back and selected
  ; the express option.
  StrCpy $ALL_USERS $ALL_USERS_DEFAULT
  Call UpdateShellVarContext
  Call LoadPreviousSettings

FunctionEnd

Function PageAllUsers

  ${If} $REINSTALL_UNINSTALL == 1
  ${AndIf} $PREVIOUS_VERSION_STATE != "same"
    ; Keep same settings
    Abort
  ${EndIf}

  ${If} $PERFORM_UPDATE == 1
    StrCpy $REINSTALL_UNINSTALL 1
    Abort
  ${EndIf}

  !insertmacro MUI_HEADER_TEXT "Choose Installation Options" "Who should this application be installed for?"

  nsDialogs::Create /NOUNLOAD 1018
  Pop $0

  nsDialogs::CreateItem /NOUNLOAD STATIC ${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS} 0 0 0 100% 30 "Please select whether you wish to make this software available to all users or just yourself."
  Pop $R0

  ${If} $IS_ADMIN == 1
    StrCpy $R2 ${BS_AUTORADIOBUTTON}|${BS_VCENTER}|${BS_MULTILINE}|${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS}|${WS_GROUP}|${WS_TABSTOP}
  ${Else}
    StrCpy $R2 ${BS_AUTORADIOBUTTON}|${BS_VCENTER}|${BS_MULTILINE}|${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS}|${WS_GROUP}|${WS_TABSTOP}|${WS_DISABLED}
  ${EndIf}
  nsDialogs::CreateItem /NOUNLOAD BUTTON $R2 0 10 55 100% 30 "&Anyone who uses this computer (all users)"
  Pop $ALL_USERS_BUTTON

  StrCpy $R2 ${BS_AUTORADIOBUTTON}|${BS_TOP}|${BS_MULTILINE}|${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS}

  ${If} $USERNAME != ""
    nsDialogs::CreateItem /NOUNLOAD BUTTON $R2 0 10 85 100% 50 "&Only for me ($USERNAME)"
  ${Else}
    nsDialogs::CreateItem /NOUNLOAD BUTTON $R2 0 10 85 100% 50 "&Only for me"
  ${EndIf}
  Pop $R0

  ${If} $PREVIOUS_VERSION != ""
    ${If} $ALL_USERS_DEFAULT == 1
      nsDialogs::CreateItem /NOUNLOAD STATIC ${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS} 0 0 -30 100% 30 "Step Test Assistant has been previously installed for all users."
    ${Else}
      nsDialogs::CreateItem /NOUNLOAD STATIC ${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS} 0 0 -30 100% 30 "Step Test Assistant has been previously installed for this user only."
    ${EndIf}
  ${Else}
    nsDialogs::CreateItem /NOUNLOAD STATIC ${WS_VISIBLE}|${WS_CHILD}|${WS_CLIPSIBLINGS} 0 0 -30 100% 30 "Installation for all users requires Administrator privileges."
  ${EndIf}
  Pop $R1

  ${If} $ALL_USERS == "1"
    SendMessage $ALL_USERS_BUTTON ${BM_SETCHECK} 1 0
  ${Else}
    SendMessage $R0 ${BM_SETCHECK} 1 0
  ${EndIf}

  ${If} $PREVIOUS_VERSION == ""
    ${IF} $SKIPLICENSE == 1
      EnableWindow $mui.Button.Back 0
    ${ENDIF}
  ${EndIf}

  nsDialogs::Show

FunctionEnd

Function PageLeaveAllUsers

  SendMessage $ALL_USERS_BUTTON ${BM_GETCHECK} 0 0 $R0
  ${If} $R0 == 0
    StrCpy $ALL_USERS "0"
  ${Else}
    StrCpy $ALL_USERS "1"
  ${EndIf}
  Call UpdateShellVarContext

FunctionEnd

Function PageComponentsPre

  ${If} $PERFORM_UPDATE == 1
    Abort
  ${EndIf}

  ${If} $REINSTALL_UNINSTALL == 1
  ${AndIf} $PREVIOUS_VERSION_STATE != "same"

    Abort

  ${EndIf}

FunctionEnd

Function PageDirectoryPre

  ${If} $PERFORM_UPDATE == 1
    Abort
  ${EndIf}

  ${If} $REINSTALL_UNINSTALL == "1"
  ${AndIf} $PREVIOUS_VERSION_STATE != "same"

    Abort

  ${EndIf}

FunctionEnd

Function PageStartmenuPre

  ${If} $PERFORM_UPDATE == 1
    Abort
  ${EndIf}

  ${If} $REINSTALL_UNINSTALL == "1"
  ${AndIf} $PREVIOUS_VERSION_STATE != "same"

    ${If} "$STARTMENU_FOLDER" == ""

      StrCpy "$STARTMENU_FOLDER" ">"

    ${EndIf}

    Abort

  ${EndIf}

FunctionEnd

Function CheckStepTestAssistantRunning

  Push "StepTestAssistant.exe"
  Call IsProcessRunning
  Pop $R1

  ${If} $PERFORM_UPDATE == 1
    StrCpy $R0 10
  ${Else}
    StrCpy $R0 2
  ${EndIf}

  ${While} $R1 != ''

    ${If} $R0 == 0

      ${ExitWhile}

    ${EndIf}

    Sleep 500

    Push "StepTestAssistant.exe"
    Call IsProcessRunning
    Pop $R1

    IntOp $R0 $R0 - 1

  ${EndWhile}

  Push "StepTestAssistant.exe"
  Call IsProcessRunning
  Pop $R1

  ${While} $R1 != ''

    ${MessageBox} MB_ABORTRETRYIGNORE|MB_DEFBUTTON2 "StepTestAssistant appears to be running.$\nPlease close all running instances of StepTestAssistant before continuing the installation." IDIGNORE 'IDABORT CheckStepTestAssistantRunning_abort IDIGNORE CheckStepTestAssistantRunning_ignore'

    Push "StepTestAssistant.exe"
    Call IsProcessRunning
    Pop $R1

  ${EndWhile}

 CheckStepTestAssistantRunning_ignore:
  Return

 CheckStepTestAssistantRunning_abort:
  Quit

FunctionEnd

Function .OnInstFailed
FunctionEnd

Function .onInstSuccess

  ${MementoSectionSave}

  ; Detect multiple install directories
  ${If} $OldRunDir != ''
    ${GetFileVersion} $OldRunDir $R0
    ${GetFileVersion} "$Instdir\StepTestAssistant.exe" $R1

    StrCpy $R2 $OldRunDir -14

    ${If} $R0 != ''
    ${AndIf} $R1 != ''
    ${AndIf} $R0 != $R1
      ${MessageBox} MB_ICONEXCLAMATION 'Multiple installations of Step Test Assistant detected.$\n$\nStep Test Assistant ${VERSION} has been installed to "$InstDir".$\nAn old installation of Step Test Assistant $R0 still exists in the "$R2" directory.$\n$\nPlease delete the old version in the "$R2" directory.' IDOK ''
    ${EndIf}
  ${EndIf}

FunctionEnd

;--------------------------------
; Languages
;--------------------------------

  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
; Installer Sections
;--------------------------------

!macro INSTALLICONSET SET
  DetailPrint "Installing ${SET} icon set"
  SetDetailsPrint none
  SetOutPath "$INSTDIR\resources\${SET}"
  File "${top_srcdir}/src\interface\resources\${SET}\theme.xml"
  File /r "${top_srcdir}/src\interface\resources\${SET}\*.png"
  File /nonfatal /r "${top_srcdir}/src\interface\resources\${SET}\*.gif"
  SetDetailsPrint lastused
!macroend

Section "Step Test Assistant Client" SecMain

  SectionIn 1 RO

  Call CheckStepTestAssistantRunning

  ${If} $REINSTALL_UNINSTALL != ""

    Call RunUninstaller

  ${EndIf}

  Call CleanLeftovers


  SetOutPath "$INSTDIR"

!ifndef DUMMY_INSTALLER

  File "..\src\interface\${LT_EXEDIR}FileZilla.exe"
  File "..\src\putty\${LT_EXEDIR}fzsftp.exe"
  File "..\src\putty\${LT_EXEDIR}fzputtygen.exe"
!if /FileExists "..\src\storj\${LT_EXEDIR}fzstorj.exe"
  File "..\src\storj\${LT_EXEDIR}fzstorj.exe"
!endif

  File "${top_srcdir}\GPL.html"
  File "${top_srcdir}\NEWS"
  File "${top_srcdir}\AUTHORS"

  !include "dll_gui_install.nsh"

  DetailPrint "Installing resource files"
  SetDetailsPrint none

  SetOutPath "$INSTDIR\resources"
  File "${top_srcdir}/src\interface\resources\defaultfilters.xml"
  File "${top_srcdir}/src\interface\resources\finished.wav"

  SetOutPath "$INSTDIR\resources\xrc"
  File "${top_srcdir}/src\interface\resources\xrc\*.xrc"

  SetOutPath "$INSTDIR\resources\16x16"
  File "${top_srcdir}/src\interface\resources\16x16\*.png"
  File "${top_srcdir}/src\interface\resources\16x16\*.gif"

  SetOutPath "$INSTDIR\resources\20x20"
  File "${top_srcdir}/src\interface\resources\20x20\*.png"

  SetOutPath "$INSTDIR\resources\24x24"
  File "${top_srcdir}/src\interface\resources\24x24\*.png"

  SetOutPath "$INSTDIR\resources\32x32"
  File "${top_srcdir}/src\interface\resources\32x32\*.png"

  SetOutPath "$INSTDIR\resources\48x48"
  File "${top_srcdir}/src\interface\resources\48x48\*.png"

  SetOutPath "$INSTDIR\resources\480x480"
  File "${top_srcdir}/src\interface\resources\480x480\*.png"

  !insertmacro INSTALLICONSET default

  SetOutPath "$INSTDIR\docs"
  File "${top_srcdir}\docs\fzdefaults.xml.example"

!endif
  SetDetailsPrint lastused

  ; Create uninstaller
  WriteUninstaller "$INSTDIR\uninstall.exe"

  WriteRegStr SHCTX "${INSTALLER_REGKEY}" "" $INSTDIR
  WriteRegStr SHCTX "${INSTALLER_REGKEY}" "Version" "${VERSION}"

  ${If} $PERFORM_UPDATE == 0
    ; Open question: How to deal with non-installer packages, e.g. the zip archive?
!ifdef ENABLE_OFFERS
    WriteRegDWORD SHCTX "${INSTALLER_REGKEY}" "Package" 2
!else
    WriteRegDWORD SHCTX "${INSTALLER_REGKEY}" "Package" 1
!endif

    ClearErrors
    ${GetOptions} $CommandLine "/channel" $R1
    ${Unless} ${Errors}
      WriteRegStr SHCTX "${INSTALLER_REGKEY}" "Channel" $R1
    ${EndUnless}

  ${EndIf}

  WriteRegDWORD SHCTX "${INSTALLER_REGKEY}" "Updated" $PERFORM_UPDATE

  ${StrRep} $R0 "$INSTDIR\uninstall.exe" '"' '""'
  WriteRegExpandStr SHCTX "${UNINSTALLER_REGKEY}" "UninstallString" '"$R0"'

  WriteRegExpandStr SHCTX "${UNINSTALLER_REGKEY}" "InstallLocation" "$INSTDIR"
  WriteRegStr   SHCTX "${UNINSTALLER_REGKEY}"   "DisplayName"    "Step Test Assistant ${VERSION}"
  WriteRegStr   SHCTX "${UNINSTALLER_REGKEY}"   "DisplayIcon"    "$INSTDIR\StepTestAssistant.exe"
  WriteRegStr   SHCTX "${UNINSTALLER_REGKEY}"   "DisplayVersion" "${VERSION}"
  WriteRegStr   SHCTX "${UNINSTALLER_REGKEY}"   "URLInfoAbout"   "${WEBSITE_URL}"
  WriteRegStr   SHCTX "${UNINSTALLER_REGKEY}"   "URLUpdateInfo"  "${WEBSITE_URL}"
  WriteRegStr   SHCTX "${UNINSTALLER_REGKEY}"   "HelpLink"       "${WEBSITE_URL}"
  WriteRegStr   SHCTX "${UNINSTALLER_REGKEY}"   "Publisher"      "${PUBLISHER}"
  WriteRegDWORD SHCTX "${UNINSTALLER_REGKEY}"   "VersionMajor"   "${VERSION_MAJOR}"
  WriteRegDWORD SHCTX "${UNINSTALLER_REGKEY}"   "VersionMinor"   "${VERSION_MINOR}"
  WriteRegDWORD SHCTX "${UNINSTALLER_REGKEY}"   "NoModify"       "1"
  WriteRegDWORD SHCTX "${UNINSTALLER_REGKEY}"   "NoRepair"       "1"

  Call GetInstalledSize
  WriteRegDWORD SHCTX "${UNINSTALLER_REGKEY}" "EstimatedSize" "$GetInstalledSize.total" ; Create/Write the reg key with the dword value

  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application

  ; Create shortcuts
  SetOutPath "$INSTDIR"
  CreateDirectory "$SMPROGRAMS\$STARTMENU_FOLDER"
  CreateShortCut "$SMPROGRAMS\$STARTMENU_FOLDER\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
  CreateShortCut "$SMPROGRAMS\$STARTMENU_FOLDER\StepTestAssistant.lnk" "$INSTDIR\StepTetsAssistant.exe"

  ${If} ${AtLeastWin7}
    ; Setting AppID
    push "StepTestAssistant.Client.AppID"
    push "$SMPROGRAMS\$STARTMENU_FOLDER\StepTestAssistant.lnk"
    nsis_appid::set_appid
  ${EndIf}

  !insertmacro MUI_STARTMENU_WRITE_END

  Push $R0
  StrCpy $R0 "$STARTMENU_FOLDER" 1
  ${if} $R0 == ">"
    ; Write folder to registry
    WriteRegStr "${MUI_STARTMENUPAGE_Application_REGISTRY_ROOT}" "${MUI_STARTMENUPAGE_Application_REGISTRY_KEY}" "${MUI_STARTMENUPAGE_Application_REGISTRY_VALUENAME}" ">"
  ${endif}
  Pop $R0

  ${If} $ALL_USERS == 1
    ; Enable mini dumps
    ${If} ${RunningX64}
      SetRegView 64
    ${EndIf}
    WriteRegDWORD HKLM "${DUMP_KEY}\filezilla.exe"  "DumpType" "1"
    WriteRegDWORD HKLM "${DUMP_KEY}\fzsftp.exe"     "DumpType" "1"
    WriteRegDWORD HKLM "${DUMP_KEY}\fzputtygen.exe" "DumpType" "1"
!if /FileExists "..\src\storj\${LT_EXEDIR}fzstorj.exe"
    WriteRegDWORD HKLM "${DUMP_KEY}\fzstorj.exe"    "DumpType" "1"
!endif
    ${If} ${RunningX64}
      SetRegView lastused
    ${EndIf}
  ${EndIf}

  ; Register App Path so that typing filezilla in Win+R dialog starts FileZilla
  WriteRegStr SHCTX "Software\Microsoft\Windows\CurrentVersion\App Paths\StepTestAssistant.exe" "" "$INSTDIR\StepTestAssistant.exe"
  WriteRegStr SHCTX "Software\Microsoft\Windows\CurrentVersion\App Paths\StepTestAssistant.exe" "Path" "$INSTDIR"

SectionEnd

${MementoSection} "Icon sets" SecIconSets

!ifndef DUMMY_INSTALLER
  !insertmacro INSTALLICONSET blukis
  !insertmacro INSTALLICONSET classic
  !insertmacro INSTALLICONSET cyril
  !insertmacro INSTALLICONSET flatzilla
  !insertmacro INSTALLICONSET lone
  !insertmacro INSTALLICONSET minimal
  !insertmacro INSTALLICONSET opencrystal
  !insertmacro INSTALLICONSET sun
  !insertmacro INSTALLICONSET tango
!endif

${MementoSectionEnd}

!if "@FILEZILLA_LINGUAS@" != ""

  !macro INSTALLLANGFILE FILE LANG OUTFILE

     SetOutPath "$INSTDIR\locales\${LANG}"
     File /oname=${OUTFILE} ${FILE}

  !macroend

  ${MementoSection} "Language files" SecLang

    DetailPrint "Installing language files"
    SetDetailsPrint none

    ; installlangfiles.nsh is generated by configure and just contains a series of
    ; !insertmacro INSTALLLANGFILE <lang>
!ifndef DUMMY_INSTALLER
    !include installlangfiles.nsh
!endif

    SetDetailsPrint lastused

  ${MementoSectionEnd}

!endif

${MementoSection} "Shell Extension" SecShellExt

!ifndef DUMMY_INSTALLER
  SetOutPath "$INSTDIR"
  !define LIBRARY_SHELL_EXTENSION
  !define LIBRARY_COM
  !define LIBRARY_IGNORE_VERSION

  ${If} ${RunningX64}
    SetRegView 32
  ${EndIf}
  !insertmacro InstallLib REGDLL NOTSHARED REBOOT_NOTPROTECTED ../src/fzshellext/32/.libs\libfzshellext-0.dll $INSTDIR\fzshellext.dll "$INSTDIR"
  ${If} ${RunningX64}
    SetRegView lastused
  ${EndIf}

  !define LIBRARY_X64
  ${If} ${RunningX64}
    !insertmacro InstallLib REGDLL NOTSHARED REBOOT_NOTPROTECTED ../src/fzshellext/64/.libs\libfzshellext-0.dll $INSTDIR\fzshellext_64.dll "$INSTDIR"
  ${Else}
    !insertmacro InstallLib DLL NOTSHARED REBOOT_NOTPROTECTED ../src/fzshellext/64/.libs\libfzshellext-0.dll $INSTDIR\fzshellext_64.dll "$INSTDIR"
  ${Endif}
  !undef LIBRARY_X64
!endif

${MementoSectionEnd}

${MementoUnselectedSection} "Desktop Icon" SecDesktop

  SetOutPath "$INSTDIR"
  CreateShortCut "$DESKTOP\Step Test Assistant.lnk" "$INSTDIR\StepTestAssistant.exe" "" "$INSTDIR\StepTestAssistant.exe" 0

  ${If} ${AtLeastWin7}
    ; Setting AppID
    push "StepTestAssistant.Client.AppID"
    push "$DESKTOP\StepTestAssistant.lnk"
    nsis_appid::set_appid
  ${EndIf}

${MementoSectionEnd}

${MementoSectionDone}

;--------------------------------
; Functions
;--------------------------------

Function PostInstPage

  ; Don't advance automatically if details expanded
  FindWindow $R0 "#32770" "" $HWNDPARENT
  GetDlgItem $R0 $R0 1016
  System::Call user32::IsWindowVisible(i$R0)i.s
  Pop $R0

  ${If} $R0 != 0
    SetAutoClose false
  ${EndIf}

FunctionEnd

Function PageFinishPre
  ${If} $PERFORM_UPDATE == 1
    !insertmacro UAC_AsUser_ExecShell '' '$INSTDIR\StepTestAssistant.exe' '' '' SW_SHOWNORMAL
    Quit
  ${EndIf}
FunctionEnd

Function CustomFinishRun

  !insertmacro UAC_AsUser_ExecShell '' '$INSTDIR\StepTestAssistant.exe' '' '' SW_SHOWNORMAL

FunctionEnd

Function GetInstalledSize
  Push $0
  Push $1
  StrCpy $GetInstalledSize.total 0
  ${ForEach} $1 0 256 + 1
    ${if} ${SectionIsSelected} $1
      SectionGetSize $1 $0
      IntOp $GetInstalledSize.total $GetInstalledSize.total + $0
    ${Endif}
  ${Next}
  Pop $1
  Pop $0
  IntFmt $GetInstalledSize.total "0x%08X" $GetInstalledSize.total
  Push $GetInstalledSize.total
FunctionEnd

;--------------------------------
; Descriptions
;--------------------------------

  ; Language strings
  LangString DESC_SecMain ${LANG_ENGLISH} "Required program files."
  LangString DESC_SecIconSets ${LANG_ENGLISH} "Additional icon sets."
!if "@FILEZILLA_LINGUAS@" != ""
  LangString DESC_SecLang ${LANG_ENGLISH} "Language files."
!endif
  LangString DESC_SecShellExt ${LANG_ENGLISH} "Shell extension for advanced drag && drop support. Required for drag && drop from FileZilla into Explorer."
  LangString DESC_SecDesktop ${LANG_ENGLISH} "Create desktop icon for Step Test Assistant"

  ; Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecMain} $(DESC_SecMain)
    !insertmacro MUI_DESCRIPTION_TEXT ${SecIconSets} $(DESC_SecIconSets)
!if "@FILEZILLA_LINGUAS@" != ""
    !insertmacro MUI_DESCRIPTION_TEXT ${SecLang} $(DESC_SecLang)
!endif
    !insertmacro MUI_DESCRIPTION_TEXT ${SecShellExt} $(DESC_SecShellExt)
    !insertmacro MUI_DESCRIPTION_TEXT ${SecDesktop} $(DESC_SecDesktop)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
; Uninstaller Variables
;--------------------------------

Var un.REMOVE_ALL_USERS
Var un.REMOVE_CURRENT_USER

;--------------------------------
; Uninstaller Functions
;--------------------------------

Function un.GetUserInfo
  ClearErrors
  UserInfo::GetName
  ${If} ${Errors}
    StrCpy $IS_ADMIN 1
    Return
  ${EndIf}
  Pop $USERNAME

  ${If} ${UAC_IsAdmin}
    StrCpy $IS_ADMIN 1
  ${Else}
    StrCpy $IS_ADMIN 0
  ${EndIf}

FunctionEnd

Function un.ReadPreviousVersion

  ReadRegStr $R0 HKLM "${INSTALLER_REGKEY}" ""

  ${If} $R0 != ""
    ; Detect version
    ReadRegStr $R2 HKLM "${INSTALLER_REGKEY}" "Version"
    ${If} $R2 == ""
      StrCpy $R0 ""
    ${EndIf}
  ${EndIf}

  ReadRegStr $R1 HKCU "${INSTALLER_REGKEY}" ""

  ${If} $R1 != ""
    ; Detect version
    ReadRegStr $R2 HKCU "${INSTALLER_REGKEY}" "Version"
    ${If} $R2 == ""
      StrCpy $R1 ""
    ${EndIf}
  ${EndIf}

  ${If} $R1 == $INSTDIR
    Strcpy $un.REMOVE_CURRENT_USER 1
  ${EndIf}
  ${If} $R0 == $INSTDIR
    Strcpy $un.REMOVE_ALL_USERS 1
  ${EndIf}
  ${If} $un.REMOVE_CURRENT_USER != 1
  ${AndIf} $un.REMOVE_ALL_USERS != 1
    ${If} $R1 != ""
      Strcpy $un.REMOVE_CURRENT_USER 1
      ${If} $R0 == $R1
        Strcpy $un.REMOVE_ALL_USERS 1
      ${EndIf}
    ${Else}
      StrCpy $un.REMOVE_ALL_USERS = 1
    ${EndIf}
  ${EndIf}

FunctionEnd

Function un.onInit

  ${un.GetParameters} $CommandLine

  ${un.GetOptions} $CommandLine "/quiet" $R1
  ${If} ${Errors}
    StrCpy $Quiet 0
  ${Else}
    StrCpy $Quiet 1
    SetSilent silent
  ${EndIf}

  Call un.GetUserInfo
  Call un.ReadPreviousVersion

  ${If} $un.REMOVE_ALL_USERS == 1
  ${AndIf} $IS_ADMIN == 0
    ${MessageBox} MB_ICONSTOP "Step Test Assistant has been installed for all users.$\nPlease restart the uninstaller with Administrator privileges to remove it." IDOK ''
    Abort
  ${EndIf}

FunctionEnd

Function un.RemoveStartmenu

  !insertmacro MUI_STARTMENU_GETFOLDER Application $MUI_TEMP

  Delete "$SMPROGRAMS\$MUI_TEMP\Uninstall.lnk"
  Delete "$SMPROGRAMS\$MUI_TEMP\StepTestAssistant.lnk"

  ${un.GetOptions} $CommandLine "/keepstartmenudir" $R1
  ${If} ${Errors}

    ; Delete empty start menu parent diretories
    StrCpy $MUI_TEMP "$SMPROGRAMS\$MUI_TEMP"

    startMenuDeleteLoop:
      RMDir $MUI_TEMP
      GetFullPathName $MUI_TEMP "$MUI_TEMP\.."

      IfErrors startMenuDeleteLoopDone

      StrCmp $MUI_TEMP $SMPROGRAMS startMenuDeleteLoopDone startMenuDeleteLoop
    startMenuDeleteLoopDone:

  ${EndUnless}

FunctionEnd

Function un.ConfirmPagePre

  ${un.GetOptions} $CommandLine "/frominstall" $R1
  ${Unless} ${Errors}
    Abort
  ${EndUnless}

FunctionEnd

Function un.FinishPagePre

  ${un.GetOptions} $CommandLine "/frominstall" $R1
  ${Unless} ${Errors}
    SetRebootFlag false
    Abort
  ${EndUnless}

FunctionEnd

;--------------------------------
; Uninstaller Section
;--------------------------------

!if "@FILEZILLA_LINGUAS@" != ""

  !macro UNINSTALLLANGFILE LANG FILENAME

    Delete "$INSTDIR\locales\${LANG}\${FILENAME}"
    RMDir "$INSTDIR\locales\${LANG}"

  !macroend

!endif

!macro UNINSTALLICONSET SET

  DetailPrint "Removing ${SET} icon set"
  SetDetailsPrint none

  Delete "$INSTDIR\resources\${SET}\theme.xml"
  Delete "$INSTDIR\resources\${SET}\16x16\*.png"
  Delete "$INSTDIR\resources\${SET}\20x20\*.png"
  Delete "$INSTDIR\resources\${SET}\24x24\*.png"
  Delete "$INSTDIR\resources\${SET}\32x32\*.png"
  Delete "$INSTDIR\resources\${SET}\48x48\*.png"
  Delete "$INSTDIR\resources\${SET}\480x480\*.png"
  Delete "$INSTDIR\resources\${SET}\16x16\*.gif"
  Delete "$INSTDIR\resources\${SET}\20x20\*.gif"
  Delete "$INSTDIR\resources\${SET}\24x24\*.gif"
  Delete "$INSTDIR\resources\${SET}\32x32\*.gif"
  Delete "$INSTDIR\resources\${SET}\48x48\*.gif"
  RMDir "$INSTDIR\resources\${SET}\16x16"
  RMDir "$INSTDIR\resources\${SET}\20x20"
  RMDir "$INSTDIR\resources\${SET}\24x24"
  RMDir "$INSTDIR\resources\${SET}\32x32"
  RMDir "$INSTDIR\resources\${SET}\48x48"
  RMDir "$INSTDIR\resources\${SET}\480x480"
  RMDir "$INSTDIR\resources\${SET}"

  SetDetailsPrint lastused

!macroend

Section "Uninstall"

  ${If} ${RunningX64}
    SetRegView 32
  ${EndIf}
  !insertmacro UnInstallLib REGDLL NOTSHARED REBOOT_NOTPROTECTED "$INSTDIR\fzshellext.dll"
  ${If} ${RunningX64}
    SetRegView lastused
  ${EndIf}

  !define LIBRARY_X64
  ${If} ${RunningX64}
    !insertmacro UnInstallLib REGDLL NOTSHARED REBOOT_NOTPROTECTED "$INSTDIR\fzshellext_64.dll"
  ${Else}
    !insertmacro UnInstallLib DLL NOTSHARED REBOOT_NOTPROTECTED "$INSTDIR\fzshellext_64.dll"
  ${Endif}
  !undef LIBRARY_X64

  Delete "$INSTDIR\StepTestAssistant.exe"
  Delete "$INSTDIR\fzsftp.exe"
  Delete "$INSTDIR\fzputtygen.exe"
!if /FileExists "..\src\storj\${LT_EXEDIR}fzstorj.exe"
  Delete "$INSTDIR\fzstorj.exe"
!endif

  Delete "$INSTDIR\GPL.html"
  Delete "$INSTDIR\NEWS"
  Delete "$INSTDIR\AUTHORS"

  !include "dll_gui_uninstall.nsh"

  DetailPrint "Removing resource files"
  SetDetailsPrint none

  Delete "$INSTDIR\resources\xrc\*.xrc"
  Delete "$INSTDIR\resources\defaultfilters.xml"
  Delete "$INSTDIR\resources\finished.wav"
  Delete "$INSTDIR\resources\16x16\*.png"
  Delete "$INSTDIR\resources\20x20\*.png"
  Delete "$INSTDIR\resources\24x24\*.png"
  Delete "$INSTDIR\resources\32x32\*.png"
  Delete "$INSTDIR\resources\48x48\*.png"
  Delete "$INSTDIR\resources\480x480\*.png"
  Delete "$INSTDIR\resources\16x16\*.gif"
  Delete "$INSTDIR\resources\32x32\*.gif"

  SetDetailsPrint lastused

  !insertmacro UNINSTALLICONSET blukis
  !insertmacro UNINSTALLICONSET classic
  !insertmacro UNINSTALLICONSET cyril
  !insertmacro UNINSTALLICONSET default
  !insertmacro UNINSTALLICONSET flatzilla
  !insertmacro UNINSTALLICONSET lone
  !insertmacro UNINSTALLICONSET minimal
  !insertmacro UNINSTALLICONSET opencrystal
  !insertmacro UNINSTALLICONSET sun
  !insertmacro UNINSTALLICONSET tango

!if "@FILEZILLA_LINGUAS@" != ""

  DetailPrint "Removing language files"
  SetDetailsPrint none

  ; uninstalllangfiles.nsh is generated by configure and just contains a series of
  ; !insertmacro UNINSTALLLANGFILE <lang>
  !include uninstalllangfiles.nsh

  SetDetailsPrint lastused

!endif

  Delete "$INSTDIR\uninstall.exe"

  Delete "$INSTDIR\docs\fzdefaults.xml.example"

!if "@FILEZILLA_LINGUAS@" != ""
  RMDir "$INSTDIR\locales"
!endif
  RMDir "$INSTDIR\resources\480x480"
  RMDir "$INSTDIR\resources\48x48"
  RMDir "$INSTDIR\resources\32x32"
  RMDir "$INSTDIR\resources\20x20"
  RMDir "$INSTDIR\resources\24x24"
  RMDir "$INSTDIR\resources\16x16"
  RMDir "$INSTDIR\resources\xrc"
  RMDir "$INSTDIR\resources"
  RMDir "$INSTDIR\docs"

  ${un.GetOptions} $CommandLine "/frominstall" $R1
  ${If} ${Errors}
    RMDir /REBOOTOK "$INSTDIR"

    DeleteRegValue SHCTX "${INSTALLER_REGKEY}" "Package"
    DeleteRegValue SHCTX "${INSTALLER_REGKEY}" "Updated"
    DeleteRegValue SHCTX "${INSTALLER_REGKEY}" "Channel"
  ${EndIf}

  ${If} $un.REMOVE_ALL_USERS == 1
    SetShellVarContext all
    Call un.RemoveStartmenu

    DeleteRegKey /ifempty HKLM "${INSTALLER_REGKEY}"
    DeleteRegKey HKLM "${UNINSTALLER_REGKEY}"

    Delete "$DESKTOP\FileZilla Client.lnk"

    ; Remove dump key
    ${If} ${RunningX64}
      SetRegView 64
    ${EndIf}
    DeleteRegValue HKLM "${DUMP_KEY}\StepTestAssistant.exe"  "DumpType"
    DeleteRegValue HKLM "${DUMP_KEY}\fzsftp.exe"     "DumpType"
    DeleteRegValue HKLM "${DUMP_KEY}\fzputtygen.exe" "DumpType"
!if /FileExists "..\src\storj\${LT_EXEDIR}fzstorj.exe"
    DeleteRegValue HKLM "${DUMP_KEY}\fzstorj.exe"    "DumpType"
!endif
    DeleteRegKey /ifempty HKLM "${DUMP_KEY}\StepTestAssistant.exe"
    DeleteRegKey /ifempty HKLM "${DUMP_KEY}\fzsftp.exe"
    DeleteRegKey /ifempty HKLM "${DUMP_KEY}\fzputtygen.exe"
!if /FileExists "..\src\storj\${LT_EXEDIR}fzstorj.exe"
    DeleteRegKey /ifempty HKLM "${DUMP_KEY}\fzstorj.exe"
!endif
    ${If} ${RunningX64}
      SetRegView lastused
    ${EndIf}
  ${EndIf}

  ${If} $un.REMOVE_CURRENT_USER == 1
    SetShellVarContext current
    Call un.RemoveStartmenu

    DeleteRegKey /ifempty HKCU "${INSTALLER_REGKEY}"
    DeleteRegKey HKCU "${UNINSTALLER_REGKEY}"

    Delete "$DESKTOP\StepTestAssistant.lnk"
  ${EndIf}

  DeleteRegKey SHCTX "Software\Microsoft\Windows\CurrentVersion\App Paths\StepTestAssistant.exe"

SectionEnd