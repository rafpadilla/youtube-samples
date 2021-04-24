## Ejemplo del uso de Dapper

Para este ejemplo se utiliza esta tabla en una base de datos en localdb

```sql
CREATE TABLE [dbo].[Products]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] NVARCHAR(400) NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [Created] DATETIMEOFFSET NOT NULL
)
```


La página de Dapper con documentación sobre la librería: [https://dapper-tutorial.net/](https://dapper-tutorial.net/)