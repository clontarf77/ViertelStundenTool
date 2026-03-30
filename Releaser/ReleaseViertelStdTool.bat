@ECHO OFF

set tool=ViertelStdTool
set version=3_0_2_7
set releasePath=\LimitOrders15minGUI\bin\Release

md %tool%_%version%
md %tool%_%version%\V_%version%
md %tool%_%version%\V_%version%\Aligne
md %tool%_%version%\V_%version%\Comtrader
md %tool%_%version%\V_%version%\LocalFiles
md %tool%_%version%\V_%version%\SFTP\PrivateKey

xcopy ..\%releasePath%\*.dll %tool%_%version%\V_%version%
xcopy ..\%releasePath%\*.exe %tool%_%version%\V_%version%
xcopy ..\%releasePath%\*.exe.config %tool%_%version%\V_%version%
xcopy  ..\%releasePath%\Aligne %tool%_%version%\V_%version%\Aligne /S
xcopy ..\%releasePath%\SFTP\PrivateKey %tool%_%version%\V_%version%\SFTP\PrivateKey

"C:\Program Files\7-Zip\7z" a -tzip %tool%_%version%\%tool%_%version%.zip .\%tool%_%version%\V_%version%\*

pause





