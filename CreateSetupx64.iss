; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

;#define MyAppVersion "0.1.0"

#define MyAppName "PowerSettings"
#define MyAppExeName "PowerSettings.exe"
#define SetupExeName "PowerSettings_Setupx64"
#define MyAppPublisher "Saurav KS"
#define MyAppURL "https://clicksrv.github.io/PowerSettings"
#define CompileOutputPath ".\PowerSettings.App\bin\Release\net7.0-windows\win-x64\publish\"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{98001817-9598-4513-9DD1-61707E6B3F9C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
; Remove the following line to run in administrative install mode (install for all users.)
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=.\publish\
OutputBaseFilename={#SetupExeName}
SetupIconFile=.\PowerSettings.App\tray-light.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
FlatComponentsList=yes
DirExistsWarning=no
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; Flags: unchecked
Name: "enableautostart"; Description: "Start PowerSettings with Windows"; Flags: unchecked

[Registry]
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "{#MyAppName}"; ValueData: """{app}\{#MyAppExeName}"" --silent --delayed"; Flags: uninsdeletevalue;
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run"; ValueType: binary; ValueName: "{#MyAppName}"; ValueData: "03 00 00 00 00 00 00 00 00 00 00 00"; Flags: uninsdeletevalue;
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run"; ValueType: binary; ValueName: "{#MyAppName}"; ValueData: "02 00 00 00 00 00 00 00 00 00 00 00"; Tasks: enableautostart

[Files]
Source: "{#CompileOutputPath}{#MyAppExeName}"; DestDir: "{app}"; Flags: 
Source: "{#CompileOutputPath}*"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent