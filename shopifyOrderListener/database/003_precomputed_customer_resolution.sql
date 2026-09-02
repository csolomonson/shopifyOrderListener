USE [M1_ME];
GO
SET XACT_ABORT ON;
GO
BEGIN TRANSACTION;

IF COL_LENGTH(N'SalesOrders.Orders', N'CustomerResolutionJson') IS NULL
  ALTER TABLE SalesOrders.Orders ADD CustomerResolutionJson nvarchar(max) NULL;

IF COL_LENGTH(N'SalesOrders.Orders', N'CustomerResolutionAt') IS NULL
  ALTER TABLE SalesOrders.Orders ADD CustomerResolutionAt datetime2(3) NULL;

IF NOT EXISTS (SELECT 1 FROM SalesOrders.SchemaVersions WHERE VersionNumber=3)
  INSERT SalesOrders.SchemaVersions (VersionNumber,Description)
  VALUES (3,N'Precomputed customer-resolution results for fast order review');

COMMIT TRANSACTION;
GO
