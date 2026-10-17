IF OBJECT_ID(N'[cafestock].[__EFMigrationsHistory]') IS NULL
BEGIN
    IF SCHEMA_ID(N'cafestock') IS NULL EXEC(N'CREATE SCHEMA [cafestock];');
    CREATE TABLE [cafestock].[__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    IF SCHEMA_ID(N'cafestock') IS NULL EXEC(N'CREATE SCHEMA [cafestock];');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[Branches] (
        [Id] uniqueidentifier NOT NULL,
        [Code] nvarchar(160) NOT NULL,
        [Name] nvarchar(160) NOT NULL,
        [Address] nvarchar(500) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Branches] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[BusinessSettings] (
        [Id] uniqueidentifier NOT NULL,
        [OperatorName] nvarchar(160) NOT NULL,
        [Version] rowversion NOT NULL,
        [BusinessName] nvarchar(160) NOT NULL,
        CONSTRAINT [PK_BusinessSettings] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[Categories] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(160) NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[MigrationHistory] (
        [Id] uniqueidentifier NOT NULL,
        [SourceFingerprint] nvarchar(64) NOT NULL,
        [AppliedAt] datetime2 NOT NULL,
        [ProductCount] int NOT NULL,
        [EmployeeCount] int NOT NULL,
        [MovementCount] int NOT NULL,
        [CountCount] int NOT NULL,
        CONSTRAINT [PK_MigrationHistory] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[Roles] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(160) NOT NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[Units] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(160) NOT NULL,
        CONSTRAINT [PK_Units] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[StockAreas] (
        [Id] uniqueidentifier NOT NULL,
        [Code] nvarchar(160) NOT NULL,
        [Name] nvarchar(160) NOT NULL,
        [Location] nvarchar(160) NOT NULL,
        [BranchId] uniqueidentifier NOT NULL,
        [Area] int NOT NULL,
        CONSTRAINT [PK_StockAreas] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_StockAreas_Kind] CHECK ([Area] IN (1,2)),
        CONSTRAINT [FK_StockAreas_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [cafestock].[Branches] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[Employees] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(160) NOT NULL,
        [Phone] nvarchar(160) NOT NULL,
        [HireDate] datetime2 NULL,
        [RoleId] uniqueidentifier NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Employees_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [cafestock].[Roles] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[Products] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(160) NOT NULL,
        [CategoryId] uniqueidentifier NOT NULL,
        [UnitId] uniqueidentifier NOT NULL,
        [MinimumStock] decimal(18,6) NOT NULL,
        [Code] nvarchar(160) NOT NULL,
        [SourceCell] nvarchar(160) NOT NULL,
        [ImageKey] nvarchar(160) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Products_MinimumStock] CHECK ([MinimumStock] >= 0),
        CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [cafestock].[Categories] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Products_Units_UnitId] FOREIGN KEY ([UnitId]) REFERENCES [cafestock].[Units] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[ImportedLocations] (
        [ImportId] uniqueidentifier NOT NULL,
        [SourceWarehouseId] uniqueidentifier NOT NULL,
        [AreaId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_ImportedLocations] PRIMARY KEY ([ImportId], [SourceWarehouseId]),
        CONSTRAINT [FK_ImportedLocations_MigrationHistory_ImportId] FOREIGN KEY ([ImportId]) REFERENCES [cafestock].[MigrationHistory] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ImportedLocations_StockAreas_AreaId] FOREIGN KEY ([AreaId]) REFERENCES [cafestock].[StockAreas] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[EmployeeBranches] (
        [EmployeeId] uniqueidentifier NOT NULL,
        [BranchId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_EmployeeBranches] PRIMARY KEY ([EmployeeId], [BranchId]),
        CONSTRAINT [FK_EmployeeBranches_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [cafestock].[Branches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_EmployeeBranches_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [cafestock].[Employees] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[StockCounts] (
        [Id] uniqueidentifier NOT NULL,
        [SourceWarehouseId] uniqueidentifier NOT NULL,
        [ReviewedName] nvarchar(160) NOT NULL,
        [BranchId] uniqueidentifier NOT NULL,
        [SubmittedById] uniqueidentifier NULL,
        [SubmittedName] nvarchar(160) NOT NULL,
        [SubmittedAt] datetime2 NOT NULL,
        [Status] int NOT NULL,
        [ReviewedById] uniqueidentifier NULL,
        [ReviewedAt] datetime2 NULL,
        [ReviewNote] nvarchar(2000) NOT NULL,
        [IsDemo] bit NOT NULL,
        [Version] rowversion NOT NULL,
        CONSTRAINT [PK_StockCounts] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_StockCounts_Status] CHECK ([Status] IN (1,2,3)),
        CONSTRAINT [FK_StockCounts_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [cafestock].[Branches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_StockCounts_Employees_ReviewedById] FOREIGN KEY ([ReviewedById]) REFERENCES [cafestock].[Employees] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_StockCounts_Employees_SubmittedById] FOREIGN KEY ([SubmittedById]) REFERENCES [cafestock].[Employees] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[Transfers] (
        [Id] uniqueidentifier NOT NULL,
        [SourceAreaId] uniqueidentifier NOT NULL,
        [TargetAreaId] uniqueidentifier NOT NULL,
        [CreatedById] uniqueidentifier NULL,
        [CreatedAt] datetime2 NOT NULL,
        [Note] nvarchar(2000) NOT NULL,
        [IsDemo] bit NOT NULL,
        CONSTRAINT [PK_Transfers] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Transfers_DifferentAreas] CHECK ([SourceAreaId] <> [TargetAreaId]),
        CONSTRAINT [FK_Transfers_Employees_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [cafestock].[Employees] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Transfers_StockAreas_SourceAreaId] FOREIGN KEY ([SourceAreaId]) REFERENCES [cafestock].[StockAreas] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Transfers_StockAreas_TargetAreaId] FOREIGN KEY ([TargetAreaId]) REFERENCES [cafestock].[StockAreas] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[UserAccounts] (
        [EmployeeId] uniqueidentifier NOT NULL,
        [Username] nvarchar(160) NOT NULL,
        [UsernameKey] nvarchar(160) NOT NULL,
        [PasswordHash] nvarchar(256) NOT NULL,
        [PasswordSalt] nvarchar(256) NOT NULL,
        [IsEnabled] bit NOT NULL,
        CONSTRAINT [PK_UserAccounts] PRIMARY KEY ([EmployeeId]),
        CONSTRAINT [FK_UserAccounts_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [cafestock].[Employees] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[StockCountLines] (
        [CountId] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [ExpectedBar] decimal(18,6) NULL,
        [CountedBar] decimal(18,6) NULL,
        [ExpectedDepot] decimal(18,6) NULL,
        [CountedDepot] decimal(18,6) NULL,
        [Expected] decimal(18,6) NOT NULL,
        [Counted] decimal(18,6) NOT NULL,
        CONSTRAINT [PK_StockCountLines] PRIMARY KEY ([CountId], [ProductId]),
        CONSTRAINT [CK_StockCountLines_Quantities] CHECK ([Expected] >= 0 AND [Counted] >= 0 AND ([ExpectedBar] IS NULL OR [ExpectedBar] >= 0) AND ([ExpectedDepot] IS NULL OR [ExpectedDepot] >= 0) AND ([CountedBar] IS NULL OR [CountedBar] >= 0) AND ([CountedDepot] IS NULL OR [CountedDepot] >= 0)),
        CONSTRAINT [FK_StockCountLines_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [cafestock].[Products] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_StockCountLines_StockCounts_CountId] FOREIGN KEY ([CountId]) REFERENCES [cafestock].[StockCounts] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[StockMovements] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [AreaId] uniqueidentifier NOT NULL,
        [OccurredAt] datetime2 NOT NULL,
        [Type] nvarchar(160) NOT NULL,
        [Quantity] decimal(18,6) NOT NULL,
        [Note] nvarchar(2000) NOT NULL,
        [ActorId] uniqueidentifier NULL,
        [CountId] uniqueidentifier NULL,
        [TransferId] uniqueidentifier NULL,
        [IsDemo] bit NOT NULL,
        CONSTRAINT [PK_StockMovements] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_StockMovements_Employees_ActorId] FOREIGN KEY ([ActorId]) REFERENCES [cafestock].[Employees] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_StockMovements_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [cafestock].[Products] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_StockMovements_StockAreas_AreaId] FOREIGN KEY ([AreaId]) REFERENCES [cafestock].[StockAreas] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_StockMovements_StockCounts_CountId] FOREIGN KEY ([CountId]) REFERENCES [cafestock].[StockCounts] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_StockMovements_Transfers_TransferId] FOREIGN KEY ([TransferId]) REFERENCES [cafestock].[Transfers] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE TABLE [cafestock].[TransferLines] (
        [TransferId] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Quantity] decimal(18,6) NOT NULL,
        CONSTRAINT [PK_TransferLines] PRIMARY KEY ([TransferId], [ProductId]),
        CONSTRAINT [CK_TransferLines_Quantity] CHECK ([Quantity] > 0),
        CONSTRAINT [FK_TransferLines_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [cafestock].[Products] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TransferLines_Transfers_TransferId] FOREIGN KEY ([TransferId]) REFERENCES [cafestock].[Transfers] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Branches_Name] ON [cafestock].[Branches] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Categories_Name] ON [cafestock].[Categories] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_EmployeeBranches_BranchId] ON [cafestock].[EmployeeBranches] ([BranchId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_Employees_RoleId] ON [cafestock].[Employees] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_ImportedLocations_AreaId] ON [cafestock].[ImportedLocations] ([AreaId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE UNIQUE INDEX [IX_MigrationHistory_SourceFingerprint] ON [cafestock].[MigrationHistory] ([SourceFingerprint]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Products_CategoryId_Name] ON [cafestock].[Products] ([CategoryId], [Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_Products_UnitId] ON [cafestock].[Products] ([UnitId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Roles_Name] ON [cafestock].[Roles] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE UNIQUE INDEX [IX_StockAreas_BranchId_Area] ON [cafestock].[StockAreas] ([BranchId], [Area]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_StockCountLines_ProductId] ON [cafestock].[StockCountLines] ([ProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_StockCounts_BranchId] ON [cafestock].[StockCounts] ([BranchId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_StockCounts_ReviewedById] ON [cafestock].[StockCounts] ([ReviewedById]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_StockCounts_SubmittedById] ON [cafestock].[StockCounts] ([SubmittedById]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_StockMovements_ActorId] ON [cafestock].[StockMovements] ([ActorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_StockMovements_AreaId_ProductId_OccurredAt] ON [cafestock].[StockMovements] ([AreaId], [ProductId], [OccurredAt]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_StockMovements_CountId] ON [cafestock].[StockMovements] ([CountId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_StockMovements_ProductId] ON [cafestock].[StockMovements] ([ProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_StockMovements_TransferId] ON [cafestock].[StockMovements] ([TransferId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_TransferLines_ProductId] ON [cafestock].[TransferLines] ([ProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_Transfers_CreatedById] ON [cafestock].[Transfers] ([CreatedById]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_Transfers_SourceAreaId] ON [cafestock].[Transfers] ([SourceAreaId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE INDEX [IX_Transfers_TargetAreaId] ON [cafestock].[Transfers] ([TargetAreaId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Units_Name] ON [cafestock].[Units] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_UserAccounts_UsernameKey] ON [cafestock].[UserAccounts] ([UsernameKey]) WHERE [IsEnabled] = 1');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [cafestock].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261016215511_InitialElazig'
)
BEGIN
    INSERT INTO [cafestock].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261016215511_InitialElazig', N'8.0.32');
END;
GO

COMMIT;
GO

