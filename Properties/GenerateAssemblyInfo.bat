::-----------------------------
@ECHO OFF
SETLOCAL ENABLEEXTENSIONS


REM ��ȡ����

SET workDir=%1
SET template=%2
SET target=%3

GOTO MAIN
::=============================


::-----------------------------
:MAIN
pushd %workDir%
SET workDir=.

REM ������
IF %workDir%=="" GOTO ARGUMENT_ERROR
IF %template%=="" GOTO ARGUMENT_ERROR
IF %target%=="" GOTO ARGUMENT_ERROR

REM ��ѯע���
@echo off 
for /f "tokens=1,2,* " %%i in ('REG QUERY "HKLM\SOFTWARE\TortoiseSVN" ^| find /i "Directory"') do set "TSVN_PATH=%%k" 

REM ���� TSVN �滻ģ��

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
ECHO ����Ĳ�����Ч��
GOTO FAIL

:NOT_FOUND_TSVN
ECHO ��ѯTortoiseSVN �İ�װ��Ϣʧ�ܡ�
GOTO FAIL

:UNKNOW_ERROR
ECHO ���ɳ�����Ϣ����δ֪����
:FAIL
::=============================

::-----------------------------
::** Program exit
:FAIL
::DEL /Q %TSVN_INFO_FILE% 2>NUL
ECHO ���ɳ�����Ϣʧ�ܡ�
popd
EXIT 1

:SUCESSED
ECHO ���ɳ�����Ϣ�ɹ���
popd
EXIT 0
::=============================