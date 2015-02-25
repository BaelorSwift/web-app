:: Move web.config from source repository to virtual wwwroot
:: -0xdeafcafe

echo Move web.config
IF EXIST "D:\home\site\wwwroot\web.config" (
  DEL /F /S /Q /A "D:\home\site\wwwroot\web.config"
)
call copy "D:\home\site\repository\scripts\web.config" "D:\home\site\wwwroot\web.config" /Y
