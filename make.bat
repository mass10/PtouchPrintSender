@REM
@REM �r���h�p�̃����`���[�ł��B
@REM
@REM �y�g�p���@�z
@REM  make �����s����ƁA�A�v���P�[�V�����{�̂��r���h����܂��B
@REM
@REM �y���Ӂz
@REM  * Rust �ɂ���ċL�q����Ă��܂��Brustup ���K�v�ł��B
@REM

@SET PP_MSBUILD=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe

@cargo fmt
@cargo run %*
