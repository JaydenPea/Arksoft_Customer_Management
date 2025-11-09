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
CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [Address] nvarchar(500) NOT NULL,
    [TelephoneNumber] nvarchar(20) NULL,
    [ContactPersonName] nvarchar(100) NULL,
    [ContactPersonEmail] nvarchar(255) NULL,
    [VatNumber] nvarchar(50) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);

CREATE INDEX [IX_Customers_ContactPersonEmail] ON [Customers] ([ContactPersonEmail]);

CREATE INDEX [IX_Customers_Name] ON [Customers] ([Name]);

CREATE INDEX [IX_Customers_VatNumber] ON [Customers] ([VatNumber]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251109083056_InitialCreate', N'9.0.0');

COMMIT;
GO

