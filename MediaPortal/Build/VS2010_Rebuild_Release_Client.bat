
set ProgramDir=%ProgramFiles%
if exist "%ProgramFiles(x86)%" set ProgramDir=%ProgramFiles(x86)%

"%ProgramDir%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.com" ..\Source\MP2-Client.sln /Rebuild "Release|x86" /Out Build_Client.log
