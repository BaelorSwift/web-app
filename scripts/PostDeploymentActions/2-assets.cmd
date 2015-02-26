:: Install Node Modules, execute grunt tasks
:: -0xdeafcafe

call cd "D:\home\site\approot\src\BaelorApi\"
call npm install
call grunt default

:: Move assets from physical wwwroot, to virtual wwwroot
IF EXIST "D:\home\site\wwwroot\css" (
  RD /S /Q "D:\home\site\wwwroot\css"
)
Xcopy /s /e /h /i /y "D:\home\site\approot\src\BaelorApi\wwwroot\css" "D:\home\site\wwwroot\css"
IF EXIST "D:\home\site\wwwroot\js" (
  RD /S /Q "D:\home\site\wwwroot\js"
)
Xcopy /s /e /h /i /y "D:\home\site\approot\src\BaelorApi\wwwroot\js" "D:\home\site\wwwroot\js"
IF EXIST "D:\home\site\wwwroot\lib" (
  RD /S /Q "D:\home\site\wwwroot\lib"
)
Xcopy /s /e /h /i /y "D:\home\site\approot\src\BaelorApi\wwwroot\lib" "D:\home\site\wwwroot\lib"
