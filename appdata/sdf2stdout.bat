@echo off
REM echo( %0
REM echo( %1

REM copy sdf locally because accessing sdf causes metadata changes in it (why microsoft, why?!)
REM SET SCRIPT_DIR=%~dp0
REM set tmpVar=%SCRIPT_DIR%_%date:~6,4%-%date:~3,2%-%date:~0,2%_%time::=.%.sdf
REM echo( %tmpVar%
REM copy /B "%1" "%tmpVar%"

SET SDF_FILE=%1
SET passwd=

REM PAZIENTI.SDF
echo(
echo( PAZIENTI.SDF - INTESTAZIONE REFERTO
sdf2csv.exe %SDF_FILE% %passwd% "SELECT * FROM intestazione_referto" stdout
echo(
echo( PAZIENTI.SDF - UTENTI
sdf2csv.exe %SDF_FILE% %passwd% "SELECT * FROM utenti" stdout
echo(
echo( PAZIENTI.SDF - PAZIENTI
sdf2csv.exe %SDF_FILE% %passwd% "SELECT * FROM pazienti" stdout
echo(
echo( PAZIENTI.SDF - ANALISI
sdf2csv.exe %SDF_FILE% %passwd% "SELECT * FROM analisi" stdout
echo(
echo( PAZIENTI.SDF - OUTPUT
sdf2csv.exe %SDF_FILE% %passwd% "SELECT * FROM Output" stdout
echo(
echo( PAZIENTI.SDF - ETNIA
sdf2csv.exe %SDF_FILE% %passwd% "SELECT * FROM etnia" stdout

REM EVENTS/LOGFILE.SDF
echo(echo(
echo( EVENTS/LOGFILE.SDF - FILEEPATH
sdf2csv.exe %SDF_FILE% %passwd% "SELECT * FROM fileepath" stdout
echo(
echo( EVENTS/LOGFILE.SDF - LOGS
sdf2csv.exe %SDF_FILE% %passwd% "SELECT * FROM logs" stdout

echo(
echo( INFORMATION_SCHEMA.COLUMNS
sdf2csv.exe %SDF_FILE% %passwd% "SELECT TABLE_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS" stdout

REM del %tmpVar%
