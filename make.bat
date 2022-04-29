
@DEL bin\Release\*.exe

@"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" PTouchPrintSender.sln /p:configuration="Release"

@DIR /S /B bin\*.exe
