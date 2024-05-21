IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [GoodsCategories] (
    [GoodsCategoryId] int NOT NULL IDENTITY,
    [GoodsCategoryName] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [ImageCode] nvarchar(max) NOT NULL,
    [IsDeleted] bit NOT NULL,
    [Remark] nvarchar(max) NOT NULL,
    [CreateTime] datetime2 NOT NULL,
    [UpdateTime] datetime2 NOT NULL,
    [UpdateBy] nvarchar(max) NOT NULL,
    [CreateBy] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_GoodsCategories] PRIMARY KEY ([GoodsCategoryId])
);
GO

CREATE TABLE [StoreHouses] (
    [StoreHouseId] int NOT NULL IDENTITY,
    [StoreHouseName] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [IsDeleted] bit NOT NULL,
    [Remark] nvarchar(max) NOT NULL,
    [CreateTime] datetime2 NOT NULL,
    [UpdateTime] datetime2 NOT NULL,
    [UpdateBy] nvarchar(max) NOT NULL,
    [CreateBy] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_StoreHouses] PRIMARY KEY ([StoreHouseId])
);
GO

CREATE TABLE [Goods] (
    [GoodsId] int NOT NULL IDENTITY,
    [GoodsName] nvarchar(max) NOT NULL,
    [Number] float NOT NULL,
    [GoodsCategoryId] int NOT NULL,
    [StoreHouseId] int NOT NULL,
    [Price] float NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [IsDeleted] bit NOT NULL,
    [Remark] nvarchar(max) NOT NULL,
    [CreateTime] datetime2 NOT NULL,
    [UpdateTime] datetime2 NOT NULL,
    [UpdateBy] nvarchar(max) NOT NULL,
    [CreateBy] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Goods] PRIMARY KEY ([GoodsId]),
    CONSTRAINT [FK_Goods_GoodsCategories_GoodsCategoryId] FOREIGN KEY ([GoodsCategoryId]) REFERENCES [GoodsCategories] ([GoodsCategoryId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Goods_StoreHouses_StoreHouseId] FOREIGN KEY ([StoreHouseId]) REFERENCES [StoreHouses] ([StoreHouseId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Goods_GoodsCategoryId] ON [Goods] ([GoodsCategoryId]);
GO

CREATE INDEX [IX_Goods_StoreHouseId] ON [Goods] ([StoreHouseId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240308013621_Initial', N'8.0.2');
GO

COMMIT;
GO

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

BEGIN TRANSACTION;
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Records]') AND [c].[name] = N'CreateBy');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Records] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Records] DROP COLUMN [CreateBy];
GO

ALTER TABLE [Records] ADD [IsDeleted] tinyint NOT NULL DEFAULT CAST(0 AS tinyint);
GO

CREATE TABLE [WareHouseRecord] (
    [WareHouseRecordId] int NOT NULL IDENTITY,
    [SupplierName] nvarchar(max) NOT NULL,
    [IsDeleted] bit NOT NULL,
    [Remark] nvarchar(max) NULL,
    [CreateTime] datetime2 NOT NULL,
    [UpdateTime] datetime2 NULL,
    [UpdateBy] nvarchar(max) NULL,
    [CreateBy] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_WareHouseRecord] PRIMARY KEY ([WareHouseRecordId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240311083539_add_table_warehouse', N'8.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Permission] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Role] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(450) NOT NULL,
    [IsDeleted] bit NOT NULL,
    [Remark] nvarchar(max) NULL,
    [CreateTime] datetime2 NOT NULL,
    [UpdateTime] datetime2 NULL,
    [UpdateBy] nvarchar(max) NULL,
    [CreateBy] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [role_permission] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] int NOT NULL,
    [PermissionId] int NOT NULL,
    [IsDeleted] bit NOT NULL,
    [Remark] nvarchar(max) NULL,
    [CreateTime] datetime2 NOT NULL,
    [UpdateTime] datetime2 NULL,
    [UpdateBy] nvarchar(max) NULL,
    [CreateBy] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_role_permission] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [User] (
    [Id] int NOT NULL IDENTITY,
    [UserName] nvarchar(30) NOT NULL,
    [PassWord] nvarchar(30) NOT NULL,
    [Phone] nvarchar(11) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [IsDeleted] bit NOT NULL,
    [Remark] nvarchar(200) NULL,
    [CreateTime] datetime2 NOT NULL,
    [UpdateTime] datetime2 NULL,
    [UpdateBy] nvarchar(max) NULL,
    [CreateBy] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UserRole] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    [IsDeleted] bit NOT NULL,
    [Remark] nvarchar(max) NULL,
    [CreateTime] datetime2 NOT NULL,
    [UpdateTime] datetime2 NULL,
    [UpdateBy] nvarchar(max) NULL,
    [CreateBy] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY ([Id])
);
GO

CREATE UNIQUE INDEX [IX_Role_Name] ON [Role] ([Name]);
GO

CREATE UNIQUE INDEX [IX_User_Phone] ON [User] ([Phone]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240425085642_Add_Authentication', N'8.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [WareHouseRecord];
GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Records]') AND [c].[name] = N'WareHouseRecordId');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Records] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [Records] DROP COLUMN [WareHouseRecordId];
GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[User]') AND [c].[name] = N'UserName');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [User] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [User] ALTER COLUMN [UserName] nvarchar(100) NOT NULL;
GO

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[User]') AND [c].[name] = N'PassWord');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [User] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [User] ALTER COLUMN [PassWord] nvarchar(300) NOT NULL;
GO

CREATE INDEX [IX_Records_GoodsId] ON [Records] ([GoodsId]);
GO

CREATE INDEX [IX_Records_StoreHouseId] ON [Records] ([StoreHouseId]);
GO

ALTER TABLE [Records] ADD CONSTRAINT [FK_Records_Goods_GoodsId] FOREIGN KEY ([GoodsId]) REFERENCES [Goods] ([GoodsId]) ON DELETE CASCADE;
GO

ALTER TABLE [Records] ADD CONSTRAINT [FK_Records_StoreHouses_StoreHouseId] FOREIGN KEY ([StoreHouseId]) REFERENCES [StoreHouses] ([StoreHouseId]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240502024818_update_record_cloumn', N'8.0.2');
GO

COMMIT;
GO

