:: Install Node Modules, execute grunt tasks
:: -0xdeafcafe

call cd "D:\home\site\approot\src\BaelorApi\"
call npm install
call grunt bower:install
call grunt sass
call grunt typescript
