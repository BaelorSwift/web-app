:: Install Node Modules, execute grunt tasks
:: -0xdeafcafe

call cd "D:\home\site\approot\src\BaelorApi\"
call npm install
call cmd.exe /c grunt -b "D:\home\site\approot\src\BaelorApi\" --gruntfile "D:\home\site\approot\src\BaelorApi\gruntfile.js" bower
call cmd.exe /c grunt -b "D:\home\site\approot\src\BaelorApi\" --gruntfile "D:\home\site\approot\src\BaelorApi\gruntfile.js" sass
call cmd.exe /c grunt -b "D:\home\site\approot\src\BaelorApi\" --gruntfile "D:\home\site\approot\src\BaelorApi\gruntfile.js" typescript
