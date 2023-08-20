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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230817133703_InitialModel')
BEGIN
    CREATE TABLE [TaxBands] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [LowerLimit] int NOT NULL,
        [UpperLimit] int NULL,
        [TaxRate] int NOT NULL,
        CONSTRAINT [PK_TaxBands] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230817133703_InitialModel')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'LowerLimit', N'Name', N'TaxRate', N'UpperLimit') AND [object_id] = OBJECT_ID(N'[TaxBands]'))
        SET IDENTITY_INSERT [TaxBands] ON;
    EXEC(N'INSERT INTO [TaxBands] ([Id], [LowerLimit], [Name], [TaxRate], [UpperLimit])
    VALUES (1, 0, N''A'', 0, 5000),
    (2, 5000, N''B'', 20, 20000),
    (3, 20000, N''C'', 40, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'LowerLimit', N'Name', N'TaxRate', N'UpperLimit') AND [object_id] = OBJECT_ID(N'[TaxBands]'))
        SET IDENTITY_INSERT [TaxBands] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230817133703_InitialModel')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230817133703_InitialModel', N'7.0.10');
END;
GO

COMMIT;
GO

