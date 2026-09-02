USE [M1_ME];
GO
SET XACT_ABORT ON;
GO
BEGIN TRANSACTION;
IF COL_LENGTH(N'SalesOrders.Orders', N'M1BillingLocationID') IS NULL ALTER TABLE SalesOrders.Orders ADD M1BillingLocationID nvarchar(30) NULL;
IF COL_LENGTH(N'SalesOrders.Orders', N'M1BillingContactID') IS NULL ALTER TABLE SalesOrders.Orders ADD M1BillingContactID nvarchar(30) NULL;
IF COL_LENGTH(N'SalesOrders.Orders', N'MatchValidationJson') IS NULL ALTER TABLE SalesOrders.Orders ADD MatchValidationJson nvarchar(max) NULL;
IF COL_LENGTH(N'SalesOrders.Orders', N'AddressOverrideJson') IS NULL ALTER TABLE SalesOrders.Orders ADD AddressOverrideJson nvarchar(max) NULL;
IF COL_LENGTH(N'SalesOrders.Orders', N'LineOverridesJson') IS NULL ALTER TABLE SalesOrders.Orders ADD LineOverridesJson nvarchar(max) NULL;
IF NOT EXISTS (SELECT 1 FROM SalesOrders.SchemaVersions WHERE VersionNumber=2)
  INSERT SalesOrders.SchemaVersions (VersionNumber,Description) VALUES (2,N'Separate billing selections, address validation, and line overrides');
COMMIT TRANSACTION;
GO
