; Turn off old selected section
; 12 06 2005: Luis Wong
; Template to generate an installer
; Especially for the generation of EasyPlayer installers
; Trimedia Interactive Projects
 
; -------------------------------
; Start
  !define NAME "Step Test Assistant" 
  Name "${NAME}"
  !define MUI_FILE "StepTestAssistant"
  !define MUI_BRANDINGTEXT "${NAME}"
  CRCCheck On
 
  ; We should test if we must use an absolute path 
  !include "${NSISDIR}\Contrib\Modern UI\System.nsh"
  
;---------------------------------
;General
 
  OutFile "Setup.exe"
  ShowInstDetails "nevershow"
  ShowUninstDetails "nevershow"
  ;SetCompressor "bzip2"
 
  !define MUI_ICON "..\Resources\icons8-glucometer-32.ico"
  !define MUI_UNICON "..\Resources\icons8-glucometer-32.ico"
  !define MUI_SPECIALBITMAP "Bitmap.bmp"
  !define RELEASE "debug"
 
;--------------------------------
;Folder selection page
 
  InstallDir "$PROGRAMFILES\${NAME}"
 
 
;--------------------------------
;Modern UI Configuration
 
  !define MUI_WELCOMEPAGE  
  !define MUI_LICENSEPAGE
  !define MUI_DIRECTORYPAGE
  !define MUI_ABORTWARNING
  !define MUI_UNINSTALLER
  !define MUI_UNCONFIRMPAGE
  !define MUI_FINISHPAGE  
 
 
;--------------------------------
;Language
 
  !insertmacro MUI_LANGUAGE "English"
 
 
;--------------------------------
;Data
 
  LicenseData "Read_me.txt"
 
 
;-------------------------------- 
;Installer Sections     
Section "install" Installation
 
;Add files
  SetOutPath "$INSTDIR"
 
  File "..\bin\${RELEASE}\Autofac.dll"
  File "..\bin\${RELEASE}\Dapper.dll"
  File "..\bin\${RELEASE}\LanterneRouge.Fresno.Calculations.dll"
  File "..\bin\${RELEASE}\LanterneRouge.Fresno.Database.SQLite.dll"
  File "..\bin\${RELEASE}\LanterneRouge.Fresno.DataLayer.dll"
  File "..\bin\${RELEASE}\LanterneRouge.Fresno.Report.dll"
  File "..\bin\${RELEASE}\LanterneRouge.Wpf.dll"
  File "..\bin\${RELEASE}\log4net.dll"
  File "..\bin\${RELEASE}\MathNet.Numerics.dll"
  File "..\bin\${RELEASE}\Microsoft.Bcl.AsyncInterfaces.dll"
  File "..\bin\${RELEASE}\Microsoft.Expression.Interactions.dll"
  File "..\bin\${RELEASE}\MigraDoc.DocumentObjectModel-gdi.dll"
  File "..\bin\${RELEASE}\MigraDoc.Rendering-gdi.dll"
  File "..\bin\${RELEASE}\MigraDoc.RtfRendering-gdi.dll"
  File "..\bin\${RELEASE}\MRULib.dll"
  File "..\bin\${RELEASE}\Newtonsoft.Json.dll"
  File "..\bin\${RELEASE}\OxyPlot.dll"
  File "..\bin\${RELEASE}\OxyPlot.Pdf.dll"
  File "..\bin\${RELEASE}\OxyPlot.Wpf.dll"
  File "..\bin\${RELEASE}\OxyPlot.Wpf.Shared.dll"
  File "..\bin\${RELEASE}\PdfSharp.Charting-gdi.dll"
  File "..\bin\${RELEASE}\PdfSharp-gdi.dll"
  File "..\bin\${RELEASE}\PortableJsonSettingsProvider.dll"
  File "..\bin\${RELEASE}\${MUI_FILE}.exe"
  File "..\bin\${RELEASE}\${MUI_FILE}.exe.config"
  File "..\bin\${RELEASE}\System.Buffers.dll"
  File "..\bin\${RELEASE}\System.Collections.Immutable.dll"
  File "..\bin\${RELEASE}\System.ComponentModel.Annotations.dll"
  File "..\bin\${RELEASE}\System.Configuration.ConfigurationManager.dll"
  File "..\bin\${RELEASE}\System.Data.SQLite.dll"
  File "..\bin\${RELEASE}\System.Diagnostics.DiagnosticSource.dll"
  File "..\bin\${RELEASE}\System.Memory.dll"
  File "..\bin\${RELEASE}\System.Numerics.Vectors.dll"
  File "..\bin\${RELEASE}\System.Runtime.CompilerServices.Unsafe.dll"
  File "..\bin\${RELEASE}\System.Security.AccessControl.dll"
  File "..\bin\${RELEASE}\System.Security.Permissions.dll"
  File "..\bin\${RELEASE}\System.Security.Principal.Windows.dll"
  File "..\bin\${RELEASE}\System.Threading.Tasks.Extensions.dll"
  File "..\bin\${RELEASE}\System.ValueTuple.dll"
  File "..\bin\${RELEASE}\System.Windows.Interactivity.dll"
  SetOutPath "$INSTDIR\x64"
  File "..\bin\${RELEASE}\x64\SQLite.Interop.dll"
  SetOutPath "$INSTDIR\x86"
  File "..\bin\${RELEASE}\x86\SQLite.Interop.dll"  
 
;create desktop shortcut
  CreateShortCut "$DESKTOP\${NAME}.lnk" "$INSTDIR\${MUI_FILE}.exe" ""
 
;create start-menu items
  CreateDirectory "$SMPROGRAMS\${NAME}"
  CreateShortCut "$SMPROGRAMS\${NAME}\Uninstall.lnk" "$INSTDIR\Uninstall.exe" "" "$INSTDIR\Uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\${NAME}\${NAME}.lnk" "$INSTDIR\${MUI_FILE}.exe" "" "$INSTDIR\${MUI_FILE}.exe" 0
 
;write uninstall information to the registry
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "DisplayName" "${NAME} (remove only)"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}" "UninstallString" "$INSTDIR\Uninstall.exe"
 
  WriteUninstaller "$INSTDIR\Uninstall.exe"
 
SectionEnd
 
 
;--------------------------------    
;Uninstaller Section  
Section "Uninstall"
 
;Delete Files 
  RMDir /r "$INSTDIR\*.*"    
 
;Remove the installation directory
  RMDir "$INSTDIR"
 
;Delete Start Menu Shortcuts
  Delete "$DESKTOP\${NAME}.lnk"
  Delete "$SMPROGRAMS\${NAME}\*.*"
  RmDir  "$SMPROGRAMS\${NAME}"
 
;Delete Uninstaller And Unistall Registry Entries
  DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\${NAME}"
  DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${NAME}"  
 
SectionEnd
 
 
;--------------------------------    
;MessageBox Section
 
 
;Function that calls a messagebox when installation finished correctly
Function .onInstSuccess
  MessageBox MB_OK "You have successfully installed ${NAME}. Use the desktop icon to start the program."
FunctionEnd
 
Function un.onUninstSuccess
  MessageBox MB_OK "You have successfully uninstalled ${NAME}."
FunctionEnd
 
 
;eof