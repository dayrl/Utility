::-----------------------------
@ECHO OFF
SETLOCAL ENABLEEXTENSIONS


REM 获取参数

SET workDir=%1
SET template=%2
SET target=%3

GOTO MAIN
::=============================


::-----------------------------
:MAIN
pushd %workDir%
SET workDir=.

REM 检查参数
IF %workDir%=="" GOTO ARGUMENT_ERROR
IF %template%=="" GOTO ARGUMENT_ERROR
IF %target%=="" GOTO ARGUMENT_ERROR

REM 查询注册表
@echo off 
for /f "tokens=1,2,* " %%i in ('REG QUERY "HKLM\SOFTWARE\TortoiseSVN" ^| find /i "Directory"') do set "TSVN_PATH=%%k" 

REM 调用 TSVN 替换模板

::IF NOT %ERRORLEVEL% == 0 GOTO UNKNOW_ERROR
IF "%TSVN_PATH%"=="" (
   IF EXIST "C:\Program Files\TortoiseSVN\" (
		SET TSVN_PATH=C:\Program Files\TortoiseSVN\
   )  ELSE IF EXIST "D:\Program Files\TortoiseSVN\" (
        SET TSVN_PATH=D:\Program Files\TortoiseSVN\
   )  ELSE IF EXIST "E:\Program Files\TortoiseSVN\" (
        SET TSVN_PATH=E:\Program Files\TortoiseSVN\
   ) ELSE GOTO NOT_FOUND_TSVN
   
)

ECHO "%TSVN_PATH%bin\SubWCRev.exe" %workDir% %template% %target% -f
"%TSVN_PATH%bin\SubWCRev.exe" %workDir% %template% %target% -f > NUL


IF NOT %ERRORLEVEL% == 0 GOTO UNKNOW_ERROR
GOTO SUCESSED
::=============================


::-----------------------------
::** Error handlers

:ARGUMENT_ERROR
ECHO 传入的参数无效。
GOTO FAIL

:NOT_FOUND_TSVN
ECHO 查询TortoiseSVN 的安装信息失败。
GOTO FAIL

:UNKNOW_ERROR
ECHO 生成程序集信息出现未知错误。
:FAIL
::=============================

::-----------------------------
::** Program exit
:FAIL
::DEL /Q %TSVN_INFO_FILE% 2>NUL
ECHO 生成程序集信息失败。
popd
EXIT 1

:SUCESSED
ECHO 生成程序集信息成功。
popd
EXIT 0
::=============================