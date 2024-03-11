BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Records]') AND [c].[name] = N'CreateBy');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Records] DROP CONSTRAINT [' + @var0 + '];');
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

CREATE INDEX [IX_Records_WareHouseRecordId] ON [Records] ([WareHouseRecordId]);
GO

ALTER TABLE [Records] ADD CONSTRAINT [FK_Records_WareHouseRecord_WareHouseRecordId] FOREIGN KEY ([WareHouseRecordId]) REFERENCES [WareHouseRecord] ([WareHouseRecordId]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240311082200_add_table_warehouse', N'8.0.2');
GO

COMMIT;
GO

