@SET PP_MSBUILD=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe

@CALL cargo fmt
@CALL cargo run %*
