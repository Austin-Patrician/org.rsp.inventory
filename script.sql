BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[StoreHouses]') AND [c].[name] = N'UpdateTime');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [StoreHouses] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [StoreHouses] ALTER COLUMN [UpdateTime] datetime2 NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[StoreHouses]') AND [c].[name] = N'UpdateBy');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [StoreHouses] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [StoreHouses] ALTER COLUMN [UpdateBy] nvarchar(max) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[StoreHouses]') AND [c].[name] = N'Remark');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [StoreHouses] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [StoreHouses] ALTER COLUMN [Remark] nvarchar(max) NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[StoreHouses]') AND [c].[name] = N'Location');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [StoreHouses] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [StoreHouses] ALTER COLUMN [Location] nvarchar(max) NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[StoreHouses]') AND [c].[name] = N'Description');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [StoreHouses] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [StoreHouses] ALTER COLUMN [Description] nvarchar(max) NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GoodsCategories]') AND [c].[name] = N'UpdateTime');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [GoodsCategories] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [GoodsCategories] ALTER COLUMN [UpdateTime] datetime2 NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GoodsCategories]') AND [c].[name] = N'UpdateBy');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [GoodsCategories] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [GoodsCategories] ALTER COLUMN [UpdateBy] nvarchar(max) NULL;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GoodsCategories]') AND [c].[name] = N'Remark');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [GoodsCategories] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [GoodsCategories] ALTER COLUMN [Remark] nvarchar(max) NULL;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GoodsCategories]') AND [c].[name] = N'ImageCode');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [GoodsCategories] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [GoodsCategories] ALTER COLUMN [ImageCode] nvarchar(max) NULL;
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GoodsCategories]') AND [c].[name] = N'Description');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [GoodsCategories] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [GoodsCategories] ALTER COLUMN [Description] nvarchar(max) NULL;
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Goods]') AND [c].[name] = N'UpdateTime');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Goods] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Goods] ALTER COLUMN [UpdateTime] datetime2 NULL;
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Goods]') AND [c].[name] = N'UpdateBy');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Goods] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [Goods] ALTER COLUMN [UpdateBy] nvarchar(max) NULL;
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Goods]') AND [c].[name] = N'Remark');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Goods] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [Goods] ALTER COLUMN [Remark] nvarchar(max) NULL;
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Goods]') AND [c].[name] = N'Description');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Goods] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [Goods] ALTER COLUMN [Description] nvarchar(max) NULL;
GO

CREATE TABLE [Records] (
    [RecordId] int NOT NULL IDENTITY,
    [Direction] tinyint NOT NULL,
    [GoodsId] int NOT NULL,
    [Quantity] float NOT NULL,
    [StoreHouseId] int NOT NULL,
    [TradeTime] datetime2 NOT NULL,
    [CreateBy] nvarchar(max) NOT NULL,
    [Use] nvarchar(max) NOT NULL,
    [WareHouseRecordId] int NOT NULL,
    CONSTRAINT [PK_Records] PRIMARY KEY ([RecordId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240311052510_add_table_record', N'8.0.2');
GO

COMMIT;
GO

