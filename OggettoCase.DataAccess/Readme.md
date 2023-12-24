# Обновление миграций
1. Найти в Solution Explorer'е проект `Host`. Выбрать в контекстом меню пункт Set as StartUp Project.
2. Открыть окно Tools -> NuGet Package Manager -> Package Manager Console.
3. В выпадающем списке `Default project`, расположенном в верхней части окна, выбрать `OggettoCase.DataContracts.DataAccess`.
4. Создать новую миграцию с помощью команды:

```shell
Add-Migration Initial -Context DataBaseContext
```
Или
```shell
dotnet ef migrations add InitialCreate --context DatabaseContext --output-dir Migrations/  --project ../OggettoCase.DataAccess --startup-project ../Host/ 
```

5. Создать SQL-скрипт с помощью команды:
```shell
Script-Migration -Context DataBaseContext -From InitialCreate -To addRouter
```
Или
```shell
dotnet ef migrations script --project ../OggettoCase.DataAccess --startup-project ../Host/ -o ../OggettoCase.DataAccess/Migrations/SQL/InitialCreate.sql
```

6. [Опционально] Обновить базу данных командой 
```shell
dotnet ef database update --project ./src/OggettoCase.DataAccess --startup-project ./src/Host/
```

