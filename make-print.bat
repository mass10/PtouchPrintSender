@REM
@REM 印刷を行います。
@REM

@REM ==================== dry-run 設定 ====================
@SET PT_DRYRUN=true
@SET PT_DRYRUN=

@CALL bin\Release\PtouchPrintSender.exe %*
