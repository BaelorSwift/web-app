:: Install Node Modules, execute grunt tasks
:: -0xdeafcafe

echo Executing Front-End asset management - npm/grunt
call cd "D:\home\site\approot\src\BaelorApi\"
call npm install
call grunt bower:install
call grunt sass
call grunt typescript
